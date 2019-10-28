﻿using AdaptableMapper.Traversals;
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

            MappingConfiguration deserializationConfiguration = GetFakedSerializationConfiguration();

            XElement result = Mapper.Map(deserializationConfiguration, System.IO.File.ReadAllText(@".\Resources\BOO_Reservation.xml")) as XElement;

            Errors.ErrorObservable.GetInstance().Unregister(errorObserver);

            string expectedResult = System.IO.File.ReadAllText(@".\Resources\ExpectedSandboxResult.xml");
            XElement xExpectedResult = XElement.Parse(expectedResult);

            result.Should().BeEquivalentTo(xExpectedResult);

            errorObserver.GetErrors().Count.Should().Be(0);
        }

        private static MappingConfiguration GetFakedSerializationConfiguration()
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

            var roomStayScope = new ScopeTraversalComposite(
                new List<ScopeTraversalComposite>(),
                new List<Mapping>()
                {
                    roomStayGuestNameSearchMap,
                    roomStayCodeMap
                },
                new Xml.XmlGetScope("./RoomStays/RoomStay"),
                new Xml.XmlTraversal("./RoomStays"),
                new Xml.XmlTraversalTemplate("./RoomStay"),
                new Xml.XmlCreateNewChild()
            );

            var reservationHotelCodeMap = new Mapping(
                new Xml.XmlGet("./RoomStays/RoomStay/BasicPropertyInfo/@HotelCode"),
                new Xml.XmlSet("./ResGlobalInfo/BasicPropertyInfo/@HotelCode")
            );

            var reservationIdMap = new Mapping(
                new Xml.XmlGet("./ResGlobalInfo/HotelReservationIDs/HotelReservationID[@ResID_Type='18']/@ResID_Value"),
                new Xml.XmlSet("./ResGlobalInfo/HotelReservationIDs/HotelReservationID[@ResID_Type='5']/@ResID_Value")
            );

            var reservationScope = new ScopeTraversalComposite(
                new List<ScopeTraversalComposite>()
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
                new Xml.XmlCreateNewChild()
            );

            var contextFactory = new Contexts.ContextFactory(
                new Xml.XmlObjectConverter(),
                new Xml.XmlTargetInstantiator(System.IO.File.ReadAllText(@".\Resources\SandboxTemplate.xml"))
            );

            var mappingConfiguration = new MappingConfiguration(reservationScope, contextFactory);

            return mappingConfiguration;
        }
    }
}