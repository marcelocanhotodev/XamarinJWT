using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IlovEat.Pages.Login;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IlovEat.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Welcome : ContentPage
    {
        public Welcome()
        {
            InitializeComponent();
        }

        private async void BtnCadastrar_Clicked(object sender, EventArgs e)
        {
            this.IsEnabled = false;
            await Navigation.PushAsync(new Login.SignUp());
            this.IsEnabled = true;
        }
                
        private void BtnAcessar_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Login.Login());
        }
    }
}