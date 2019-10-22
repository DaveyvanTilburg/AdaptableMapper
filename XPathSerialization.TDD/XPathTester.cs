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
            XPathSerializer.Deserialize(GetFakedDeserializationConfigurations(), System.IO.File.ReadAllText(@".\Xmls\BOO_Reservation.xml"), result);

            result.Reservations.Count.Should().Be(2);
            result.Reservations[0].Id.Should().Be("03a804fa");
            result.Reservations[0].HotelCode.Should().Be("62818");
            result.Reservations[1].Id.Should().Be("03a804fb");
            result.Reservations[1].HotelCode.Should().Be("62818");

            result.Reservations[0].RoomStays.Count.Should().Be(2);
            result.Reservations[0].RoomStays[0].Code.Should().Be("6281801");
            result.Reservations[0].RoomStays[0].GuestName.Should().Be("S*****");
            result.Reservations[0].RoomStays[0].RateCode.Should().Be("65090");
            result.Reservations[0].RoomStays[0].GuestId.Should().Be("1");
            result.Reservations[0].RoomStays[1].Code.Should().Be("6281802");
            result.Reservations[0].RoomStays[1].GuestName.Should().Be("D****");
            result.Reservations[0].RoomStays[1].RateCode.Should().Be("65090");
            result.Reservations[0].RoomStays[1].GuestId.Should().Be("2");

            result.Reservations[0].Guests.Count.Should().Be(2);
            result.Reservations[0].Guests[0].GivenName.Should().Be("S*****");
            result.Reservations[0].Guests[0].Surname.Should().Be("K**************");
            result.Reservations[0].Guests[0].GuestId.Should().Be("1");
            result.Reservations[0].Guests[1].GivenName.Should().Be("D****");
            result.Reservations[0].Guests[1].Surname.Should().Be("T**************");
            result.Reservations[0].Guests[1].GuestId.Should().Be("2");
        }

        [Fact]
        public void Serialize()
        {
            var source = new Root();
            XPathSerializer.Deserialize(GetFakedDeserializationConfigurations(), System.IO.File.ReadAllText(@".\Xmls\BOO_Reservation.xml"), source);

            string template = System.IO.File.ReadAllText(@".\Xmls\SandboxTemplate.xml");
            string result = XPathSerializer.Serialize(GetFakedSerializationConfigurations(), template, source);

            string expectedResult = System.IO.File.ReadAllText(@".\Xmls\ExpectedSandboxResult.xml");
            XElement xResult = XElement.Parse(result);
            XElement xExpectedResult = XElement.Parse(expectedResult);

            xResult.Should().BeEquivalentTo(xExpectedResult);
        }

        private XPathConfiguration GetFakedSerializationConfigurations()
        {
            string searchPath = "GuestId";
            string xPath = "./RoomTypes/RoomType/RoomDescription/@GuestLastName";
            string adaptablePath = "../Guests{'Name':'GuestId','Value':'{{searchResult}}'}/Surname";

            XPathConfiguration roomDescriptionNameHotel = XPathConfigurationBase.CreateXPathSearch(xPath, adaptablePath, searchPath);
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

        private XPathConfiguration GetFakedDeserializationConfigurations()
        {
            string searchPath = "./ResGuestRPHs/ResGuestRPH/@RPH";
            string xPath = @"../../ResGuests/ResGuest[@ResGuestRPH=""{{searchResult}}""]/Profiles/ProfileInfo/Profile/Customer/PersonName/GivenName";
            string adaptablePath = "GuestName";

            XPathConfiguration roomStayGuestName = XPathConfigurationBase.CreateXPathSearch(xPath, adaptablePath, searchPath);
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