--- Venue
insert into dbo.Venue
values ('Luzhniki Stadium', 'st. Luzhniki, 24, Moscow, Russia, 119048', '+7 495 780−08−08'),
('Gomel Regional Drama Theater', 'pl. Lenin 1, Gomel 246050', '+375232757763')

--- Layout
insert into dbo.Layout
values (1, 'Layout for football games. '),
(1, 'Layout for concerts.'),
(2, 'Layout for comedy performances.'),
(2, 'Layout for detective performances.')

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
(4, 'Parterre of second layout.', 2, 1)

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
(11, 2, 1), (11, 2, 2), (11, 2, 3)

----- Event
insert into dbo.Event
values
('Footbal match', 'Netherlands - Russia', 1, N'2021-03-01 00:00:00'),
('Football match.', 'Netherlands - Belarus', 1, N'2021-04-01 00:00:00')
