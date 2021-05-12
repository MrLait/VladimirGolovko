CREATE PROCEDURE [dbo].[GetByIdEvent]
	@Id int
AS
	SELECT Id, Name, Description, LayoutId, StartDateTime, EndDateTime, ImageUrl FROM [dbo].[Event] WHERE Id = @Id
GO
