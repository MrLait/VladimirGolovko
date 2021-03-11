CREATE PROCEDURE [dbo].[CreateEvent]
	@Name           varchar(120),
	@Description    varchar(max),
	@LayoutId       int,
    @DateTime       Datetime
AS
    INSERT INTO [dbo].[Event] (Name, Description, LayoutId, DateTime)
    VALUES (@Name, @Description, @LayoutId, @DateTime)

    SELECT SCOPE_IDENTITY()
GO