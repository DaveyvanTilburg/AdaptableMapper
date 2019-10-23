using FluentAssertions;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Xml.Linq;
using XPathObjects;
using Xunit;

namespace XPathSerialization.TDD
{
    public class XPathTester
    {
        [Fact]
        public void Serialize()
        {
            var result = new Root();
            XPathSerializer.Serialize(GetFakedSerializationConfigurations(), System.IO.File.ReadAllText(@".\Xmls\BOO_Reservation.xml"), result);

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
        public void Deserialize()
        {
            var source = new Root();
            XPathSerializer.Serialize(GetFakedSerializationConfigurations(), System.IO.File.ReadAllText(@".\Xmls\BOO_Reservation.xml"), source);

            string template = System.IO.File.ReadAllText(@".\Xmls\SandboxTemplate.xml");
            string result = XPathSerializer.Deserialize(GetFakedDeserializationConfiguration(), template, source);

            string expectedResult = System.IO.File.ReadAllText(@".\Xmls\ExpectedSandboxResult.xml");
            XElement xResult = XElement.Parse(result);
            XElement xExpectedResult = XElement.Parse(expectedResult);

            xResult.Should().BeEquivalentTo(xExpectedResult);
        }

        [Fact]
        public void SaveAndLoadThroughJsonSerialization()
        {
            XPathConfiguration source = GetFakedDeserializationConfiguration();

            string serialized = JsonConvert.SerializeObject(source);
            XPathConfiguration target = JsonConvert.DeserializeObject<XPathConfiguration>(serialized);

            source.Should().BeEquivalentTo(target);
        }

        private XPathConfiguration GetFakedDeserializationConfiguration()
        {
            string searchPath = "GuestId";
            string xPath = "./RoomTypes/RoomType/RoomDescription/@GuestLastName";
            string adaptablePath = "../Guests{'Name':'GuestId','Value':'{{searchResult}}'}/Surname";

            var roomGuestLastNameSearch = XPathConfiguration.CreateSearchConfiguration(xPath, adaptablePath, searchPath);
            var roomStayCodeMap = XPathConfiguration.CreateMapConfiguration("./RoomTypes/RoomType/@RoomTypeCode", "Code");
            var roomStayConfiguration = new List<XPathConfiguration>() { roomStayCodeMap, roomGuestLastNameSearch };
            var roomStayScope = XPathConfiguration.CreateScopeConfiguration("./RoomStays/RoomStay", "RoomStays", roomStayConfiguration);

            var reservationHotelCodeMap = XPathConfiguration.CreateMapConfiguration(@"./ResGlobalInfo/BasicPropertyInfo/@HotelCode", "HotelCode");
            var reservationIdMap = XPathConfiguration.CreateMapConfiguration(@"./ResGlobalInfo/HotelReservationIDs/HotelReservationID[@ResID_Type='5']/@ResID_Value", "Id");
            var reservationConfiguration = new List<XPathConfiguration>() { reservationIdMap, reservationHotelCodeMap, roomStayScope };
            var reservationScope = XPathConfiguration.CreateScopeConfiguration("//ReservationsList/HotelReservation", "Reservations", reservationConfiguration);

            return reservationScope;
        }

        private XPathConfiguration GetFakedSerializationConfigurations()
        {
            string searchPath = "./ResGuestRPHs/ResGuestRPH/@RPH";
            string xPath = @"../../ResGuests/ResGuest[@ResGuestRPH=""{{searchResult}}""]/Profiles/ProfileInfo/Profile/Customer/PersonName/GivenName";
            string adaptablePath = "GuestName";

            var roomStayGuestNameSearch = XPathConfiguration.CreateSearchConfiguration(xPath, adaptablePath, searchPath);
            var roomStayGuestId = XPathConfiguration.CreateMapConfiguration("./ResGuestRPHs/ResGuestRPH/@RPH", "GuestId");
            var roomStayCodeMap = XPathConfiguration.CreateMapConfiguration("./RoomTypes/RoomType/@RoomTypeCode", "Code");
            var roomStayRateCodeMap = XPathConfiguration.CreateMapConfiguration("./RoomRates/RoomRate/@RatePlanCode", "RateCode");
            var roomStayConfiguration = new List<XPathConfiguration>() { roomStayCodeMap, roomStayGuestNameSearch, roomStayRateCodeMap, roomStayGuestId };
            var roomStayScope = XPathConfiguration.CreateScopeConfiguration("./RoomStays/RoomStay", "RoomStays", roomStayConfiguration);

            var guestIdMap = XPathConfiguration.CreateMapConfiguration("./@ResGuestRPH", "GuestId");
            var guestGivenNameMap = XPathConfiguration.CreateMapConfiguration("./Profiles/ProfileInfo/Profile/Customer/PersonName/GivenName", "GivenName");
            var guestSurNameMap = XPathConfiguration.CreateMapConfiguration("./Profiles/ProfileInfo/Profile/Customer/PersonName/Surname", "Surname");
            var guestConfiguration = new List<XPathConfiguration>() { guestGivenNameMap, guestSurNameMap, guestIdMap };
            var guestScope = XPathConfiguration.CreateScopeConfiguration("./ResGuests/ResGuest", "Guests", guestConfiguration);

            var reservationHotelCodeMap = XPathConfiguration.CreateMapConfiguration(@"./RoomStays/RoomStay/BasicPropertyInfo/@HotelCode", "HotelCode");
            var reservationIdMap = XPathConfiguration.CreateMapConfiguration(@"./ResGlobalInfo/HotelReservationIDs/HotelReservationID[@ResID_Type='18']/@ResID_Value", "Id");
            var reservationConfiguration = new List<XPathConfiguration>() { reservationIdMap, roomStayScope, guestScope, reservationHotelCodeMap };
            var reservationScope = XPathConfiguration.CreateScopeConfiguration("//HotelReservations/HotelReservation", "Reservations", reservationConfiguration);

            return reservationScope;
        }
    }
}