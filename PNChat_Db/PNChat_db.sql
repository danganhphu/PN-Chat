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
GO

CREATE TABLE [Group] (
    [Code] varchar(32) NOT NULL,
    [Type] varchar(32) NOT NULL,
    [Avatar] varchar(max) NULL,
    [Name] nvarchar(250) NULL,
    [Created] datetime NOT NULL,
    [CreatedBy] varchar(32) NOT NULL,
    [LastActive] datetime NOT NULL,
    CONSTRAINT [PK_Group] PRIMARY KEY ([Code])
);
DECLARE @defaultSchema AS sysname;
SET @defaultSchema = SCHEMA_NAME();
DECLARE @description AS sql_variant;
SET @description = CONCAT(N'single: chat 1-1', NCHAR(13), NCHAR(10), N'multi: chat 1-n');
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', @defaultSchema, 'TABLE', N'Group', 'COLUMN', N'Type';
GO

CREATE TABLE [GroupCall] (
    [Code] varchar(32) NOT NULL,
    [Type] varchar(32) NOT NULL,
    [Avatar] varchar(max) NULL,
    [Name] nvarchar(250) NULL,
    [Created] datetime NOT NULL,
    [CreatedBy] varchar(32) NOT NULL,
    [LastActive] datetime NOT NULL,
    CONSTRAINT [PK_GroupCall] PRIMARY KEY ([Code])
);
DECLARE @defaultSchema AS sysname;
SET @defaultSchema = SCHEMA_NAME();
DECLARE @description AS sql_variant;
SET @description = CONCAT(N'single: chat 1-1', NCHAR(13), NCHAR(10), N'multi: chat 1-n');
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', @defaultSchema, 'TABLE', N'GroupCall', 'COLUMN', N'Type';
GO

CREATE TABLE [User] (
    [Code] varchar(32) NOT NULL,
    [UserName] varchar(32) NULL,
    [Password] varchar(124) NULL,
    [FullName] nvarchar(50) NULL,
    [Dob] varchar(50) NULL,
    [Phone] varchar(50) NULL,
    [Email] varchar(50) NULL,
    [Address] nvarchar(255) NULL,
    [Avatar] varchar(max) NULL,
    [Gender] nvarchar(10) NULL,
    [LastLogin] datetime NULL,
    [CurrentSession] varchar(500) NULL,
    CONSTRAINT [PK_User] PRIMARY KEY ([Code])
);
GO

CREATE TABLE [Call] (
    [Id] int NOT NULL IDENTITY,
    [GroupCallCode] varchar(32) NOT NULL,
    [UserCode] varchar(32) NOT NULL,
    [Url] nvarchar(500) NOT NULL,
    [Status] varchar(32) NOT NULL,
    [Created] datetime NOT NULL,
    CONSTRAINT [PK_Call] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Call_GroupCall] FOREIGN KEY ([GroupCallCode]) REFERENCES [GroupCall] ([Code]),
    CONSTRAINT [FK_Call_User] FOREIGN KEY ([UserCode]) REFERENCES [User] ([Code])
);
GO

CREATE TABLE [Contact] (
    [Id] bigint NOT NULL IDENTITY,
    [UserCode] varchar(32) NOT NULL,
    [ContactCode] varchar(32) NOT NULL,
    [Created] datetime NOT NULL,
    CONSTRAINT [PK_Contact] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Contact_User] FOREIGN KEY ([UserCode]) REFERENCES [User] ([Code]),
    CONSTRAINT [FK_Contact_User1] FOREIGN KEY ([ContactCode]) REFERENCES [User] ([Code])
);
GO

CREATE TABLE [GroupUser] (
    [Id] bigint NOT NULL IDENTITY,
    [GroupCode] varchar(32) NOT NULL,
    [UserCode] varchar(32) NOT NULL,
    CONSTRAINT [PK_GroupUser] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_GroupUser_Group] FOREIGN KEY ([GroupCode]) REFERENCES [Group] ([Code]),
    CONSTRAINT [FK_GroupUser_User] FOREIGN KEY ([UserCode]) REFERENCES [User] ([Code])
);
GO

CREATE TABLE [Message] (
    [Id] bigint NOT NULL IDENTITY,
    [Type] varchar(10) NOT NULL,
    [GroupCode] varchar(32) NOT NULL,
    [Content] nvarchar(max) NULL,
    [Path] nvarchar(255) NULL,
    [Created] datetime NOT NULL,
    [CreatedBy] varchar(32) NOT NULL,
    CONSTRAINT [PK_Message] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Message_Group] FOREIGN KEY ([GroupCode]) REFERENCES [Group] ([Code]),
    CONSTRAINT [FK_Message_User] FOREIGN KEY ([CreatedBy]) REFERENCES [User] ([Code])
);
DECLARE @defaultSchema AS sysname;
SET @defaultSchema = SCHEMA_NAME();
DECLARE @description AS sql_variant;
SET @description = CONCAT(N'text', NCHAR(13), NCHAR(10), N'media', NCHAR(13), NCHAR(10), N'attachment');
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', @defaultSchema, 'TABLE', N'Message', 'COLUMN', N'Type';
GO

CREATE INDEX [IX_Call_GroupCallCode] ON [Call] ([GroupCallCode]);
GO

CREATE INDEX [IX_Call_UserCode] ON [Call] ([UserCode]);
GO

CREATE INDEX [IX_Contact_ContactCode] ON [Contact] ([ContactCode]);
GO

CREATE INDEX [IX_Contact_UserCode] ON [Contact] ([UserCode]);
GO

CREATE INDEX [IX_GroupUser_GroupCode] ON [GroupUser] ([GroupCode]);
GO

CREATE INDEX [IX_GroupUser_UserCode] ON [GroupUser] ([UserCode]);
GO

CREATE INDEX [IX_Message_CreatedBy] ON [Message] ([CreatedBy]);
GO

CREATE INDEX [IX_Message_GroupCode] ON [Message] ([GroupCode]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20230320051622_InitialPNChatDb', N'7.0.4');
GO

COMMIT;
GO

