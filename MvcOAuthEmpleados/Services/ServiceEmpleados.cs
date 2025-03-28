using MvcOAuthEmpleados.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace MvcOAuthEmpleados.Services
{
    public class ServiceEmpleados
    {
        private string UrlApi;
        private MediaTypeWithQualityHeaderValue Header;
        private IHttpContextAccessor httpContextAccessor;

        public ServiceEmpleados(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            this.UrlApi = configuration.GetValue<string>("ApiUrls:ApiEmpleados");
            this.Header = new MediaTypeWithQualityHeaderValue("application/json");
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> GetTokenAsync(string userName, string password)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "api/auth/login";
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                LoginModel model = new LoginModel
                {
                    UserName = userName,
                    Password = password
                };
                string jsonData = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                HttpResponseMessage responseMessage = await client.PostAsync(request, content);
                if (responseMessage.IsSuccessStatusCode)
                {
                    string data = await responseMessage.Content.ReadAsStringAsync();
                    JObject keys = JObject.Parse(data);
                    string token = keys.GetValue("response").ToString();
                    return token;
                }
                else
                {
                    return null;
                }
            }
        }

        private async Task<T> CallApiAsync<T>(string request)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                HttpResponseMessage responseMessage = await client.GetAsync(request);
                if (responseMessage.IsSuccessStatusCode)
                {
                    T data = await responseMessage.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    return default(T);
                }
            }
        }

        //VAMOS A REALIZAR UNA SOBRECARGA DEL METODO RECIBIENDO EL TOKEN
        private async Task<T> CallApiAsync<T>(string request, string token)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                client.DefaultRequestHeaders.Add("Authorization", "bearer " + token);
                HttpResponseMessage response = await client.GetAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    T data = await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    return default(T);
                }
            }
        }

        public async Task<List<Empleado>> GetEmpleadosAsync()
        {
            string request = "api/empleados";
            List<Empleado> empleados = await this.CallApiAsync<List<Empleado>>(request);
            return empleados;
        }

        //ALAMCENAREMOS EL TOKEN EN SESSION
        //POR AHORA RECIBIREMOS EL TOKEN EN EL METODO 
        public async Task<Empleado> FindEmpleadoAsync(int idEmpleado)
        {
            string token = this.httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == "TOKEN").Value;
            string request = "api/empleados/" + idEmpleado;
            Empleado empleado = await this.CallApiAsync<Empleado>(request, token);
            return empleado;
        }

        public async Task<Empleado> GetPerfilAsync()
        {
            string token = this.httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == "TOKEN").Value;
            string request = "api/empleados/perfil";
            Empleado empleado = await this.CallApiAsync<Empleado>(request, token);
            return empleado;
        }

        public async Task<List<Empleado>> GetCompisAsync()
        {
            string token = this.httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == "TOKEN").Value;
            string request = "api/empleados/compis";
            List<Empleado> compis = await this.CallApiAsync<List<Empleado>>(request, token);
            return compis;
        }
    }
}
