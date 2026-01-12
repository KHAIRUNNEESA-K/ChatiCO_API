IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [Users] (
    [UserId] int NOT NULL IDENTITY,
    [Username] nvarchar(50) NOT NULL,
    [PhoneNumber] nvarchar(15) NOT NULL,
    [ProfilePicture] varbinary(max) NULL,
    [Bio] nvarchar(250) NULL,
    [isOnline] bit NOT NULL,
    [LastSeen] datetime2 NULL,
    [IsVerified] bit NOT NULL,
    [CreatedBy] nvarchar(max) NULL,
    [CreatedOn] datetime2 NULL,
    [ModifiedBy] nvarchar(max) NULL,
    [ModifiedOn] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    [IsDeleted] bit NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([UserId])
);

CREATE TABLE [Contacts] (
    [ContactId] int NOT NULL IDENTITY,
    [UserId] int NOT NULL,
    [ContactUserId] int NOT NULL,
    [CreatedBy] nvarchar(max) NULL,
    [CreatedOn] datetime2 NULL,
    [ModifiedBy] nvarchar(max) NULL,
    [ModifiedOn] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    [IsDeleted] bit NOT NULL,
    CONSTRAINT [PK_Contacts] PRIMARY KEY ([ContactId]),
    CONSTRAINT [FK_Contacts_Users_ContactUserId] FOREIGN KEY ([ContactUserId]) REFERENCES [Users] ([UserId]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Contacts_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId]) ON DELETE NO ACTION
);

CREATE TABLE [Login] (
    [LoginId] int NOT NULL IDENTITY,
    [UserId] int NOT NULL,
    [IsSuccessful] bit NOT NULL,
    [LoginTime] datetime NULL,
    [CreatedBy] nvarchar(max) NULL,
    [CreatedOn] datetime2 NULL,
    [ModifiedBy] nvarchar(max) NULL,
    [ModifiedOn] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    [IsDeleted] bit NOT NULL,
    CONSTRAINT [PK_Login] PRIMARY KEY ([LoginId]),
    CONSTRAINT [FK_Login_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId]) ON DELETE CASCADE
);

CREATE INDEX [IX_Contacts_ContactUserId] ON [Contacts] ([ContactUserId]);

CREATE INDEX [IX_Contacts_UserId] ON [Contacts] ([UserId]);

CREATE INDEX [IX_Login_UserId] ON [Login] ([UserId]);

CREATE UNIQUE INDEX [IX_Users_PhoneNumber] ON [Users] ([PhoneNumber]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251111104250_InitialCreate', N'9.0.10');

CREATE TABLE [Messages] (
    [MessageId] int NOT NULL IDENTITY,
    [SenderId] int NOT NULL,
    [ReceiverId] int NOT NULL,
    [MessageType] nvarchar(20) NOT NULL,
    [Content] VARBINARY(MAX) NULL,
    [FileName] nvarchar(255) NULL,
    [FileType] nvarchar(100) NULL,
    [SentTime] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
    [DeliveredTime] datetime2 NULL,
    [ReadTime] datetime2 NULL,
    [CreatedBy] nvarchar(max) NULL,
    [CreatedOn] datetime2 NULL,
    [ModifiedBy] nvarchar(max) NULL,
    [ModifiedOn] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    [IsDeleted] bit NOT NULL DEFAULT CAST(0 AS bit),
    CONSTRAINT [PK_Messages] PRIMARY KEY ([MessageId]),
    CONSTRAINT [FK_Messages_Users_ReceiverId] FOREIGN KEY ([ReceiverId]) REFERENCES [Users] ([UserId]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Messages_Users_SenderId] FOREIGN KEY ([SenderId]) REFERENCES [Users] ([UserId]) ON DELETE NO ACTION
);

CREATE INDEX [IX_Messages_ReceiverId] ON [Messages] ([ReceiverId]);

CREATE INDEX [IX_Messages_SenderId] ON [Messages] ([SenderId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251117104646_AddMessageEntity', N'9.0.10');

COMMIT;
GO

