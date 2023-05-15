using ClientesWeb.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ClientesWeb.Services
{
    public class Services
    {
        private readonly IConfiguration configuration;

        public Services(IConfiguration iConfig)
        {
            configuration = iConfig;
        }

        public async Task<TokenModel> RetornaToken(string _login, string _senha)
        {
            Services _services = new Services(configuration);
            TokenModel _Token = new TokenModel();

            string urlbase = SettingsConfigHelper.AppSetting("api_endpoint_urlbase");
            string urltoken = SettingsConfigHelper.AppSetting("api_endpoint_token");
            string url = urlbase + urltoken;

            try
            {
                var httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(url);
                httpClient.Timeout = new TimeSpan(0, 2, 0);
                httpClient.DefaultRequestHeaders.Add("ContentType", "application/json");

                LoginAPI _loginapi = new LoginAPI();
                _loginapi.login = CleanInput(_login);
                _loginapi.senha = CleanInput(_senha);
                StringContent content = new StringContent(JsonConvert.SerializeObject(_loginapi), Encoding.UTF8, "application/json");

                HttpResponseMessage response = httpClient.PostAsync(url, content).Result;

                var jsonResponse = await response.Content.ReadAsStringAsync();

                TokenModel _rootToken = new TokenModel();

                if (jsonResponse != null)
                {
                    try
                    {
                        _Token = JsonConvert.DeserializeObject<TokenModel>(jsonResponse);
                        return _Token;
                    }
                    catch (Exception ex)
                    {
                        _Token.status = 0;
                        _Token.mensagem = ex.Message;
                        _Token.cliente = null;
                        _Token.token = null;
                    }
                }

                return _Token;
            }
            catch (Exception ex)
            {
                _Token.status = 0;
                _Token.mensagem = ex.Message;
                _Token.cliente = null;
                _Token.token = null;

                return _Token;
            }
        }

        public async Task<CepModel> RetornaCep(string _cep)
        {
            Services _services = new Services(configuration);
            CepModel _Cep = new CepModel();

            string urlbase = SettingsConfigHelper.AppSetting("api_endpoint_urlbase");
            string urlcep = SettingsConfigHelper.AppSetting("api_endpoint_get_cep");
            string url = urlbase + urlcep;

            try
            {
                var httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(url);
                httpClient.Timeout = new TimeSpan(0, 2, 0);
                httpClient.DefaultRequestHeaders.Add("ContentType", "application/json");

                StringContent content = new StringContent(JsonConvert.SerializeObject(_cep), Encoding.UTF8, "application/json");

                HttpResponseMessage response = httpClient.PostAsync(url, content).Result;

                var jsonResponse = await response.Content.ReadAsStringAsync();

                TokenModel _rootToken = new TokenModel();

                if (jsonResponse != null)
                {
                    try
                    {
                        _Cep = JsonConvert.DeserializeObject<CepModel>(jsonResponse);
                        return _Cep;
                    }
                    catch (Exception ex)
                    {
                        _Cep.Logradouro = "";
                        _Cep.Bairro = "";
                        _Cep.Cidade = "";
                        _Cep.Estado = "";
                    }
                }

                return _Cep;
            }
            catch (Exception ex)
            {
                _Cep.Logradouro = "";
                _Cep.Bairro = "";
                _Cep.Cidade = "";
                _Cep.Estado = "";

                return _Cep;
            }
        }

        public string CleanInput(string strIn)
        {
            try
            {
                return strIn.Replace(".", "").Replace("-", "").Replace("/", "");
            }
            catch (RegexMatchTimeoutException)
            {
                return String.Empty;
            }
        }

        public async Task<bool> CadastraCliente(ClienteModel cliente, string token)
        {
            string urlbase = configuration["Parametros:api_endpoint_urlbase"];
            string url = urlbase + configuration["Parametros:api_endpoint_clientes"];

            string jsonString = JsonConvert.SerializeObject(cliente);

            try
            {
                HttpMessageHandler handler = new HttpClientHandler()
                {
                };

                var httpClient = new HttpClient(handler)
                {
                    BaseAddress = new Uri(url),
                    Timeout = new TimeSpan(0, 2, 0)
                };

                httpClient.DefaultRequestHeaders.Add("ContentType", "application/json");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                StringContent content = new StringContent(JsonConvert.SerializeObject(cliente), Encoding.UTF8, "application/json");
                var response = httpClient.PostAsync(url, content).Result;

                var jsonResponse = await response.Content.ReadAsStringAsync();

                if (jsonResponse != null && jsonResponse != "[]")
                {
                    var _retornoapi = JsonConvert.DeserializeObject<ClienteModel>(jsonResponse);
                    ClienteModel _usuario = _retornoapi;

                    return true;

                }

                return true;
            }
            catch (Exception ex)
            {                
                return false;
            }
        }

    }
}
