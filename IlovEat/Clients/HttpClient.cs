using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using System.Net.Http.Headers;
using IlovEat.Providers;
using Polly;
using System.Net;

namespace IlovEat.Clients
{
    public sealed class ClientHttp
    {

        private static readonly Lazy<ClientHttp> _lazy = new Lazy<ClientHttp>(true);
        static ClientHttp current { get { return _lazy.Value; } }
        HttpClient _client = null;

        public ClientHttp()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Aplica token no client HTTP
        /// </summary>
        public static void AplicarToken(string token)
        {
            if (!string.IsNullOrEmpty(token))
                current._client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Replace("Bearer ", ""));
        }

        /// <summary>
        /// Obtém o token atual do client
        /// </summary>
        public static string ObterToken()
            => current._client.DefaultRequestHeaders.Authorization.Parameter;

        /// <summary>
        /// Remove o token atual se houver
        /// </summary>
        public static void LimparToken()
        {
            if (current._client.DefaultRequestHeaders.Authorization != null)
                current._client.DefaultRequestHeaders.Authorization = null;
        }

        /// <summary>
        /// Executa método GET na rota especificada
        /// </summary>
        /// <param name="caminho">ação da api a ser executada</param>
        /// <returns>True sucesso False erro</returns>
        public static async Task<RespostaHttp> GetAsync(string caminho, int retries = 0)
            => await SendAsync(HttpMethod.Get, caminho, retries);

        /// <summary>
        /// Executa método POST na rota especificada
        /// </summary>
        /// <param name="caminho">ação da api a ser executada</param>
        /// <param name="content">objeto a ser submetido</param>
        /// <returns>True sucesso False erro</returns>
        public static async Task<RespostaHttp> PostAsync(string caminho, object content, int retries = 0)
            => await SendAsync(HttpMethod.Post, caminho, retries, content);

        /// <summary>
        /// Executa um método HTTP na rota especificada
        /// </summary>
        /// <param name="verb">verbo HTTP a ser executado</param>
        /// <param name="caminho">ação da api a ser executada</param>
        /// <param name="content">(Opcional)objeto a ser submetido</param>
        /// <param name="permitirLogoff">Se a comunicação resultar em unauthorized ou forbidden, efetua logoff automático</param>
        /// <returns>True sucesso False erro</returns>
        public static async Task<RespostaHttp> SendAsync(HttpMethod verb, string caminho, int retries = 0, object content = null, bool permitirLogoff = true)
        {
          //  if (Connectivity.NetworkAccess == NetworkAccess.None)
          //      return new RespostaHttp(mensagem: "Sem conexão");

            var requestUri = $"{ParametersProvider.URL_API}/{caminho}";

            using (var request = new HttpRequestMessage(verb, requestUri))
            {
                if (content != null)
                    request.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");

                using (var response = await Policy
                .Handle<HttpRequestException>()
                .WaitAndRetry(retryCount: retries, sleepDurationProvider: (attempt) => TimeSpan.FromSeconds(2))
                .Execute(async () => await current._client.SendAsync(request)))
                    return await current.CriarRespostaHttp(response);
            }
        }

        public static TTipo ObterObjeto<TTipo>(string content)
        {
            return JsonConvert.DeserializeObject<TTipo>(content);
        }


        /// <summary>
        /// Lê mensagem de retorno da chamada Http e desserializa para obter descrição do erro
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        async Task<RespostaHttp> CriarRespostaHttp(HttpResponseMessage response)
        {
            var retorno = ObterObjeto<RespostaHttp>(await response.Content.ReadAsStringAsync());
            retorno.statuscode = response.StatusCode;
            return retorno;
        }

        public class RespostaHttp
        {

            public string content { get; set; }
            public string message { get; set; }
            public HttpStatusCode statuscode { get; set; }

        }
    }
}


