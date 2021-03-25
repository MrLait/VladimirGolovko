CREATE PROCEDURE [dbo].[GetAllEvent]
AS
    SELECT Id, Name, Description, LayoutId, DateTime FROM [dbo].[Event]
GO
