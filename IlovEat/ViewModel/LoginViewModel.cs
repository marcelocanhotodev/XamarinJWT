using IlovEat.Clients;
using IlovEat.Models;
using IlovEat.Pages.Home;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace IlovEat.ViewModel
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Action ReturnMessage;

        #region Atributos
        private string email;
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Email"));
            }

        }

        private string response;
        public string Response
        {
            get { return response; }
            set
            {
                response = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Response"));
            }

        }

        private string senha;
        public string Senha
        {
            get { return senha; }
            set
            {
                senha = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Senha"));
            }

        }

        public ICommand SubmitCommand { protected set; get; }

        #endregion

        public LoginViewModel()
        {
            SubmitCommand = new Command(async () => await OnSubmit());
        }


        async Task OnSubmit()
        {
            Login login = new Login { email = this.Email, senha = this.Senha };

            var respostaHttp = await ClientHttp.PostAsync("sessions", login);

            if (respostaHttp.statuscode.Equals(HttpStatusCode.Created))
            {
                try
                {
                    var loginResponse = ClientHttp.ObterObjeto<LoginResponse>(respostaHttp.content);
                    ClientHttp.AplicarToken(loginResponse.token);
                    App.Current.MainPage = new Home();
                }
                catch (Exception)
                {

                    throw;
                }
            }
            else
            {
                this.response = respostaHttp.message;
                ReturnMessage();
            }
        }
    }
}
