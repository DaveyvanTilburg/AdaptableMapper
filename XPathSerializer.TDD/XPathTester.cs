using FluentAssertions;
using System.Collections.Generic;
using System.Xml.Linq;
using XPathObjects;
using Xunit;

namespace XPathSerialization.TDD
{
    public class XPathTester
    {
        [Fact]
        public void Deserialize()
        {
            var result = new Root();
            XPathSerializer.Deserialize(GetFakedConfigurations(), System.IO.File.ReadAllText(@".\Xmls\BOO_Reservation.xml"), result);

            result.Reservations.Count.Should().Be(2);
            result.Reservations[0].Id.Should().Be("03a804fa");
            result.Reservations[0].HotelCode.Should().Be("62818");
            result.Reservations[1].Id.Should().Be("03a804fb");
            result.Reservations[1].HotelCode.Should().Be("62818");

            result.Reservations[0].RoomStays.Count.Should().Be(2);
            result.Reservations[0].RoomStays[0].Code.Should().Be("6281801");
            result.Reservations[0].RoomStays[0].GuestName.Should().Be("S*****");
            result.Reservations[0].RoomStays[0].RateCode.Should().Be("65090");
            result.Reservations[0].RoomStays[1].Code.Should().Be("6281802");
            result.Reservations[0].RoomStays[1].GuestName.Should().Be("D****");
            result.Reservations[0].RoomStays[1].RateCode.Should().Be("65090");

            result.Reservations[0].Guests.Count.Should().Be(2);
            result.Reservations[0].Guests[0].GivenName.Should().Be("S*****");
            result.Reservations[0].Guests[0].Surname.Should().Be("K**************");
            result.Reservations[0].Guests[1].GivenName.Should().Be("D****");
            result.Reservations[0].Guests[1].Surname.Should().Be("T**************");
        }

        [Fact]
        public void Serialize()
        {
            var source = new Root();
            XPathSerializer.Deserialize(GetFakedConfigurations(), System.IO.File.ReadAllText(@".\Xmls\BOO_Reservation.xml"), source);

            string searchPath = "";
            string xPath = "./RoomTypes/RoomType/RoomDescription/@Name";
            string objectPath = "../HotelCode";

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

            string template = System.IO.File.ReadAllText(@".\Xmls\SandboxTemplate.xml");
            string result = XPathSerializer.Serialize(reservationScope, template, source);

            string expectedResult = System.IO.File.ReadAllText(@".\Xmls\ExpectedSandboxResult.xml");
            XElement xResult = XElement.Parse(result);
            XElement xExpectedResult = XElement.Parse(expectedResult);

            xResult.Should().BeEquivalentTo(xExpectedResult);
        }

        private XPathConfiguration GetFakedConfigurations()
        {
            string searchPath = "./ResGuestRPHs/ResGuestRPH/@RPH";
            string xPath = @"../../ResGuests/ResGuest[@ResGuestRPH=""{{searchResult}}""]/Profiles/ProfileInfo/Profile/Customer/PersonName/GivenName";
            string objectPath = "GuestName";

            XPathConfiguration roomStayGuestName = XPathConfiguration.CreateXPathSearch(xPath, objectPath, searchPath);
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
            reservationScope.SetConfigurations(reservationConfiguration);

            return reservationScope;
        }
    }
}