CREATE PROCEDURE [dbo].[UpdateEvent]
	@Id				int,
	@Name           varchar(120),
	@Description    varchar(max),
	@LayoutId       int,
	@DateTime		DateTime
AS
    UPDATE [dbo].[Event]
	SET Name = @Name, Description = @Description, LayoutId = @LayoutId, DateTime = @DateTime
	WHERE Id = @Id
	SELECT * FROM [dbo].[Event] WHERE Id = @Id
GO