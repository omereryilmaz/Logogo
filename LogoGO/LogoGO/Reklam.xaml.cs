using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace LogoGO
{
    public partial class Reklam : PhoneApplicationPage
    {
        public Reklam()
        {
            InitializeComponent();
            logofullAni.Begin();
        }

        private void cikisfullBtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Terminate(); //oyundan çıkış
        }

        private void yenidenbaslaBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}