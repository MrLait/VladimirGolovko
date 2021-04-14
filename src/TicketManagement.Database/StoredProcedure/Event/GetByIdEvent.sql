CREATE PROCEDURE [dbo].[GetByIdEvent]
	@Id int
AS
	SELECT Id, Name, Description, LayoutId, DateTime FROM [dbo].[Event] WHERE Id = @Id
GO
