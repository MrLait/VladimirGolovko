CREATE TABLE [dbo].[Basket]
(
    [Id] int identity primary key,
    [ProductId] INT NOT NULL, 
    [UserId] NVARCHAR(MAX) NOT NULL
)
