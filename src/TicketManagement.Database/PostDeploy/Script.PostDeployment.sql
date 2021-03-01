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
values (1, 'First area of first layout', 1, 1),
(1, 'Second area of first layout', 1, 1),
(2, 'First area of second layout', 4, 4)

--- Seat
insert into dbo.Seat
values (1, 1, 1),
(1, 1, 2),
(1, 1, 3),
(1, 2, 2),
(2, 1, 1),
(1, 2, 1)
