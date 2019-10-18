using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using XPathObjects;

namespace Web.Controllers
{
    public class TemplateController : ApiController
    {
        public HttpResponseMessage Get()
        {
            var roomStay = new RoomStay();
            var guest = new Guest();
            var reservation = new Reservation()
            {
                RoomStays = new List<RoomStay>() { roomStay },
                Guests = new List<Guest>() { guest }
            };
            var result = new Root()
            {
                Reservations = new List<Reservation>() { reservation }
            };

            HttpResponseMessage response = Request.CreateResponse(System.Net.HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(result), Encoding.UTF8, "application/json");
            return response;
        }
    }
}