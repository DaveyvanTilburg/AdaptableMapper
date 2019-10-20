using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Web.Controllers.XPath;
using XPathSerialization;

namespace Web.Controllers
{
    public class SerializeExampleController : ApiController
    {
        public HttpResponseMessage Get()
        {
            XPathConfigurationEntry example = XPathConfigurationEntry.Convert(GetFakedSerializationConfigurations());
            var request = new Request() { XPathConfigurationEntry = example };

            HttpResponseMessage response = Request.CreateResponse(System.Net.HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            return response;
        }

        private XPathConfiguration GetFakedSerializationConfigurations()
        {
            string searchPath = "GuestId";
            string xPath = "./RoomTypes/RoomType/RoomDescription/@GuestLastName";
            string objectPath = "../Guests{'Name':'GuestId','Value':'{{searchResult}}'}/Surname";

            XPathConfiguration roomDescriptionNameHotel = XPathConfiguration.CreateXPathSearch(xPath, objectPath, searchPath);
            XPathConfiguration roomStayCodeMap = XPathConfiguration.CreateXPathMap("./RoomTypes/RoomType/@RoomTypeCode", "Code");
            List<XPathConfiguration> roomStayConfiguration = new List<XPathConfiguration>() { roomStayCodeMap, roomDescriptionNameHotel };
            XPathConfiguration roomStayScope = XPathConfiguration.CreateXPathScope("./RoomStays/RoomStay", "RoomStays");
            roomStayScope.SetConfigurations(roomStayConfiguration);

            XPathConfiguration reservationHotelCodeMap = XPathConfiguration.CreateXPathMap(@"./ResGlobalInfo/BasicPropertyInfo/@HotelCode", "HotelCode");
            XPathConfiguration reservationIdMap = XPathConfiguration.CreateXPathMap(@"./ResGlobalInfo/HotelReservationIDs/HotelReservationID[@ResID_Type=""5""]/@ResID_Value", "Id");
            List<XPathConfiguration> reservationConfiguration = new List<XPathConfiguration>() { reservationIdMap, reservationHotelCodeMap, roomStayScope };
            XPathConfiguration reservationScope = XPathConfiguration.CreateXPathScope("//ReservationsList/HotelReservation", "Reservations");
            reservationScope.SetConfigurations(reservationConfiguration);

            return reservationScope;
        }
    }
}