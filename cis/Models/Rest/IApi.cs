using CISApps.Models.Api;

namespace CISApps.Models.Rest
{
    public interface IApi
    {
        public long pid_test {  get; }
        public Task<HttpResponseMessage> GET(string url);

        public Task<HttpResponseMessage> POST(string url, object data);

        public Task<HttpResponseMessage> PUT(string url, object data);

        public Task<HttpResponseMessage> DELETE(string url);

        public HttpClient getClient();


        public void setPrivilate(Privilate? p);

        public Privilate? GetPrivilate();

        public void clearPrivilate();

        public void setUserLK(Emp? emp);

        public Emp? getUserLK();

        public void clearUserLK();
        public void setDomain(string domains);
    }
}
