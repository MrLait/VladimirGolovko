CREATE PROCEDURE [dbo].[CreateEvent]
	@Name           varchar(120),
	@Description    varchar(max),
	@LayoutId       int,
    @StartDateTime  Datetime,
    @EndDateTime    Datetime,
    @ImageUrl       varchar(max)
AS
    INSERT INTO [dbo].[Event] (Name, Description, LayoutId, StartDateTime, EndDateTime, ImageUrl)
    VALUES (@Name, @Description, @LayoutId, @StartDateTime, @EndDateTime, @ImageUrl)

    SELECT SCOPE_IDENTITY()
GO