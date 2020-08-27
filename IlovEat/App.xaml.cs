using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using IlovEat.Pages.Login;
using IlovEat.Pages;
using Android.Content.Res;

namespace IlovEat
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new Welcome());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
