using AdaptableMapper.Traversals;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace AdaptableMapper.TDD
{
    public class XmlToMemory
    {
        [Fact]
        public void XmlToMemoryTest()
        {
            var errorObserver = new TestErrorObserver();
            Errors.ErrorObservable.GetInstance().Register(errorObserver);

            var configuration = GetFakedSerializationConfiguration();
            object resultObject = Mapper.Map(configuration, System.IO.File.ReadAllText(@".\Resources\BOO_Reservation.xml"));

            Root result = resultObject as Root;

            Errors.ErrorObservable.GetInstance().Unregister(errorObserver);

            errorObserver.GetErrors().Count.Should().Be(8);

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
            result.Reservations[0].RoomStays[0].Text.Should().BeEmpty();
            result.Reservations[0].RoomStays[0].Name.Should().BeEmpty();
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

        public static MappingConfiguration GetFakedSerializationConfiguration()
        {
            var roomStayGuestNameSearchMap = new Mapping(
                new Xml.XmlGetSearch(
                    "../../ResGuests/ResGuest[@ResGuestRPH='{{searchResult}}']/Profiles/ProfileInfo/Profile/Customer/PersonName/GivenName",
                    "./ResGuestRPHs/ResGuestRPH/@RPH"
                ),
                new Memory.AdaptableSetOnProperty("GuestName")
            );

            var roomStayTestObjectFail = new Mapping(
                new Xml.XmlGet("./RoomTypes/RoomType/RoomDescription/Text"),
                new Memory.AdaptableSetOnProperty("Test")
            );

            var roomStayNameXPathFail = new Mapping(
                new Xml.XmlGet("./RoomTypes/RoomType/RoomDescription/@Naem"),
                new Memory.AdaptableSetOnProperty("Name")
            );

            var roomStayGuestId = new Mapping(
                new Xml.XmlGet("./ResGuestRPHs/ResGuestRPH/@RPH"),
                new Memory.AdaptableSetOnProperty("GuestId")
            );

            var roomStayCodeMap = new Mapping(
                new Xml.XmlGet("./RoomTypes/RoomType/@RoomTypeCode"),
                new Memory.AdaptableSetOnProperty("Code")
            );

            var roomStayRateCodeMap = new Mapping(
                new Xml.XmlGet("./RoomRates/RoomRate/@RatePlanCode"),
                new Memory.AdaptableSetOnProperty("RateCode")
            );

            var roomStayScope = new MappingScopeComposite(
                new List<MappingScopeComposite>(),
                new List<Mapping>()
                {
                    roomStayGuestNameSearchMap,
                    roomStayTestObjectFail,
                    roomStayNameXPathFail,
                    roomStayGuestId,
                    roomStayCodeMap,
                    roomStayRateCodeMap
                },
                new Xml.XmlGetScope("./RoomStays/RoomStay"),
                new Memory.AdaptableTraversalThis(),
                new Memory.AdaptableTraversalTemplate("RoomStays"),
                new Memory.AdaptableChildCreator()
            );

            var guestIdMap = new Mapping(
                new Xml.XmlGet("./@ResGuestRPH"),
                new Memory.AdaptableSetOnProperty("GuestId")
            );

            var guestGivenNameMap = new Mapping(
                new Xml.XmlGet("./Profiles/ProfileInfo/Profile/Customer/PersonName/GivenName"),
                new Memory.AdaptableSetOnProperty("GivenName")
            );

            var guestSurNameMap = new Mapping(
                new Xml.XmlGet("./Profiles/ProfileInfo/Profile/Customer/PersonName/Surname"),
                new Memory.AdaptableSetOnProperty("Surname")
            );

            var guestScope = new MappingScopeComposite(
                new List<MappingScopeComposite>(),
                new List<Mapping>()
                {
                    guestIdMap,
                    guestGivenNameMap,
                    guestSurNameMap
                },
                new Xml.XmlGetScope("./ResGuests/ResGuest"),
                new Memory.AdaptableTraversalThis(),
                new Memory.AdaptableTraversalTemplate("Guests"),
                new Memory.AdaptableChildCreator()
            );

            var reservationHotelCodeMap = new Mapping(
                new Xml.XmlGet("./RoomStays/RoomStay/BasicPropertyInfo/@HotelCode"),
                new Memory.AdaptableSetOnProperty("HotelCode")
            );

            var reservationIdMap = new Mapping(
                new Xml.XmlGet("./ResGlobalInfo/HotelReservationIDs/HotelReservationID[@ResID_Type='18']/@ResID_Value"),
                new Memory.AdaptableSetOnProperty("Id")
            );

            var reservationScope = new MappingScopeComposite(
                new List<MappingScopeComposite>()
                {
                    roomStayScope,
                    guestScope
                },
                new List<Mapping>()
                {
                    reservationHotelCodeMap,
                    reservationIdMap
                },
                new Xml.XmlGetScope("//HotelReservations/HotelReservation"),
                new Memory.AdaptableTraversalThis(),
                new Memory.AdaptableTraversalTemplate("Reservations"),
                new Memory.AdaptableChildCreator()
            );

            System.Type testType = typeof(Root);

            var contextFactory = new Contexts.ContextFactory(
                new Xml.XmlObjectConverter(),
                new Memory.AdaptableTargetInstantiator(testType.Assembly.FullName, testType.FullName)
            );

            var mappingConfiguration = new MappingConfiguration(reservationScope, contextFactory);

            return mappingConfiguration;
        }
    }
}