﻿using FluentAssertions;
using System.Collections.Generic;
using System.Xml.Linq;
using Xunit;
using AdaptableMapper.Memory.Language;

namespace AdaptableMapper.TDD
{
    public class ModelToXml
    {
        [Fact]
        public void ModelToXmlTest()
        {
            var errorObserver = new TestErrorObserver();
            Errors.ErrorObservable.GetInstance().Register(errorObserver);

            MappingConfiguration mappingConfiguration = GetFakedMappingConfiguration();

            ModelBase source = GetSource();
            XElement result = Mapper.Map(mappingConfiguration, source) as XElement;

            Errors.ErrorObservable.GetInstance().Unregister(errorObserver);

            string expectedResult = System.IO.File.ReadAllText(@".\Resources\ExpectedSandboxResult.xml");
            XElement xExpectedResult = XElement.Parse(expectedResult);

            errorObserver.GetErrors().Count.Should().Be(12);

            result.Should().BeEquivalentTo(xExpectedResult);
        }

        private Root GetSource()
        {
            var reservation1roomstay1 = new RoomStay()
            {
                Code = "6281801",
                GuestId = "1",
                GuestName = "S*****",
                RateCode = "65090",
                Name = "",
                Text = ""
            };
            var reservation1roomstay2 = new RoomStay()
            {
                Code = "6281802",
                GuestId = "2",
                GuestName = "D****",
                RateCode = "65090",
                Name = "",
                Text = ""
            };

            var reservation1guest1 = new Guest()
            {
                GuestId = "1",
                GivenName = "S*****",
                Surname = "K**************"
            };

            var reservation1guest2 = new Guest()
            {
                GuestId = "2",
                GivenName = "D****",
                Surname = "T**************"
            };

            var reservation1 = new Reservation()
            {
                Id = "03a804fa",
                HotelCode = "62818",
                RoomStays = new List<RoomStay>()
                {
                    reservation1roomstay1,
                    reservation1roomstay2
                },
                Guests = new List<Guest>()
                {
                    reservation1guest1,
                    reservation1guest2
                }
            };

            reservation1roomstay1.Parent = reservation1;
            reservation1roomstay2.Parent = reservation1;
            reservation1guest1.Parent = reservation1;
            reservation1guest2.Parent = reservation1;

            var reservation2roomstay1 = new RoomStay()
            {
                Code = "6281801",
                GuestId = "1",
                GuestName = "S*****",
                RateCode = "65090",
                Name = "",
                Text = ""
            };
            var reservation2roomstay2 = new RoomStay()
            {
                Code = "6281801",
                GuestId = "1",
                GuestName = "S*****",
                RateCode = "65090",
                Name = "",
                Text = ""
            };

            var reservation2guest1 = new Guest()
            {
                GuestId = "1",
                GivenName = "S*****",
                Surname = "K**************"
            };

            var reservation2 = new Reservation()
            {
                Id = "03a804fb",
                HotelCode = "62818",
                RoomStays = new List<RoomStay>()
                {
                    reservation2roomstay1,
                    reservation2roomstay2
                },
                Guests = new List<Guest>()
                {
                    reservation2guest1
                }
            };

            reservation2roomstay1.Parent = reservation2;
            reservation2roomstay2.Parent = reservation2;
            reservation2guest1.Parent = reservation2;

            var root = new Root()
            {
                Reservations = new List<Reservation>()
                {
                    reservation1,
                    reservation2
                }
            };

            reservation1.Parent = root;
            reservation2.Parent = root;

            return root;
        }

        private MappingConfiguration GetFakedMappingConfiguration()
        {
            var roomGuestLastNameSearch = new Mapping(
                new Memory.ModelGetSearch(
                    "../Guests{'PropertyName':'GuestId','Value':'{{searchResult}}'}/Surname",
                    "GuestId"
                ),
                new Xml.XmlSet("./RoomTypes/RoomType/RoomDescription/@GuestLastName")
            );

            var roomStayTestObjectFail = new Mapping(
                new Memory.ModelGet("Test"),
                new Xml.XmlSet("./RoomTypes/RoomType/RoomDescription/Text")
            );

            var roomStayNameXPathFail = new Mapping(
                new Memory.ModelGet("Name"),
                new Xml.XmlSet("./RoomTypes/RoomType/RoomDescription/@Naem")
            );

            var roomStayCodeMap = new Mapping(
                new Memory.ModelGet("Code"),
                new Xml.XmlSet("./RoomTypes/RoomType/@RoomTypeCode")
            );

            var roomStayScope = new MappingScopeComposite(
                new List<MappingScopeComposite>(),
                new List<Mapping>()
                {
                    roomGuestLastNameSearch,
                    roomStayTestObjectFail,
                    roomStayNameXPathFail,
                    roomStayCodeMap
                },
                new Memory.ModelGetScope("RoomStays"),
                new Xml.XmlTraversal("./RoomStays"),
                new Xml.XmlTraversalTemplate("./RoomStay"),
                new Xml.XmlChildCreator()
            );

            var reservationHotelCodeMap = new Mapping(
                new Memory.ModelGet("HotelCode"),
                new Xml.XmlSet("./ResGlobalInfo/BasicPropertyInfo/@HotelCode")
            );

            var reservationIdMap = new Mapping(
                new Memory.ModelGet("Id"),
                new Xml.XmlSet("./ResGlobalInfo/HotelReservationIDs/HotelReservationID[@ResID_Type='5']/@ResID_Value")
            );

            var reservationScope = new MappingScopeComposite(
                new List<MappingScopeComposite>()
                {
                    roomStayScope
                },
                new List<Mapping>()
                {
                    reservationIdMap,
                    reservationHotelCodeMap
                },
                new Memory.ModelGetScope("Reservations"),
                new Xml.XmlTraversal("./ReservationsList"),
                new Xml.XmlTraversalTemplate("//HotelReservation"),
                new Xml.XmlChildCreator()
            );

            var contextFactory = new Contexts.ContextFactory(
                new Memory.ModelObjectConverter(),
                new Xml.XmlTargetInstantiator(System.IO.File.ReadAllText(@".\Resources\SandboxTemplate.xml"))
            );

            var mappingConfiguration = new MappingConfiguration(reservationScope, contextFactory, new NullObjectConverter());

            return mappingConfiguration;
        }
    }
}