CREATE TABLE [AspNetUsers] (
        [Id] nvarchar(450) NOT NULL,
        [UserName] nvarchar(256) NULL,
        [NormalizedUserName] nvarchar(256) NULL,
        [Email] nvarchar(256) NULL,
        [NormalizedEmail] nvarchar(256) NULL,
        [EmailConfirmed] bit NOT NULL,
        [PasswordHash] nvarchar(max) NULL,
        [SecurityStamp] nvarchar(max) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        [PhoneNumber] nvarchar(max) NULL,
        [PhoneNumberConfirmed] bit NOT NULL,
        [TwoFactorEnabled] bit NOT NULL,
        [LockoutEnd] datetimeoffset NULL,
        [LockoutEnabled] bit NOT NULL,
        [AccessFailedCount] int NOT NULL,
        [FirstName] NVARCHAR(256) NULL, 
    [Surname] NVARCHAR(256) NULL, 
    [Language] NVARCHAR(256) NULL, 
    [TimeZoneOffset] NVARCHAR(256) NULL, 
    [Balance] DECIMAL NULL, 
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
    );
    GO
    CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail])
    
    GO
    CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL