using IlovEat.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IlovEat.Pages.Login
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUp : ContentPage
    {
        public SignUp()
        {
            var contexto = new SignUpViewModel();
            this.BindingContext = contexto;
            contexto.ReturnMessage += () => DisplayAlert("Validação", $"{contexto.message}", "OK");
            InitializeComponent();
        }
    }
}