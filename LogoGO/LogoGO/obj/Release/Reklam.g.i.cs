﻿#pragma checksum "D:\LogoGO\LogoGOtfs\LogoGO\LogoGO\Reklam.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "7657C40F69741D028A8BDA3ACF4F1995"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace LogoGO {
    
    
    public partial class Reklam : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Media.Animation.Storyboard logofullAni;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.Image image;
        
        internal System.Windows.Controls.Button cikisfullBtn;
        
        internal System.Windows.Controls.Button yenidenbaslaBtn;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/LogoGO;component/Reklam.xaml", System.UriKind.Relative));
            this.logofullAni = ((System.Windows.Media.Animation.Storyboard)(this.FindName("logofullAni")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.image = ((System.Windows.Controls.Image)(this.FindName("image")));
            this.cikisfullBtn = ((System.Windows.Controls.Button)(this.FindName("cikisfullBtn")));
            this.yenidenbaslaBtn = ((System.Windows.Controls.Button)(this.FindName("yenidenbaslaBtn")));
        }
    }
}

