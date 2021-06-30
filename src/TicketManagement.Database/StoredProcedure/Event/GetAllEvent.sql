CREATE PROCEDURE [dbo].[GetAllEvent]
AS
    SELECT Id, Name, Description, LayoutId, StartDateTime, EndDateTime, ImageUrl FROM [dbo].[Event]
GO
