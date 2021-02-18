CREATE PROCEDURE [dbo].[UpdateEvent]
	@Id				int,
	@Name           varchar(120),
	@Description    varchar(max),
	@LayoutId       int
AS
    UPDATE [dbo].[Event]
	SET Name = @Name, Description = @Description, LayoutId = @LayoutId
	WHERE Id = @Id
	SELECT * FROM [dbo].[Event] WHERE Id = @Id
GO