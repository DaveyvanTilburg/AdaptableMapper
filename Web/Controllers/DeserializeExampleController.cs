using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using XPathSerialization;

namespace Web.Controllers
{
    public class DeserializeExampleController : ApiController
    {
        public HttpResponseMessage Get()
        {
            //convert to entries

            HttpResponseMessage response = Request.CreateResponse(System.Net.HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(null), Encoding.UTF8, "application/json");
            return response;
        }

        private XPathConfiguration GetFakedDeserializationConfigurations()
        {
            string searchPath = "./ResGuestRPHs/ResGuestRPH/@RPH";
            string xPath = @"../../ResGuests/ResGuest[@ResGuestRPH=""{{searchResult}}""]/Profiles/ProfileInfo/Profile/Customer/PersonName/GivenName";
            string objectPath = "GuestName";

            XPathConfiguration roomStayGuestName = XPathConfiguration.CreateXPathSearch(xPath, objectPath, searchPath);
            XPathConfiguration roomStayGuestId = XPathConfiguration.CreateXPathMap("./ResGuestRPHs/ResGuestRPH/@RPH", "GuestId");
            XPathConfiguration roomStayCodeMap = XPathConfiguration.CreateXPathMap("./RoomTypes/RoomType/@RoomTypeCode", "Code");
            XPathConfiguration roomStayRateCodeMap = XPathConfiguration.CreateXPathMap("./RoomRates/RoomRate/@RatePlanCode", "RateCode");
            List<XPathConfiguration> roomStayConfiguration = new List<XPathConfiguration>() { roomStayCodeMap, roomStayGuestName, roomStayRateCodeMap, roomStayGuestId };
            XPathConfiguration roomStayScope = XPathConfiguration.CreateXPathScope("./RoomStays/RoomStay", "RoomStays");
            roomStayScope.SetConfigurations(roomStayConfiguration);

            XPathConfiguration guestIdMap = XPathConfiguration.CreateXPathMap("./@ResGuestRPH", "GuestId");
            XPathConfiguration guestGivenNameMap = XPathConfiguration.CreateXPathMap("./Profiles/ProfileInfo/Profile/Customer/PersonName/GivenName", "GivenName");
            XPathConfiguration guestSurNameMap = XPathConfiguration.CreateXPathMap("./Profiles/ProfileInfo/Profile/Customer/PersonName/Surname", "Surname");
            List<XPathConfiguration> guestConfiguration = new List<XPathConfiguration>() { guestGivenNameMap, guestSurNameMap, guestIdMap };
            XPathConfiguration guestScope = XPathConfiguration.CreateXPathScope("./ResGuests/ResGuest", "Guests");
            guestScope.SetConfigurations(guestConfiguration);

            XPathConfiguration reservationHotelCodeMap = XPathConfiguration.CreateXPathMap(@"./RoomStays/RoomStay/BasicPropertyInfo/@HotelCode", "HotelCode");
            XPathConfiguration reservationIdMap = XPathConfiguration.CreateXPathMap(@"./ResGlobalInfo/HotelReservationIDs/HotelReservationID[@ResID_Type=""18""]/@ResID_Value", "Id");
            List<XPathConfiguration> reservationConfiguration = new List<XPathConfiguration>() { reservationIdMap, roomStayScope, guestScope, reservationHotelCodeMap };
            XPathConfiguration reservationScope = XPathConfiguration.CreateXPathScope("//HotelReservations/HotelReservation", "Reservations");
            reservationScope.SetConfigurations(reservationConfiguration);

            return reservationScope;
        }
    }
}