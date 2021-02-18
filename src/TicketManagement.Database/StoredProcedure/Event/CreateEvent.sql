CREATE PROCEDURE [dbo].[CreateEvent]
	@Name           varchar(120),
	@Description    varchar(max),
	@LayoutId       int
AS
    INSERT INTO [dbo].[Event] (Name, Description, LayoutId)
    VALUES (@Name, @Description, @LayoutId)

    SELECT SCOPE_IDENTITY()
GO