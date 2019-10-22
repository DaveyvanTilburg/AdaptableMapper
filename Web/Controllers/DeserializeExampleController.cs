using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Web.Controllers.XPath;
using XPathSerialization;

namespace Web.Controllers
{
    public class DeserializeExampleController : ApiController
    {
        public HttpResponseMessage Get()
        {
            XPathConfigurationEntry example = XPathConfigurationEntry.Convert(GetFakedDeserializationConfiguration());
            var request = new Request() { XPathConfigurationEntry = example };

            HttpResponseMessage response = Request.CreateResponse(System.Net.HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            return response;
        }

        private XPathConfiguration GetFakedDeserializationConfiguration()
        {
            string searchPath = "./ResGuestRPHs/ResGuestRPH/@RPH";
            string xPath = @"../../ResGuests/ResGuest[@ResGuestRPH=""{{searchResult}}""]/Profiles/ProfileInfo/Profile/Customer/PersonName/GivenName";
            string objectPath = "GuestName";

            XPathConfiguration roomStayGuestName = XPathConfigurationBase.CreateXPathSearch(xPath, objectPath, searchPath);
            XPathConfiguration roomStayGuestId = XPathConfigurationBase.CreateXPathMap("./ResGuestRPHs/ResGuestRPH/@RPH", "GuestId");
            XPathConfiguration roomStayCodeMap = XPathConfigurationBase.CreateXPathMap("./RoomTypes/RoomType/@RoomTypeCode", "Code");
            XPathConfiguration roomStayRateCodeMap = XPathConfigurationBase.CreateXPathMap("./RoomRates/RoomRate/@RatePlanCode", "RateCode");
            List<XPathConfiguration> roomStayConfiguration = new List<XPathConfiguration>() { roomStayCodeMap, roomStayGuestName, roomStayRateCodeMap, roomStayGuestId };
            XPathConfiguration roomStayScope = XPathConfigurationBase.CreateXPathScope("./RoomStays/RoomStay", "RoomStays");
            roomStayScope.SetConfigurations(roomStayConfiguration);

            XPathConfiguration guestIdMap = XPathConfigurationBase.CreateXPathMap("./@ResGuestRPH", "GuestId");
            XPathConfiguration guestGivenNameMap = XPathConfigurationBase.CreateXPathMap("./Profiles/ProfileInfo/Profile/Customer/PersonName/GivenName", "GivenName");
            XPathConfiguration guestSurNameMap = XPathConfigurationBase.CreateXPathMap("./Profiles/ProfileInfo/Profile/Customer/PersonName/Surname", "Surname");
            List<XPathConfiguration> guestConfiguration = new List<XPathConfiguration>() { guestGivenNameMap, guestSurNameMap, guestIdMap };
            XPathConfiguration guestScope = XPathConfigurationBase.CreateXPathScope("./ResGuests/ResGuest", "Guests");
            guestScope.SetConfigurations(guestConfiguration);

            XPathConfiguration reservationHotelCodeMap = XPathConfigurationBase.CreateXPathMap(@"./RoomStays/RoomStay/BasicPropertyInfo/@HotelCode", "HotelCode");
            XPathConfiguration reservationIdMap = XPathConfigurationBase.CreateXPathMap(@"./ResGlobalInfo/HotelReservationIDs/HotelReservationID[@ResID_Type=""18""]/@ResID_Value", "Id");
            List<XPathConfiguration> reservationConfiguration = new List<XPathConfiguration>() { reservationIdMap, roomStayScope, guestScope, reservationHotelCodeMap };
            XPathConfiguration reservationScope = XPathConfigurationBase.CreateXPathScope("//HotelReservations/HotelReservation", "Reservations");
            reservationScope.SetConfigurations(reservationConfiguration);

            return reservationScope;
        }
    }
}