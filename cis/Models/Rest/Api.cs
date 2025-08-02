using System.Net.Http.Headers;
using System.Net;
using System.Text;
using CISApps.Models.Api;
using System.Configuration;

namespace CISApps.Models.Rest
{
    public class Api : IApi
    {
        public Privilate? privilate { get; set; }
		public Emp? emp { get; set; }

		public Uri _base = new Uri("http://api:3000/");

        public Api(IConfiguration _config)
        {
            config = _config;
            _base = new Uri(_config.GetValue<string>("baseaddress"));
        }
        public Api() { 
        }
        public void setDomain(string domains)
        {
            _base = new Uri(domains);
        }

        IConfiguration config { get; set; }
        long IApi.pid_test { get => 0L; }

        public async Task<HttpResponseMessage> GET(string url)
        {
            HttpResponseMessage res;
            try
            {
                using (HttpClient client = getClient())
                {
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                    res = await client.GetAsync(url);
                }
            }
            catch (Exception ex)
            {
                res = new HttpResponseMessage();
                res.StatusCode = HttpStatusCode.InternalServerError;
                res.Content = new StringContent(ex.Message);
            }
            return res;
        }
        public async Task<HttpResponseMessage> DELETE(string url)
        {

            HttpResponseMessage res;
            try
            {
				using (HttpClient client = getClient())
				{
					res = await client.DeleteAsync(url);
				}
			}
            catch (Exception ex)
            {
                res = new HttpResponseMessage();
                res.StatusCode = HttpStatusCode.InternalServerError;
                res.Content = new StringContent(ex.Message);
            }
            return res;
        }

        public async Task<HttpResponseMessage> POST(string url, object data)
        {

            HttpResponseMessage res;
            try
            {
                StringContent content = new StringContent(data.ToJsonString(), Encoding.UTF8, "application/json");

				using (HttpClient client = getClient())
				{
					res = await client.PostAsync(url, content);
				}
            }
            catch (Exception ex)
            {
                res = new HttpResponseMessage();
                res.StatusCode = HttpStatusCode.InternalServerError;
                res.Content = new StringContent(ex.Message);
            }
            return res;
        }

        public async Task<HttpResponseMessage> PUT(string url, object data)
        {
            HttpResponseMessage res;
            try
            {
                string json = data.ToJsonString();
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

				using (HttpClient client = getClient())
				{
					res = await client.PutAsync(url, content);
				}
            }
            catch (Exception ex)
            {
                res = new HttpResponseMessage();
                res.StatusCode = HttpStatusCode.InternalServerError;
                res.Content = new StringContent(ex.Message);
            }
            return res;
        }

        public HttpClient getClient()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = _base;
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.ConnectionClose = true;
            return client;
        }

        public void setPrivilate(Privilate? p)
        {
            privilate = p;
        }

        public Privilate? GetPrivilate()
        {
            return privilate;
        }

        public void clearPrivilate()
        {
            privilate = null;
        }

        public void setUserLK(Emp? _emp)
        {
            emp = _emp;
        }
        public Emp? getUserLK()
		{
			return emp;
		}
		public void clearUserLK()
		{
			emp = null;
		}
    }
}
