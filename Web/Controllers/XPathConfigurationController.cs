using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Web.Controllers.XPath;
using XPathObjects;
using XPathSerialization;

namespace Web.Controllers
{
    public class XPathConfigurationController : ApiController
    {
        public HttpResponseMessage Post([FromBody]string value)
        {
            var requestBody = value;
            if (string.IsNullOrWhiteSpace(requestBody))
            {
                Request.Content.ReadAsStreamAsync().Result.Seek(0, System.IO.SeekOrigin.Begin);
                requestBody = Request.Content.ReadAsStringAsync().Result;
            }

            Request request = JsonConvert.DeserializeObject<Request>(requestBody);
            XPathConfiguration configuration = XPathConfigurationEntry.Convert(request.XPathConfigurationEntry);

            var bytes = System.Convert.FromBase64String(request.Xml);
            string input = Encoding.ASCII.GetString(bytes);
            var target = new Root();
            XPathSerializer.Deserialize(configuration, input, target);

            HttpResponseMessage response = Request.CreateResponse(System.Net.HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(target), Encoding.UTF8, "application/json");
            return response;
        }
    }
}