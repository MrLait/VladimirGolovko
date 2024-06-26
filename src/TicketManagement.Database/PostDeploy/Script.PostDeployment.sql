﻿--- Venue
insert into dbo.Venue
values 
('Luzhniki Stadium', 'st. Luzhniki, 24, Moscow, Russia, 119048', '+7 495 780-08-08'),
('Gomel Regional Drama Theater', 'pl. Lenin 1, Gomel 246050', '+375232757763'),
('The circus', 'pl. Lenin 1, Brest 246050', '+375442757763')

--- Layout
insert into dbo.Layout
values 
(1, 'Layout for football games.'),
(1, 'Layout for concerts.'),
(2, 'Layout for comedy performances.'),
(2, 'Layout for detective performances.'),
(2, 'Layout to deleteTests.')

--- Area
insert into dbo.Area
values 
(1, 'First sector of first layout.', 1, 1),
(1, 'Second sector of first layout.', 1, 2),
(1, 'Third sector of first layout.', 1, 3),
(1, 'Fourth sector of first layout.', 1, 4),
(2, 'First sector of second layout.', 1, 1),
(2, 'Third sector of second layout.', 1, 3),
(2, 'Fourth sector of second layout.', 1, 4),
(2, 'Fifth sector of second layout.', 1, 5),
(3, 'Parterre of first layout.', 1, 1),
(3, 'Balcony of first layout.', 2, 1),
(4, 'Parterre of second layout.', 2, 1),
(4, 'Area to test layout.', 2, 2)

--- Seat
insert into dbo.Seat
values 
(1, 1, 1), (1, 1, 2), (1, 1, 3),
(1, 2, 1), (1, 2, 2), (1, 2, 3),
(2, 1, 1), (2, 1, 2), (2, 1, 3),
(2, 2, 1), (2, 2, 2), (2, 2, 3),
(3, 1, 1), (3, 1, 2), (3, 1, 3),
(3, 2, 1), (3, 2, 2), (3, 2, 3),
(4, 1, 1), (4, 1, 2), (4, 1, 3),
(4, 2, 1), (4, 2, 2), (4, 2, 3),
(5, 1, 1), (5, 1, 2), (5, 1, 3),
(5, 2, 1), (5, 2, 2), (5, 2, 3),
(6, 1, 1), (6, 1, 2), (6, 1, 3),
(6, 2, 1), (6, 2, 2), (6, 2, 3),
(7, 1, 1), (7, 1, 2), (7, 1, 3),
(7, 2, 1), (7, 2, 2), (7, 2, 3),
(8, 1, 1), (8, 1, 2), (8, 1, 3),
(8, 2, 1), (8, 2, 2), (8, 2, 3),
(9, 1, 1), (9, 1, 2), (9, 1, 3),
(9, 2, 1), (9, 2, 2), (9, 2, 3),
(10, 1, 1), (10, 1, 2), (10, 1, 3),
(10, 2, 1), (10, 2, 2), (10, 2, 3),
(11, 1, 1), (11, 1, 2), (11, 1, 3),
(11, 2, 1), (11, 2, 2), (11, 2, 3),
(11, 3, 3)

----- Event
insert into dbo.Event
values
('Footbal match.', 'Netherlands - Russia', 1, N'2021-03-01 00:00:00', N'2021-03-01 01:00:00', 'Pics/FootballPicOne.PNG'),
('Football match.', 'Netherlands - Belarus', 1, N'2021-04-01 00:00:00', N'2021-04-01 02:00:00', 'Pics/FootballPicTwo.PNG'),
('Event to test.', 'Netherlands - Belarus', 1, N'2021-04-01 00:00:00', N'2021-04-01 03:00:00', 'Pics/FootballPicOne.PNG')

INSERT INTO [dbo].[EventArea] ([EventId], [Description], [CoordX], [CoordY], [Price]) 
VALUES
(1, N'First sector of first layout.', 1, 1, CAST(100 AS Decimal(18, 0))),
(1, N'Second sector of first layout.', 1, 2, CAST(100 AS Decimal(18, 0))),
(1, N'Third sector of first layout.', 1, 3, CAST(100 AS Decimal(18, 0))),
(1, N'Fourth sector of first layout.', 1, 4, CAST(100 AS Decimal(18, 0))),
(2, N'First sector of second layout.', 1, 1, CAST(100 AS Decimal(18, 0))),
(2, N'Third sector of second layout.', 1, 3, CAST(100 AS Decimal(18, 0))),
(2, N'Fourth sector of second layout.', 1, 4, CAST(100 AS Decimal(18, 0))),
(2, N'Fifth sector of second layout.', 1, 5, CAST(100 AS Decimal(18, 0)))

INSERT INTO [dbo].[EventSeat] ([EventAreaId], [Row], [Number], [State]) 
VALUES 
(1, 1, 1, 0),
(1, 1, 2, 0),
(1, 1, 3, 0),
(1, 2, 1, 0),
(1, 2, 2, 0),
(1, 2, 3, 0),
(2, 1, 1, 0),
(2, 1, 2, 0),
(2, 1, 3, 0),
(2, 2, 1, 0),
(2, 2, 2, 0),
(2, 2, 3, 0),
(3, 1, 1, 0),
(3, 1, 2, 0),
(3, 1, 3, 0),
(3, 2, 1, 0),
(3, 2, 2, 0),
(3, 2, 3, 0),
(4, 1, 1, 0),
(4, 1, 2, 0),
(4, 1, 3, 0),
(4, 2, 1, 0),
(4, 2, 2, 0),
(4, 2, 3, 0),
(5, 1, 1, 0),
(5, 1, 2, 0),
(5, 1, 3, 0),
(5, 2, 1, 0),
(5, 2, 2, 0),
(5, 2, 3, 0),
(6, 1, 1, 0),
(6, 1, 2, 0),
(6, 1, 3, 0),
(6, 2, 1, 0),
(6, 2, 2, 0),
(6, 2, 3, 0),
(7, 1, 1, 0),
(7, 1, 2, 0),
(7, 1, 3, 0),
(7, 2, 1, 0),
(7, 2, 2, 0),
(7, 2, 3, 0),
(8, 1, 1, 0),
(8, 1, 2, 0),
(8, 1, 3, 0),
(8, 2, 1, 0),
(8, 2, 2, 0),
(8, 2, 3, 0)