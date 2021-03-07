using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using TicketManagement.DataAccess.Domain.Models;
using TicketManagement.DataAccess.Interfaces;

namespace TicketManagement.UnitTests.DataAccess
{
    public class MockEntites
    {
        public List<Venue> Venues { get; set; }

        public List<Layout> Layouts { get; set; }

        public List<Area> Areas { get; set; }

        public List<Seat> Seats { get; set; }

        public List<Event> Events { get; set; }

        public List<EventArea> EventAreas { get; set; }

        public List<EventSeat> EventSeats { get; set; }

        public Mock<IDbContext> Mock { get; set; }

        [SetUp]
        public void InitialMock()
        {
            Venues = new List<Venue>
            {
                new Venue { Id = 1, Description = "Luzhniki Stadium", Address = "st. Luzhniki, 24, Moscow, Russia, 119048", Phone = "+7 495 780-08-08" },
                new Venue { Id = 2, Description = "Gomel Regional Drama Theater", Address = "pl. Lenin 1, Gomel 246050", Phone = "+375232757763" },
                new Venue { Id = 3, Description = "The circus", Address = "pl. Lenin 1, Brest 246050", Phone = "+375442757763" },
            };

            Layouts = new List<Layout>
            {
                new Layout { Id = 1, VenueId = 1, Description = "Layout for football games." },
                new Layout { Id = 2, VenueId = 1, Description = "Layout for concerts." },
                new Layout { Id = 3, VenueId = 2, Description = "Layout for comedy performances." },
                new Layout { Id = 4, VenueId = 2, Description = "Layout for detective performances." },
                new Layout { Id = 5, VenueId = 2, Description = "Layout to deleteTests." },
            };

            Areas = new List<Area>
            {
                new Area { Id = 1, LayoutId = 1,  Description = "First sector of first layout.", CoordX = 1, CoordY = 1 },
                new Area { Id = 2, LayoutId = 1,  Description = "Second sector of first layout.", CoordX = 1, CoordY = 2 },
                new Area { Id = 3, LayoutId = 1,  Description = "Third sector of first layout.", CoordX = 1, CoordY = 3 },
                new Area { Id = 4, LayoutId = 1,  Description = "Fourth sector of first layout.", CoordX = 1, CoordY = 4 },
                new Area { Id = 5, LayoutId = 2,  Description = "First sector of second layout.", CoordX = 1, CoordY = 1 },
                new Area { Id = 6, LayoutId = 2,  Description = "Third sector of second layout.", CoordX = 1, CoordY = 3 },
                new Area { Id = 7, LayoutId = 2,  Description = "Fourth sector of second layout.", CoordX = 1, CoordY = 4 },
                new Area { Id = 8, LayoutId = 2,  Description = "Fifth sector of second layout.", CoordX = 1, CoordY = 5 },
                new Area { Id = 9, LayoutId = 3,  Description = "Parterre of first layout.", CoordX = 1, CoordY = 1 },
                new Area { Id = 10, LayoutId = 3,  Description = "Balcony of first layout.", CoordX = 2, CoordY = 1 },
                new Area { Id = 11, LayoutId = 4,  Description = "Parterre of second layout.", CoordX = 2, CoordY = 1 },
                new Area { Id = 12, LayoutId = 4,  Description = "Area to test layout.", CoordX = 2, CoordY = 2 },
            };

            Seats = new List<Seat>
            {
                new Seat { Id = 1, AreaId = 1, Row = 1, Number = 1 },
                new Seat { Id = 2, AreaId = 1, Row = 1, Number = 2 },
                new Seat { Id = 3, AreaId = 1, Row = 1, Number = 3 },
                new Seat { Id = 4, AreaId = 1, Row = 2, Number = 1 },
                new Seat { Id = 5, AreaId = 1, Row = 2, Number = 2 },
                new Seat { Id = 6, AreaId = 1, Row = 2, Number = 3 },
                new Seat { Id = 7, AreaId = 2, Row = 1, Number = 1 },
                new Seat { Id = 8, AreaId = 2, Row = 1, Number = 2 },
                new Seat { Id = 9, AreaId = 2, Row = 1, Number = 3 },
                new Seat { Id = 10, AreaId = 2, Row = 2, Number = 1 },
                new Seat { Id = 11, AreaId = 2, Row = 2, Number = 2 },
                new Seat { Id = 12, AreaId = 2, Row = 2, Number = 3 },
                new Seat { Id = 13, AreaId = 3, Row = 1, Number = 1 },
                new Seat { Id = 14, AreaId = 3, Row = 1, Number = 2 },
                new Seat { Id = 15, AreaId = 3, Row = 1, Number = 3 },
                new Seat { Id = 16, AreaId = 3, Row = 2, Number = 1 },
                new Seat { Id = 17, AreaId = 3, Row = 2, Number = 2 },
                new Seat { Id = 18, AreaId = 3, Row = 2, Number = 3 },
                new Seat { Id = 19, AreaId = 4, Row = 1, Number = 1 },
                new Seat { Id = 20, AreaId = 4, Row = 1, Number = 2 },
                new Seat { Id = 21, AreaId = 4, Row = 1, Number = 3 },
                new Seat { Id = 22, AreaId = 4, Row = 2, Number = 1 },
                new Seat { Id = 23, AreaId = 4, Row = 2, Number = 2 },
                new Seat { Id = 24, AreaId = 4, Row = 2, Number = 3 },
                new Seat { Id = 25, AreaId = 5, Row = 1, Number = 1 },
                new Seat { Id = 26, AreaId = 5, Row = 1, Number = 2 },
                new Seat { Id = 27, AreaId = 5, Row = 1, Number = 3 },
                new Seat { Id = 28, AreaId = 5, Row = 2, Number = 1 },
                new Seat { Id = 29, AreaId = 5, Row = 2, Number = 2 },
                new Seat { Id = 30, AreaId = 5, Row = 2, Number = 3 },
                new Seat { Id = 31, AreaId = 6, Row = 1, Number = 1 },
                new Seat { Id = 32, AreaId = 6, Row = 1, Number = 2 },
                new Seat { Id = 33, AreaId = 6, Row = 1, Number = 3 },
                new Seat { Id = 34, AreaId = 6, Row = 2, Number = 1 },
                new Seat { Id = 35, AreaId = 6, Row = 2, Number = 2 },
                new Seat { Id = 36, AreaId = 6, Row = 2, Number = 3 },
                new Seat { Id = 37, AreaId = 7, Row = 1, Number = 1 },
                new Seat { Id = 38, AreaId = 7, Row = 1, Number = 2 },
                new Seat { Id = 39, AreaId = 7, Row = 1, Number = 3 },
                new Seat { Id = 40, AreaId = 7, Row = 2, Number = 1 },
                new Seat { Id = 41, AreaId = 7, Row = 2, Number = 2 },
                new Seat { Id = 42, AreaId = 7, Row = 2, Number = 3 },
                new Seat { Id = 43, AreaId = 8, Row = 1, Number = 1 },
                new Seat { Id = 44, AreaId = 8, Row = 1, Number = 2 },
                new Seat { Id = 45, AreaId = 8, Row = 1, Number = 3 },
                new Seat { Id = 46, AreaId = 8, Row = 2, Number = 1 },
                new Seat { Id = 47, AreaId = 8, Row = 2, Number = 2 },
                new Seat { Id = 48, AreaId = 8, Row = 2, Number = 3 },
                new Seat { Id = 49, AreaId = 9, Row = 1, Number = 1 },
                new Seat { Id = 50, AreaId = 9, Row = 1, Number = 2 },
                new Seat { Id = 51, AreaId = 9, Row = 1, Number = 3 },
                new Seat { Id = 52, AreaId = 9, Row = 2, Number = 1 },
                new Seat { Id = 53, AreaId = 9, Row = 2, Number = 2 },
                new Seat { Id = 54, AreaId = 9, Row = 2, Number = 3 },
                new Seat { Id = 55, AreaId = 10, Row = 1, Number = 1 },
                new Seat { Id = 56, AreaId = 10, Row = 1, Number = 2 },
                new Seat { Id = 57, AreaId = 10, Row = 1, Number = 3 },
                new Seat { Id = 58, AreaId = 10, Row = 2, Number = 1 },
                new Seat { Id = 59, AreaId = 10, Row = 2, Number = 2 },
                new Seat { Id = 60, AreaId = 10, Row = 2, Number = 3 },
                new Seat { Id = 61, AreaId = 11, Row = 1, Number = 1 },
                new Seat { Id = 62, AreaId = 11, Row = 1, Number = 2 },
                new Seat { Id = 63, AreaId = 11, Row = 1, Number = 3 },
                new Seat { Id = 64, AreaId = 11, Row = 2, Number = 1 },
                new Seat { Id = 65, AreaId = 11, Row = 2, Number = 2 },
                new Seat { Id = 66, AreaId = 11, Row = 2, Number = 3 },
                new Seat { Id = 67, AreaId = 11, Row = 3, Number = 3 },
            };

            EventAreas = new List<EventArea>
            {
                new EventArea { Id = 1, EventId = 1, Description = "First sector of first layout.", CoordX = 1, CoordY = 1, Price = 100 },
                new EventArea { Id = 2, EventId = 1, Description = "Second sector of first layout.", CoordX = 1, CoordY = 2, Price = 100 },
                new EventArea { Id = 3, EventId = 1, Description = "Third sector of first layout.", CoordX = 1, CoordY = 3, Price = 100 },
                new EventArea { Id = 4, EventId = 1, Description = "Fourth sector of first layout.", CoordX = 1, CoordY = 4, Price = 100 },
                new EventArea { Id = 5, EventId = 2, Description = "First sector of second layout.", CoordX = 1, CoordY = 1, Price = 100 },
                new EventArea { Id = 6, EventId = 2, Description = "Third sector of second layout.", CoordX = 1, CoordY = 3, Price = 100 },
                new EventArea { Id = 7, EventId = 2, Description = "Fourth sector of second layout.", CoordX = 1, CoordY = 4, Price = 100 },
                new EventArea { Id = 8, EventId = 2, Description = "Fifth sector of second layout.", CoordX = 1, CoordY = 5, Price = 100 },
            };

            EventSeats = new List<EventSeat>
            {
                new EventSeat { Id = 1, EventAreaId = 1, Row = 1, Number = 1, State = 0 },
                new EventSeat { Id = 2, EventAreaId = 1, Row = 1, Number = 2, State = 0 },
                new EventSeat { Id = 3, EventAreaId = 1, Row = 1, Number = 3, State = 0 },
                new EventSeat { Id = 4, EventAreaId = 1, Row = 2, Number = 1, State = 0 },
                new EventSeat { Id = 5, EventAreaId = 1, Row = 2, Number = 2, State = 0 },
                new EventSeat { Id = 6, EventAreaId = 1, Row = 2, Number = 3, State = 0 },
                new EventSeat { Id = 7, EventAreaId = 2, Row = 1, Number = 1, State = 0 },
                new EventSeat { Id = 8, EventAreaId = 2, Row = 1, Number = 2, State = 0 },
                new EventSeat { Id = 9, EventAreaId = 2, Row = 1, Number = 3, State = 0 },
                new EventSeat { Id = 10, EventAreaId = 2, Row = 2, Number = 1, State = 0 },
                new EventSeat { Id = 11, EventAreaId = 2, Row = 2, Number = 2, State = 0 },
                new EventSeat { Id = 12, EventAreaId = 2, Row = 2, Number = 3, State = 0 },
                new EventSeat { Id = 13, EventAreaId = 3, Row = 1, Number = 1, State = 0 },
                new EventSeat { Id = 14, EventAreaId = 3, Row = 1, Number = 2, State = 0 },
                new EventSeat { Id = 15, EventAreaId = 3, Row = 1, Number = 3, State = 0 },
                new EventSeat { Id = 16, EventAreaId = 3, Row = 2, Number = 1, State = 0 },
                new EventSeat { Id = 17, EventAreaId = 3, Row = 2, Number = 2, State = 0 },
                new EventSeat { Id = 18, EventAreaId = 3, Row = 2, Number = 3, State = 0 },
                new EventSeat { Id = 19, EventAreaId = 4, Row = 1, Number = 1, State = 0 },
                new EventSeat { Id = 20, EventAreaId = 4, Row = 1, Number = 2, State = 0 },
                new EventSeat { Id = 21, EventAreaId = 4, Row = 1, Number = 3, State = 0 },
                new EventSeat { Id = 22, EventAreaId = 4, Row = 2, Number = 1, State = 0 },
                new EventSeat { Id = 23, EventAreaId = 4, Row = 2, Number = 2, State = 0 },
                new EventSeat { Id = 24, EventAreaId = 4, Row = 2, Number = 3, State = 0 },
                new EventSeat { Id = 25, EventAreaId = 5, Row = 1, Number = 1, State = 0 },
                new EventSeat { Id = 26, EventAreaId = 5, Row = 1, Number = 2, State = 0 },
                new EventSeat { Id = 27, EventAreaId = 5, Row = 1, Number = 3, State = 0 },
                new EventSeat { Id = 28, EventAreaId = 5, Row = 2, Number = 1, State = 0 },
                new EventSeat { Id = 29, EventAreaId = 5, Row = 2, Number = 2, State = 0 },
                new EventSeat { Id = 30, EventAreaId = 5, Row = 2, Number = 3, State = 0 },
                new EventSeat { Id = 31, EventAreaId = 6, Row = 1, Number = 1, State = 0 },
                new EventSeat { Id = 32, EventAreaId = 6, Row = 1, Number = 2, State = 0 },
                new EventSeat { Id = 33, EventAreaId = 6, Row = 1, Number = 3, State = 0 },
                new EventSeat { Id = 34, EventAreaId = 6, Row = 2, Number = 1, State = 0 },
                new EventSeat { Id = 35, EventAreaId = 6, Row = 2, Number = 2, State = 0 },
                new EventSeat { Id = 36, EventAreaId = 6, Row = 2, Number = 3, State = 0 },
                new EventSeat { Id = 37, EventAreaId = 7, Row = 1, Number = 1, State = 0 },
                new EventSeat { Id = 38, EventAreaId = 7, Row = 1, Number = 2, State = 0 },
                new EventSeat { Id = 39, EventAreaId = 7, Row = 1, Number = 3, State = 0 },
                new EventSeat { Id = 40, EventAreaId = 7, Row = 2, Number = 1, State = 0 },
                new EventSeat { Id = 41, EventAreaId = 7, Row = 2, Number = 2, State = 0 },
                new EventSeat { Id = 42, EventAreaId = 7, Row = 2, Number = 3, State = 0 },
                new EventSeat { Id = 43, EventAreaId = 8, Row = 1, Number = 1, State = 0 },
                new EventSeat { Id = 44, EventAreaId = 8, Row = 1, Number = 2, State = 0 },
                new EventSeat { Id = 45, EventAreaId = 8, Row = 1, Number = 3, State = 0 },
                new EventSeat { Id = 46, EventAreaId = 8, Row = 2, Number = 1, State = 0 },
                new EventSeat { Id = 47, EventAreaId = 8, Row = 2, Number = 2, State = 0 },
                new EventSeat { Id = 48, EventAreaId = 8, Row = 2, Number = 3, State = 0 },
            };

            Events = new List<Event>
            {
            };
            Mock = new Mock<IDbContext>();
            Mock.Setup(x => x.Venues.GetAll()).Returns(Venues);
            Mock.Setup(x => x.Seats.GetAll()).Returns(Seats);
            Mock.Setup(x => x.Layouts.GetAll()).Returns(Layouts);
            Mock.Setup(x => x.Areas.GetAll()).Returns(Areas);
            Mock.Setup(x => x.EventAreas.GetAll()).Returns(EventAreas);
            Mock.Setup(x => x.EventSeats.GetAll()).Returns(EventSeats);
            ////Mock.Setup(x => x.Events.GetAll()).Returns(Events);
        }
    }
}
