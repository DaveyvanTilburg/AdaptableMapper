using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
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
            XPathConfiguration configuration = Convert(request.XPathConfigurationEntry);

            var bytes = System.Convert.FromBase64String(request.XML);
            string input = ASCIIEncoding.ASCII.GetString(bytes);
            var target = new Root();
            XPathSerializer.Deserialize(configuration, input, target);

            HttpResponseMessage response = Request.CreateResponse(System.Net.HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(target), Encoding.UTF8, "application/json");
            return response;
        }

        private XPathConfiguration Convert(XPathConfigurationEntry step)
        {
            XPathConfiguration current = null;
            if (step.Type.Equals("Scope"))
                current = XPathConfiguration.CreateXPathScope(step.XPath, step.AdaptablePath);
            else if (step.Type.Equals("Map"))
                current = XPathConfiguration.CreateXPathMap(step.XPath, step.AdaptablePath);
            else if (step.Type.Equals("Search"))
                current = XPathConfiguration.CreateXPathSearch(step.XPath, step.AdaptablePath, step.SearchPath);

            var children = step.Configurations?
                .Select(c => Convert(c))
                .ToList();

            current.SetConfigurations(children);

            return current;
        }
    }
}