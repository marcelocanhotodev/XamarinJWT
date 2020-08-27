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
    public class SignUpViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region Actions
        public Action ReturnMessage;
        #endregion

        #region Atributos
        private string nome;
        private string senha;
        private string confirmarsenha;
        private string email;

        private User user;

        public string message { get; set; }

        public string Nome
        {
            get { return nome; }
            set
            {
                nome = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Nome"));
            }
        }

        public string Senha
        {
            get { return senha; }
            set
            {
                senha = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Senha"));
            }
        }

        public string ConfirmarSenha
        {
            get { return confirmarsenha; }
            set
            {
                confirmarsenha = value;
                PropertyChanged(this, new PropertyChangedEventArgs("ConfirmarSenha"));
            }
        }


        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Email"));
            }
        }
        #endregion

        #region Comandos 
        public ICommand SignUp { protected set; get; }
        #endregion

        public SignUpViewModel()
        {
            SignUp = new Command(async () => await Register());
        }

        async Task Register()
        {

            if (!senha.Equals(confirmarsenha))
            {
                message = "Senhas divergentes!";
                ReturnMessage();
                return;
            }

            user = new User
            {
                username = this.Nome,
                password = this.Email,
                email = this.Senha
            };

            var respostaHttp = await ClientHttp.PostAsync("users", user);

            if (respostaHttp.statuscode.Equals(HttpStatusCode.Created))
            {
                try
                {
                    message = "Usuário criado com sucesso!";
                    ReturnMessage();
                    App.Current.MainPage = new Home();
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                    ReturnMessage();
                    return;
                }
            }
            else
            {
                message = respostaHttp.message;
                ReturnMessage();
                return;
            }
        }

    }
}

