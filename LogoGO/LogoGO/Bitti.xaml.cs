using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;
using Facebook;
using Microsoft.Phone.Tasks;


namespace LogoGO
{
    public partial class Bitti : PhoneApplicationPage
    {
        private const string AppID = "298681596987589";
        private int puanim;
        private FacebookClient client;
        public Bitti()
        {
            InitializeComponent();
            client = new FacebookClient();
            client.PostCompleted += (o, args) =>
            {
                //Hata Kontrolünün yapıldığı bölüm
                if (args.Error != null)
                {
                    Dispatcher.BeginInvoke(() => MessageBox.Show(args.Error.Message));
                }
                else
                {
                    Dispatcher.BeginInvoke(() => MessageBox.Show("Mesaj başarılı bir şekilde gönderildi"));
                }
            };
            puanGetir();
        }

        private IsolatedStorageSettings Kullanici;
        private void puanGetir()
        {

            Kullanici = IsolatedStorageSettings.ApplicationSettings;
            if (Kullanici.Contains("puan"))
            {
                
                rekorTxt.Text = Kullanici["puan"].ToString();
            }

           
        }
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string puan = "";
            if (NavigationContext.QueryString.TryGetValue("puan", out puan))
                puanTxt.Text = puan;

            puanim = Int32.Parse( puan);
            int yeniSkor=Int32.Parse(puan);
            if (rekorTxt.Text == "...") //daha önce oynanmamışsa
            {
                IsolatedStorageSettings.ApplicationSettings["puan"] = yeniSkor; //puan isimli IsolatedStroge a yeniSkor u atıyoruz
                IsolatedStorageSettings.ApplicationSettings.Save();
                rekorTxt.Text = yeniSkor.ToString();
            }
            else
            { //daha önceden skor kaydımız varsa
                if (yeniSkor > Convert.ToInt32(rekorTxt.Text)) //ve skor kaydımız yeniSkor dan düşükse
                {

                    IsolatedStorageSettings.ApplicationSettings["puan"] = yeniSkor;
                    IsolatedStorageSettings.ApplicationSettings.Save();
                    rekorTxt.Text = yeniSkor.ToString();
                }
            }

        }

        private void sonrakiseviyeBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Reklam.xaml", UriKind.RelativeOrAbsolute));
        }

        private void faceBtn_Click(object sender, RoutedEventArgs e)
        {
            //webGrid.Visibility = Visibility.Visible;
            //webgridAni.Begin();
            ////Client Parameters

            //var parameters = new Dictionary<string, object>();
            //parameters["client_id"] = AppID;

            //parameters["redirect_uri"] = "https://www.facebook.com/connect/login_success.html";
            //parameters["response_type"] = "token";
            //parameters["display"] = "touch";
            ////The scope is what give us the access to the users data, in this case
            ////we just want to publish on his wall
            //parameters["scope"] = "publish_stream";
            //Uri LoginURL = client.GetLoginUrl(parameters);
            //webBrowser.Navigate(LoginURL);

            ShareStatusTask shareStatusTask = new ShareStatusTask();

            shareStatusTask.Status = "LogoGO oyunundaki yeni puanım : " + puanim.ToString() + "\n Oyunu indirmek için ;\n http://www.omereryilmaz.com";

            shareStatusTask.Show();
            
            
        }

        private void webBrowser_Navigated(object sender, NavigationEventArgs e)
        {
            FacebookOAuthResult oauthResult;
            //URL'in erişim izni olduğunu kontrol ediyoruz
            if (!client.TryParseOAuthCallbackUrl(e.Uri, out oauthResult))
            {
                return;
            }
            //Checking that the user successfully accepted our app, otherwise just show the error
            if (oauthResult.IsSuccess)
            {
                client.AccessToken = oauthResult.AccessToken;

                webGrid.Visibility = Visibility.Collapsed;
                DuvaraGonder();
            }
            else
            {
                MessageBox.Show(oauthResult.ErrorDescription);
                webGrid.Visibility = Visibility.Collapsed;
            }
        }

        private void DuvaraGonder()
        {
            var parameters = new Dictionary<string, object>();
            parameters["message"] = "LogoGO oyunundaki yeni puanım : "+puanim.ToString();
            client.PostTaskAsync("me/feed/", parameters);
            
        }

        
               


        
    }
}