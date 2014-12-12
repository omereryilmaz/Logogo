using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Threading;
using System.Text;
using System.Device.Location;
using Microsoft.Phone.Maps.Toolkit;
using Microsoft.Phone.Maps;
using Windows.Devices.Geolocation;
using Microsoft.Phone.Maps.Controls;
using Microsoft.Phone.Maps.Services;

namespace LogoGO
{
   
    public partial class Oyun : PhoneApplicationPage
    {
        
        int logoIndisi=-1;
        int sayac = -1;
        int can = 3;
		int bildi =0;
        int saniye = 10;
        int skor = 0;
        int markaid;
        private ReverseGeocodeQuery MyReverseGeocodeQuery = null;
        private double _accuracy = 0.0;
        private GeoCoordinate MyCoordinate = null;
        int yaklas = 12;
        int logodiziindisi=0;
        HashSet<int> Logodizi = new HashSet<int>();
       

                        

        DispatcherTimer timer = new DispatcherTimer();
        
        public Oyun()
        {
            InitializeComponent();
			oyunEkranBas.Begin();
            loading.Begin();
            LoadingGrid.Visibility = Visibility.Visible;
            logoGetir();
            
            Loaded += Oyun_Loaded;
           
           

            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (s, e) =>
            {
                saniye = saniye - 1;
                sureTxt.Text = saniye.ToString();
                if (saniye == 0)
                {
                    timer.Stop();
                    //can--;
                    //canKontrol();
                    suredolduTxt1.Text = "Süreniz Doldu !";
                    gameover.Begin();
                }
            };
           
            
            
        }

        private void Oyun_Loaded(object sender, RoutedEventArgs e)
        {
            //Harita doğrulama kodları
            Microsoft.Phone.Maps.MapsSettings.ApplicationContext.ApplicationId = "04a6dd80-24d6-4921-8b8d-666bd55f3829";
            Microsoft.Phone.Maps.MapsSettings.ApplicationContext.AuthenticationToken = "WS2ppSWOojNt0b8TJMo10g";
            
            
        }

        private async void HaritayiGuncelle()
        {
            map1.ZoomLevel = yaklas;
            //Pushpin pin = new Pushpin();           
            

            
            Windows.Devices.Geolocation.Geolocator geolocator = new Windows.Devices.Geolocation.Geolocator();
            Windows.Devices.Geolocation.Geoposition geoposition = await geolocator.GetGeopositionAsync();
            MapLayer maplayer2 = new MapLayer();
            MapOverlay overlay2 = new MapOverlay();
            var imgs2 = new Image { Width = 56, Height = 56 };
            imgs2.Source = new BitmapImage { UriSource = new Uri("/image/pushpin2.png", UriKind.Relative) };
            overlay2.Content = imgs2;
            overlay2.GeoCoordinate = new GeoCoordinate(geoposition.Coordinate.Latitude, geoposition.Coordinate.Longitude);
            maplayer2.Add(overlay2);
            map1.Layers.Add(maplayer2);
            map1.Center = new GeoCoordinate(geoposition.Coordinate.Latitude, geoposition.Coordinate.Longitude);
            
        }

        private async void GetCurrentCoordinate()
        {
            Geolocator geolocator = new Geolocator();
            geolocator.DesiredAccuracy = PositionAccuracy.High;

            try
            {
                Geoposition currentPosition = await geolocator.GetGeopositionAsync(TimeSpan.FromMinutes(1), TimeSpan.FromSeconds(10));
                _accuracy = currentPosition.Coordinate.Accuracy;

                MyCoordinate = new GeoCoordinate(currentPosition.Coordinate.Latitude, currentPosition.Coordinate.Longitude);

                if (MyReverseGeocodeQuery == null || !MyReverseGeocodeQuery.IsBusy)
                {
                    MyReverseGeocodeQuery = new ReverseGeocodeQuery();
                    MyReverseGeocodeQuery.GeoCoordinate = new GeoCoordinate(MyCoordinate.Latitude, MyCoordinate.Longitude);
                    MyReverseGeocodeQuery.QueryCompleted += ReverseGeocodeQuery_QueryCompleted;
                    MyReverseGeocodeQuery.QueryAsync();
                }

            }
            catch (Exception ex)
            {

            }
        }

        private void ReverseGeocodeQuery_QueryCompleted(object sender, QueryCompletedEventArgs<IList<MapLocation>> y)
        {
            if (y.Error == null)
            {
                if (y.Result.Count > 0)
                {
                    MapAddress address = y.Result[0].Information.Address;
                    sehirTxt.Text = address.State.ToString();
                    dukkanlariGoster(markaid,address.State.ToString());

                }
            }
        }

        private void dukkanlariGoster(int markaid, string s)
        {
            WebClient cDukkan = new WebClient();
            cDukkan.OpenReadAsync(new Uri("http://logogo.azurewebsites.net/api/logogo/Getdukkan?markaid=" + markaid.ToString() + "&sehir=" + s));
            cDukkan.OpenReadCompleted += cDukkan_OpenReadCompleted;
        }

        void cDukkan_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {

                using (var reader2 = new StreamReader(e.Result))
                {

                    var download2 = reader2.ReadToEnd();
                    if (download2 != "[]")
                    {
                        
                        
                        
                        var gelen = JArray.Parse(download2);
                        for (int i = 0; i < gelen.Count; i++)
                        {
                            MapLayer maplayer1 = new MapLayer();
                            MapOverlay overlay1 = new MapOverlay();
                            var imgs1 = new Image { Width = 56, Height = 56 };
                            imgs1.Source = new BitmapImage { UriSource = new Uri("/image/pushpin.png", UriKind.Relative) };
                            overlay1.Content = imgs1;
                            overlay1.GeoCoordinate = new GeoCoordinate(double.Parse(gelen[i]["latitude"].ToString()), double.Parse(gelen[i]["longitude"].ToString()));
                            maplayer1.Add(overlay1);
                            map1.Layers.Add(maplayer1);
                        }
                            
                       

                    }
                    else
                    {
                        MessageBox.Show("Konumunuza ait kayıt bulunmamaktadır.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

       
        private  void logoGetir()
        {
            WebClient c = new WebClient();

            
            c.OpenReadAsync(new Uri("http://logogo.azurewebsites.net/api/logogo/Get"));
            
            c.OpenReadCompleted +=  c_OpenReadCompleted;          
         }

       

        void c_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {

            
            var rndx = new Random();
            while (Logodizi.Count != 12)
            {
                Logodizi.Add(rndx.Next(3, 18));
            }
           

            sayac++;
            if (sayac > 2)
            {
                logoIndisi = Logodizi.ElementAt(logodiziindisi);
                logodiziindisi++;
            }
            else
            {
                logoIndisi = sayac;
            }

            //logoIndisi++;
            try {
                    using (var reader = new StreamReader(e.Result))
                    {
                        var download = reader.ReadToEnd();
                        var o = JArray.Parse(download);
                        aciklamaTxt.Text = o[logoIndisi]["markaisim"].ToString() +" markasının logo rengini veya renklerini tahmin ediniz.";
                        BitmapImage bm = new BitmapImage(new Uri(o[logoIndisi]["resim2"].ToString(), UriKind.RelativeOrAbsolute));
                        imgLogo.Source = bm;
                        BitmapImage bmRenkli = new BitmapImage(new Uri(o[logoIndisi]["resim"].ToString(), UriKind.RelativeOrAbsolute));
                        renklilogoimg.Source = bmRenkli;
                        string color = "#" + o[logoIndisi]["renk1"];
                        markaid = Int32.Parse( o[logoIndisi]["markaid"].ToString());
                        timer.Start();
                        
                        var rnd = new Random();
                        int hangiRec = rnd.Next(1, 6);
                        renkliButonOlustur(color, hangiRec); //Renkli butonların oluşturulduğu fonksiyon
                        digerRenkliButonlar(hangiRec);

                        loading.Stop();
                        LoadingGrid.Visibility = Visibility.Collapsed;
                    }
            }
            catch { MessageBox.Show("Bağlantıda sorun oluştu."); }
            
        }

       

        private void digerRenkliButonlar(int hangiRec)
        {
            var Renkdizi = new HashSet<int>();
            var rndx = new Random();
            while (Renkdizi.Count != 15)
            {
                Renkdizi.Add(rndx.Next(1, 255));
            }



            if (hangiRec == 1) { }
            else

                rec1.Fill = new SolidColorBrush(Color.FromArgb(255, Convert.ToByte(Renkdizi.ElementAt(0)), Convert.ToByte(Renkdizi.ElementAt(1)), Convert.ToByte(Renkdizi.ElementAt(2))));
            if (hangiRec == 2) { }
            else

                rec2.Fill = new SolidColorBrush(Color.FromArgb(255, Convert.ToByte(Renkdizi.ElementAt(3)), Convert.ToByte(Renkdizi.ElementAt(4)), Convert.ToByte(Renkdizi.ElementAt(5))));
            if (hangiRec == 3) { }
            else

                rec3.Fill = new SolidColorBrush(Color.FromArgb(255, Convert.ToByte(Renkdizi.ElementAt(6)), Convert.ToByte(Renkdizi.ElementAt(7)), Convert.ToByte(Renkdizi.ElementAt(8))));
            if (hangiRec == 4) { }
            else

                rec4.Fill = new SolidColorBrush(Color.FromArgb(255, Convert.ToByte(Renkdizi.ElementAt(9)), Convert.ToByte(Renkdizi.ElementAt(10)), Convert.ToByte(Renkdizi.ElementAt(11))));
            if (hangiRec == 5) { }
            else

                rec5.Fill = new SolidColorBrush(Color.FromArgb(255, Convert.ToByte(Renkdizi.ElementAt(12)), Convert.ToByte(Renkdizi.ElementAt(13)), Convert.ToByte(Renkdizi.ElementAt(14))));

        }

        private void renkliButonOlustur(string color, int i)
        {
            byte R = Convert.ToByte(color.Substring(1, 2), 16);
            byte G = Convert.ToByte(color.Substring(3, 2), 16);
            byte B = Convert.ToByte(color.Substring(5, 2), 16);
            SolidColorBrush scb = new SolidColorBrush(Color.FromArgb(255, R, G, B));
            btnRenk1.Tag = "0";
            btnRenk2.Tag = "0";
            btnRenk3.Tag = "0";
            btnRenk4.Tag = "0";
            btnRenk5.Tag = "0";



            switch (i)
            {
                case 1:
                    rec1.Fill = scb;
                    btnRenk1.Tag = "1"; break;
                case 2:
                    rec2.Fill = scb;
                    btnRenk2.Tag = "1"; break;
                case 3:
                    rec3.Fill = scb;
                    btnRenk3.Tag = "1"; break;
                case 4:
                    rec4.Fill = scb;
                    btnRenk4.Tag = "1"; break;
                case 5:
                    rec5.Fill = scb;
                    btnRenk5.Tag = "1"; break;

            }





        }

        private void reklamGetir()
        {
            WebClient cReklam = new WebClient();
            cReklam.OpenReadAsync(new Uri("http://logogo.azurewebsites.net/api/logogo/Getir?id=" + markaid.ToString()));
            cReklam.OpenReadCompleted += cReklam_OpenReadCompleted;

        }

        void cReklam_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {
                
                using (var reader2 = new StreamReader(e.Result))
                {
                    
                    var download2 = reader2.ReadToEnd();
                    if(download2!="[]"){
                        neredeButton.Visibility = Visibility.Visible;
                    var gelen = JArray.Parse(download2);
                    BitmapImage rm = new BitmapImage(new Uri(gelen[0]["reklamresim"].ToString(), UriKind.RelativeOrAbsolute));
                    firsatimg.Source = rm;

                    }
                    else{
                        neredeButton.Visibility = Visibility.Collapsed;
                       BitmapImage rm2 = new BitmapImage(new Uri("images/urunyok.png", UriKind.RelativeOrAbsolute));
                    firsatimg.Source = rm2;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void btnRenk1_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            if (btnRenk1.Tag.ToString() == "1")
            {
                bildiGrid.Visibility = Visibility.Visible;
                bildiAni.Begin();
                skorGuncelle();
				bildi++;
                reklamGetir();
                
            }
            else
            {
                can--;
                
                canKontrol();
            }
        }

        

       

        private void btnRenk2_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            if (btnRenk2.Tag.ToString() == "1")
            {
                bildiGrid.Visibility = Visibility.Visible;
                bildiAni.Begin();
                skorGuncelle();
				bildi++;
                reklamGetir();
				
                
            }
            else
            {
                can--;
                
                canKontrol();
            }
        }

        

        private void btnRenk3_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            if (btnRenk3.Tag.ToString() == "1")
            {
                bildiGrid.Visibility = Visibility.Visible;
                bildiAni.Begin();
                skorGuncelle();
				bildi++;
                reklamGetir();
				
                
            }
            else
            {
                can--;
                
				canKontrol();
            }
        }

        private void btnRenk4_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            if (btnRenk4.Tag.ToString() == "1")
            {
                bildiGrid.Visibility = Visibility.Visible;
                bildiAni.Begin();
                skorGuncelle();
				bildi++;
                reklamGetir();
                
            }
            else
            {
                can--;
               
                canKontrol();
            }
        }

        private void btnRenk5_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            if (btnRenk5.Tag.ToString() == "1")
            {
                bildiGrid.Visibility = Visibility.Visible;
                bildiAni.Begin();
                skorGuncelle();
				bildi++;
                reklamGetir();
               
            }
            else
            {
                can--;
                
                canKontrol();
            }
        }
        private void canKontrol()
        {
            
            if (can == 0)
            {
                gameover.Begin();
            }
            else
            {
                if (saniye == 0)
                {
                    suredolduTxt.Text = "Süreniz Doldu !";
                    bilemediGrid.Visibility = Visibility.Visible;
                    canTxt.Text = can.ToString();
                    bilemediAni.Begin();
                    saniye = 11;
                  
                }
                else
                {
                    suredolduTxt.Text = "Yanlış Renk !";
                    bilemediGrid.Visibility = Visibility.Visible;
                    canTxt.Text = can.ToString();
                    bilemediAni.Begin();
                }
            }

        }

        private void bildiControl()
        {
            if (bildi == 15)
            {
                NavigationService.Navigate(new Uri("/Bitti.xaml?puan=" + skorTxt.Text, UriKind.RelativeOrAbsolute));
            }
            else {
                saniye = 11;
                logoGetir(); }
        }

        private void devamButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	bildiGrid.Visibility = Visibility.Collapsed;
        	bildiControl();
        }

        private void yenidenbaslaBtn_Click(object sender, RoutedEventArgs e)
        {

            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void cikisBtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Terminate(); //oyundan çıkış
        }

        private void skorGuncelle()
        {
            skorTxt.Text = (skor + (saniye * 10)+ Int32.Parse(skorTxt.Text)).ToString();
        }

        private void devamButton1_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            bilemediGrid.Visibility = Visibility.Collapsed;
            timer.Start();
        }

        private void cikisButton2_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	Application.Current.Terminate(); //oyundan çıkış
        }

        private void geriBtn_Click(object sender, RoutedEventArgs e)
        {
            neredeGrid.Visibility = Visibility.Collapsed;
            
        }

        private void neredeButton_Click(object sender, RoutedEventArgs e)
        {
            map1.Layers.Clear();
            GetCurrentCoordinate();
            HaritayiGuncelle();

            neredeGrid.Visibility = Visibility.Visible;
            neredeAni.Begin();
            //haritaGoster(markaid);
            
            
        }

        

    

        

        
    }

    
}