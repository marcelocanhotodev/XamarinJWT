using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using IlovEat.ViewModel;
using Xamarin.Essentials;

namespace IlovEat.Pages.Login
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        public Login()
        {
            var contexto = new LoginViewModel();
            this.BindingContext = contexto;
            contexto.ReturnMessage += () => DisplayAlert("Retorno", $"{contexto.Response}","OK");
            InitializeComponent();
        }
    }
}