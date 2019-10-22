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

            XPathConfiguration roomDescriptionNameHotel = XPathConfigurationBase.CreateXPathSearch(xPath, objectPath, searchPath);
            XPathConfiguration roomStayCodeMap = XPathConfigurationBase.CreateXPathMap("./RoomTypes/RoomType/@RoomTypeCode", "Code");
            List<XPathConfiguration> roomStayConfiguration = new List<XPathConfiguration>() { roomStayCodeMap, roomDescriptionNameHotel };
            XPathConfiguration roomStayScope = XPathConfigurationBase.CreateXPathScope("./RoomStays/RoomStay", "RoomStays");
            roomStayScope.SetConfigurations(roomStayConfiguration);

            XPathConfiguration reservationHotelCodeMap = XPathConfigurationBase.CreateXPathMap(@"./ResGlobalInfo/BasicPropertyInfo/@HotelCode", "HotelCode");
            XPathConfiguration reservationIdMap = XPathConfigurationBase.CreateXPathMap(@"./ResGlobalInfo/HotelReservationIDs/HotelReservationID[@ResID_Type=""5""]/@ResID_Value", "Id");
            List<XPathConfiguration> reservationConfiguration = new List<XPathConfiguration>() { reservationIdMap, reservationHotelCodeMap, roomStayScope };
            XPathConfiguration reservationScope = XPathConfigurationBase.CreateXPathScope("//ReservationsList/HotelReservation", "Reservations");
            reservationScope.SetConfigurations(reservationConfiguration);

            return reservationScope;
        }
    }
}