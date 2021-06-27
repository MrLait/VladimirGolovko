CREATE PROCEDURE [dbo].[UpdateEvent]
	@Id				int,
	@Name           varchar(120),
	@Description    varchar(max),
	@LayoutId       int,
	@StartDateTime	DateTime,
	@EndDateTime	DateTime,
	@ImageUrl		varchar(max)
AS
    UPDATE [dbo].[Event]
	SET Name = @Name, Description = @Description, LayoutId = @LayoutId, StartDateTime = @StartDateTime, EndDateTime = @EndDateTime, ImageUrl = @ImageUrl
	WHERE Id = @Id
	SELECT * FROM [dbo].[Event] WHERE Id = @Id
GO