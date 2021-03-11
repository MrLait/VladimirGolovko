CREATE PROCEDURE [dbo].[GetByIdEvent]
	@Id int
AS
	SELECT * FROM [dbo].[Event] WHERE Id = @Id
GO
