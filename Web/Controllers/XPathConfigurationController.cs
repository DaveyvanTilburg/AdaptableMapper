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
            string searchPath = "./ResGuestRPHs/ResGuestRPH/@RPH";
            string placementPath = @"../../ResGuests/ResGuest[@ResGuestRPH=""{{searchResult}}""]/Profiles/ProfileInfo/Profile/Customer/PersonName/GivenName";
            string objectPath = "GuestName";

            XPathConfiguration roomStayGuestName = XPathConfiguration.CreateXPathSearch(placementPath, objectPath, searchPath);
            XPathConfiguration roomStayCodeMap = XPathConfiguration.CreateXPathMap("./RoomTypes/RoomType/@RoomTypeCode", "Code");
            XPathConfiguration roomStayRateCodeMap = XPathConfiguration.CreateXPathMap("./RoomRates/RoomRate/@RatePlanCode", "RateCode");
            List<XPathConfiguration> roomStayConfiguration = new List<XPathConfiguration>() { roomStayCodeMap, roomStayGuestName, roomStayRateCodeMap };
            XPathConfiguration roomStayScope = XPathConfiguration.CreateXPathScope("./RoomStays/RoomStay", "RoomStays");
            roomStayScope.SetConfigurations(roomStayConfiguration);

            XPathConfiguration guestGivenNameMap = XPathConfiguration.CreateXPathMap("./Profiles/ProfileInfo/Profile/Customer/PersonName/GivenName", "GivenName");
            XPathConfiguration guestSurNameMap = XPathConfiguration.CreateXPathMap("./Profiles/ProfileInfo/Profile/Customer/PersonName/Surname", "Surname");
            List<XPathConfiguration> guestConfiguration = new List<XPathConfiguration>() { guestGivenNameMap, guestSurNameMap };
            XPathConfiguration guestScope = XPathConfiguration.CreateXPathScope("./ResGuests/ResGuest", "Guests");
            guestScope.SetConfigurations(guestConfiguration);

            XPathConfiguration reservationHotelCodeMap = XPathConfiguration.CreateXPathMap(@"./RoomStays/RoomStay/BasicPropertyInfo/@HotelCode", "HotelCode");
            XPathConfiguration reservationIdMap = XPathConfiguration.CreateXPathMap(@"./ResGlobalInfo/HotelReservationIDs/HotelReservationID[@ResID_Type=""18""]/@ResID_Value", "Id");
            List<XPathConfiguration> reservationConfiguration = new List<XPathConfiguration>() { reservationIdMap, roomStayScope, guestScope, reservationHotelCodeMap };
            XPathConfiguration reservationScope = XPathConfiguration.CreateXPathScope("//HotelReservations/HotelReservation", "Reservations");

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
            XPathSerializer.Deserialize(configuration, input, target);

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