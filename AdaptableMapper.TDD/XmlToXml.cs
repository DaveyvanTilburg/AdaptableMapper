using FluentAssertions;
using System.Collections.Generic;
using System.Xml.Linq;
using Xunit;

namespace AdaptableMapper.TDD
{
    public class XmlToXml
    {
        [Fact]
        public void XmlToXmlTest()
        {
            var errorObserver = new TestErrorObserver();
            Errors.ErrorObservable.GetInstance().Register(errorObserver);

            MappingConfiguration mappingConfiguration = GetMappingConfiguration();

            XElement result = Mapper.Map(mappingConfiguration, System.IO.File.ReadAllText(@".\Resources\BOO_Reservation.xml")) as XElement;

            Errors.ErrorObservable.GetInstance().Unregister(errorObserver);

            string expectedResult = System.IO.File.ReadAllText(@".\Resources\ExpectedSandboxResult.xml");
            XElement xExpectedResult = XElement.Parse(expectedResult);

            errorObserver.GetErrors().Count.Should().Be(0);

            result.Should().BeEquivalentTo(xExpectedResult);
        }

        [Fact]
        public void XmlTestIfResultIsString()
        {
            var errorObserver = new TestErrorObserver();
            Errors.ErrorObservable.GetInstance().Register(errorObserver);

            MappingConfiguration mappingConfiguration = GetMappingConfiguration();
            mappingConfiguration.ResultConverter = new Xml.XElementToStringObjectConverter();

            object result = Mapper.Map(mappingConfiguration, System.IO.File.ReadAllText(@".\Resources\BOO_Reservation.xml"));

            Errors.ErrorObservable.GetInstance().Unregister(errorObserver);

            XElement resultXElement = result as XElement;
            resultXElement.Should().BeNull();

            string resultString = result as string;
            resultString.Should().NotBeNull();
        }

        private static MappingConfiguration GetMappingConfiguration()
        {
            var roomStayGuestNameSearchMap = new Mapping(
                new Xml.XmlGetSearch(
                    "../../ResGuests/ResGuest[@ResGuestRPH='{{searchResult}}']/Profiles/ProfileInfo/Profile/Customer/PersonName/Surname",
                    "./ResGuestRPHs/ResGuestRPH/@RPH"
                ),
                new Xml.XmlSet("./RoomTypes/RoomType/RoomDescription/@GuestLastName")
            );

            var roomStayCodeMap = new Mapping(
                new Xml.XmlGet("./RoomTypes/RoomType/@RoomTypeCode"),
                new Xml.XmlSet("./RoomTypes/RoomType/@RoomTypeCode")
            );

            var roomStayScope = new MappingScopeComposite(
                new List<MappingScopeComposite>(),
                new List<Mapping>()
                {
                    roomStayGuestNameSearchMap,
                    roomStayCodeMap
                },
                new Xml.XmlGetScope("./RoomStays/RoomStay"),
                new Xml.XmlTraversal("./RoomStays"),
                new Xml.XmlTraversalTemplate("./RoomStay"),
                new Xml.XmlChildCreator()
            );

            var reservationHotelCodeMap = new Mapping(
                new Xml.XmlGet("./RoomStays/RoomStay/BasicPropertyInfo/@HotelCode"),
                new Xml.XmlSet("./ResGlobalInfo/BasicPropertyInfo/@HotelCode")
            );

            var reservationIdMap = new Mapping(
                new Xml.XmlGet("./ResGlobalInfo/HotelReservationIDs/HotelReservationID[@ResID_Type='18']/@ResID_Value"),
                new Xml.XmlSet("./ResGlobalInfo/HotelReservationIDs/HotelReservationID[@ResID_Type='5']/@ResID_Value")
            );

            var reservationScope = new MappingScopeComposite(
                new List<MappingScopeComposite>()
                {
                    roomStayScope
                },
                new List<Mapping>()
                {
                    reservationHotelCodeMap,
                    reservationIdMap
                },
                new Xml.XmlGetScope("//HotelReservations/HotelReservation"),
                new Xml.XmlTraversal("./ReservationsList"),
                new Xml.XmlTraversalTemplate("//HotelReservation"),
                new Xml.XmlChildCreator()
            );

            var contextFactory = new Contexts.ContextFactory(
                new Xml.XmlObjectConverter(),
                new Xml.XmlTargetInstantiator(System.IO.File.ReadAllText(@".\Resources\SandboxTemplate.xml"))
            );

            var mappingConfiguration = new MappingConfiguration(reservationScope, contextFactory, new NullObjectConverter());

            return mappingConfiguration;
        }
    }
}