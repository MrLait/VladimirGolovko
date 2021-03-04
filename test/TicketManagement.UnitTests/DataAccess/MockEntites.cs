using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Repositories.AdoRepositories;

namespace TicketManagement.UnitTests.DataAccess
{
    public class MockEntites
    {
        public IEnumerable<Venue> Venues { get; set; }

        public IEnumerable<Layout> Layouts { get; set; }

        public IEnumerable<Area> Areas { get; set; }

        public IEnumerable<Seat> Seats { get; set; }

        public IEnumerable<Event> Events { get; set; }

        public Mock<IRepository<Venue>> VenueMock { get; set; }

        [SetUp]
        public void InitialMock()
        {
            Venues = new List<Venue>
            {
                new Venue { Description = "Luzhniki Stadium", Address = "st. Luzhniki, 24, Moscow, Russia, 119048", Phone = "+7 495 780-08-08" },
                new Venue { Description = "Gomel Regional Drama Theater", Address = "pl. Lenin 1, Gomel 246050", Phone = "+375232757763" },
            };

            Layouts = new List<Layout>
            {
                new Layout { VenueId = 1, Description = "Layout for football games." },
                new Layout { VenueId = 1, Description = "Layout for concerts." },
                new Layout { VenueId = 2, Description = "Layout for comedy performances." },
                new Layout { VenueId = 2, Description = "Layout for detective performances." },
            };

            Areas = new List<Area>
            {
                new Area { LayoutId = 1,  Description = "First sector of first layout.", CoordX = 1, CoordY = 1 },
                new Area { LayoutId = 1,  Description = "Second sector of first layout.", CoordX = 1, CoordY = 2 },
                new Area { LayoutId = 1,  Description = "Third sector of first layout.", CoordX = 1, CoordY = 3 },
                new Area { LayoutId = 1,  Description = "Fourth sector of first layout.", CoordX = 1, CoordY = 4 },
                new Area { LayoutId = 2,  Description = "First sector of second layout.", CoordX = 1, CoordY = 1 },
                new Area { LayoutId = 2,  Description = "Third sector of second layout.", CoordX = 1, CoordY = 3 },
                new Area { LayoutId = 2,  Description = "Fourth sector of second layout.", CoordX = 1, CoordY = 4 },
                new Area { LayoutId = 2,  Description = "Fifth sector of second layout.", CoordX = 1, CoordY = 5 },
                new Area { LayoutId = 3,  Description = "Parterre of first layout.", CoordX = 1, CoordY = 1 },
                new Area { LayoutId = 3,  Description = "Balcony of first layout.", CoordX = 2, CoordY = 1 },
                new Area { LayoutId = 4,  Description = "Parterre of second layout.", CoordX = 2, CoordY = 1 },
            };

            Seats = new List<Seat>
            {
                new Seat { AreaId = 1, Row = 1, Number = 1 },
                new Seat { AreaId = 1, Row = 1, Number = 2 },
                new Seat { AreaId = 1, Row = 1, Number = 3 },
                new Seat { AreaId = 1, Row = 2, Number = 1 },
                new Seat { AreaId = 1, Row = 2, Number = 2 },
                new Seat { AreaId = 1, Row = 2, Number = 3 },
                new Seat { AreaId = 2, Row = 1, Number = 1 },
                new Seat { AreaId = 2, Row = 1, Number = 2 },
                new Seat { AreaId = 2, Row = 1, Number = 3 },
                new Seat { AreaId = 2, Row = 2, Number = 1 },
                new Seat { AreaId = 2, Row = 2, Number = 2 },
                new Seat { AreaId = 2, Row = 2, Number = 3 },
                new Seat { AreaId = 3, Row = 1, Number = 1 },
                new Seat { AreaId = 3, Row = 1, Number = 2 },
                new Seat { AreaId = 3, Row = 1, Number = 3 },
                new Seat { AreaId = 3, Row = 2, Number = 1 },
                new Seat { AreaId = 3, Row = 2, Number = 2 },
                new Seat { AreaId = 3, Row = 2, Number = 3 },
                new Seat { AreaId = 4, Row = 1, Number = 1 },
                new Seat { AreaId = 4, Row = 1, Number = 2 },
                new Seat { AreaId = 4, Row = 1, Number = 3 },
                new Seat { AreaId = 4, Row = 2, Number = 1 },
                new Seat { AreaId = 4, Row = 2, Number = 2 },
                new Seat { AreaId = 4, Row = 2, Number = 3 },
                new Seat { AreaId = 5, Row = 1, Number = 1 },
                new Seat { AreaId = 5, Row = 1, Number = 2 },
                new Seat { AreaId = 5, Row = 1, Number = 3 },
                new Seat { AreaId = 5, Row = 2, Number = 1 },
                new Seat { AreaId = 5, Row = 2, Number = 2 },
                new Seat { AreaId = 5, Row = 2, Number = 3 },
                new Seat { AreaId = 6, Row = 1, Number = 1 },
                new Seat { AreaId = 6, Row = 1, Number = 2 },
                new Seat { AreaId = 6, Row = 1, Number = 3 },
                new Seat { AreaId = 6, Row = 2, Number = 1 },
                new Seat { AreaId = 6, Row = 2, Number = 2 },
                new Seat { AreaId = 6, Row = 2, Number = 3 },
                new Seat { AreaId = 7, Row = 1, Number = 1 },
                new Seat { AreaId = 7, Row = 1, Number = 2 },
                new Seat { AreaId = 7, Row = 1, Number = 3 },
                new Seat { AreaId = 7, Row = 2, Number = 1 },
                new Seat { AreaId = 7, Row = 2, Number = 2 },
                new Seat { AreaId = 7, Row = 2, Number = 3 },
                new Seat { AreaId = 8, Row = 1, Number = 1 },
                new Seat { AreaId = 8, Row = 1, Number = 2 },
                new Seat { AreaId = 8, Row = 1, Number = 3 },
                new Seat { AreaId = 8, Row = 2, Number = 1 },
                new Seat { AreaId = 8, Row = 2, Number = 2 },
                new Seat { AreaId = 8, Row = 2, Number = 3 },
                new Seat { AreaId = 9, Row = 1, Number = 1 },
                new Seat { AreaId = 9, Row = 1, Number = 2 },
                new Seat { AreaId = 9, Row = 1, Number = 3 },
                new Seat { AreaId = 9, Row = 2, Number = 1 },
                new Seat { AreaId = 9, Row = 2, Number = 2 },
                new Seat { AreaId = 9, Row = 2, Number = 3 },
                new Seat { AreaId = 10, Row = 1, Number = 1 },
                new Seat { AreaId = 10, Row = 1, Number = 2 },
                new Seat { AreaId = 10, Row = 1, Number = 3 },
                new Seat { AreaId = 10, Row = 2, Number = 1 },
                new Seat { AreaId = 10, Row = 2, Number = 2 },
                new Seat { AreaId = 10, Row = 2, Number = 3 },
                new Seat { AreaId = 11, Row = 1, Number = 1 },
                new Seat { AreaId = 11, Row = 1, Number = 2 },
                new Seat { AreaId = 11, Row = 1, Number = 3 },
                new Seat { AreaId = 11, Row = 2, Number = 1 },
                new Seat { AreaId = 11, Row = 2, Number = 2 },
                new Seat { AreaId = 11, Row = 2, Number = 3 },
            };

            Events = new List<Event>
            {
            };
            VenueMock = new Mock<IRepository<Venue>>();
            VenueMock.Setup(x => x.GetAll()).Returns(Venues);
            ////Mock = new Mock<IDbContext>();
            ////Mock.Setup(x => x.Venues.GetAll()).Returns(Venues);
            ////Mock.Setup(x => x.Layouts.GetAll()).Returns(Layouts);
            ////Mock.Setup(x => x.Areas.GetAll()).Returns(Areas);
            ////Mock.Setup(x => x.Seats.GetAll()).Returns(Seats);
            ////Mock.Setup(x => x.Events.GetAll()).Returns(Events);
        }
    }
}
