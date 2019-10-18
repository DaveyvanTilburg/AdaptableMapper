using FluentAssertions;
using System.Collections.Generic;
using XPathObjects;
using Xunit;

namespace XPathSerialization.TDD
{
    public class XPathTester
    {
        [Fact]
        public void Test()
        {
            var result = new Root();
            XPathSerializer.Adept(GetFakedConfigurations(), System.IO.File.ReadAllText(@".\Xmls\BOO_Reservation.xml"), result);

            result.Reservations.Count.Should().Be(2);
            result.Reservations[0].Id.Should().Be("03a804fa");
            result.Reservations[1].Id.Should().Be("03a804fb");

            result.Reservations[0].RoomStays.Count.Should().Be(2);
            result.Reservations[0].RoomStays[0].Code.Should().Be("6281801");
            result.Reservations[0].RoomStays[1].Code.Should().Be("6281802");

            result.Reservations[0].Guests.Count.Should().Be(1);
            result.Reservations[0].Guests[0].GivenName.Should().Be("S*****");
            result.Reservations[0].Guests[0].Surname.Should().Be("K**************");
        }

        public XPathConfiguration GetFakedConfigurations()
        {
            XPathConfiguration roomStayCodeMap = XPathConfiguration.CreateXPathMap("./RoomTypes/RoomType/@RoomTypeCode", "Code");
            List<XPathConfiguration> roomStayConfiguration = new List<XPathConfiguration>() { roomStayCodeMap };
            XPathConfiguration roomStayScope = XPathConfiguration.CreateXPathScope("./RoomStays/RoomStay", "RoomStays");
            roomStayScope.SetConfigurations(roomStayConfiguration);

            XPathConfiguration guestGivenNameMap = XPathConfiguration.CreateXPathMap("./Profiles/ProfileInfo/Profile/Customer/PersonName/GivenName", "GivenName");
            XPathConfiguration guestSurNameMap = XPathConfiguration.CreateXPathMap("./Profiles/ProfileInfo/Profile/Customer/PersonName/Surname", "Surname");
            List<XPathConfiguration> guestConfiguration = new List<XPathConfiguration>() { guestGivenNameMap, guestSurNameMap };
            XPathConfiguration guestScope = XPathConfiguration.CreateXPathScope("./ResGuests/ResGuest", "Guests");
            guestScope.SetConfigurations(guestConfiguration);

            XPathConfiguration reservationIdMap = XPathConfiguration.CreateXPathMap(@"./ResGlobalInfo/HotelReservationIDs/HotelReservationID[@ResID_Type=""18""]/@ResID_Value", "Id");
            List<XPathConfiguration> reservationConfiguration = new List<XPathConfiguration>() { reservationIdMap, roomStayScope, guestScope };
            XPathConfiguration reservationScope = XPathConfiguration.CreateXPathScope("//HotelReservations/HotelReservation", "Reservations");
            reservationScope.SetConfigurations(reservationConfiguration);

            return reservationScope;
        }
    }
}