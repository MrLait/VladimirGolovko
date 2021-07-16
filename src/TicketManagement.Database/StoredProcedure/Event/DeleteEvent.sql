CREATE PROCEDURE [dbo].[DeleteEvent]
	@Id     int
AS
    DELETE FROM [dbo].[Event]
    WHERE Id = @Id
GO