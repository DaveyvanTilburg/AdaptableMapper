using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using XPathObjects;
using XPathSerialization;

namespace Web.Controllers
{
    public class XPathConfigurationController : ApiController
    {
        public object XElement { get; private set; }

        public HttpResponseMessage Get()
        {
            XPathConfigurationEntry roomStayCodeMap = new XPathConfigurationEntry() { Type = "Map", XPath = "./RoomTypes/RoomType/@RoomTypeCode", ObjectPath = "Code" };
            var roomStayConfiguration = new List<XPathConfigurationEntry>() { roomStayCodeMap };
            XPathConfigurationEntry roomStayScope = new XPathConfigurationEntry() { Type = "Scope", XPath = "./RoomStays/RoomStay", ObjectPath = "RoomStays", Configurations = roomStayConfiguration };

            XPathConfigurationEntry guestGivenNameMap = new XPathConfigurationEntry() { Type = "Map", XPath = "./Profiles/ProfileInfo/Profile/Customer/PersonName/GivenName", ObjectPath = "GivenName" };
            XPathConfigurationEntry guestSurNameMap = new XPathConfigurationEntry() { Type = "Map", XPath = "./Profiles/ProfileInfo/Profile/Customer/PersonName/Surname", ObjectPath = "Surname" };
            var guestConfiguration = new List<XPathConfigurationEntry>() { guestGivenNameMap, guestSurNameMap };
            XPathConfigurationEntry guestScope = new XPathConfigurationEntry() { Type = "Scope", XPath = "./ResGuests/ResGuest", ObjectPath = "Guests", Configurations = guestConfiguration };

            XPathConfigurationEntry reservationIdMap = new XPathConfigurationEntry() { Type = "Map", XPath = @"./ResGlobalInfo/HotelReservationIDs/HotelReservationID[@ResID_Type=""18""]/@ResID_Value", ObjectPath = "Id" };
            var reservationConfiguration = new List<XPathConfigurationEntry>() { reservationIdMap, roomStayScope, guestScope };
            XPathConfigurationEntry reservationScope = new XPathConfigurationEntry() { Type = "Scope", XPath = "//HotelReservations/HotelReservation", ObjectPath = "Reservations", Configurations = reservationConfiguration };

            HttpResponseMessage response = Request.CreateResponse(System.Net.HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(reservationScope), Encoding.UTF8, "application/json");
            return response;
        }

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
            string input = System.Text.ASCIIEncoding.ASCII.GetString(bytes);
            var target = new Root();
            XPathSerializer.Adept(configuration, input, target);

            HttpResponseMessage response = Request.CreateResponse(System.Net.HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(target), Encoding.UTF8, "application/json");
            return response;
        }

        private XPathConfiguration Convert(XPathConfigurationEntry step)
        {
            XPathConfiguration current = null;
            if (step.Type.Equals("Scope"))
                current = XPathConfiguration.CreateXPathScope(step.XPath, step.ObjectPath);
            else if (step.Type.Equals("Map"))
                current = XPathConfiguration.CreateXPathMap(step.XPath, step.ObjectPath);

            var children = step.Configurations?
                .Select(c => Convert(c))
                .ToList();

            current.SetConfigurations(children);

            return current;
        }
    }
}