IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [Suppliers] (
    [Id] uniqueidentifier NOT NULL,
    [Name] varchar(200) NOT NULL,
    [IdentificationDocument] varchar(14) NOT NULL,
    [SupplierType] tinyint NOT NULL,
    [Active] bit NOT NULL,
    CONSTRAINT [PK_Suppliers] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Addresses] (
    [Id] uniqueidentifier NOT NULL,
    [Street] varchar(200) NOT NULL,
    [Number] varchar(50) NOT NULL,
    [Complement] varchar(250) NOT NULL,
    [ZipCode] varchar(8) NOT NULL,
    [Neighborhood] varchar(100) NOT NULL,
    [City] nvarchar(max) NULL,
    [State] varchar(50) NOT NULL,
    [SupplierId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_Addresses] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Addresses_Suppliers_SupplierId] FOREIGN KEY ([SupplierId]) REFERENCES [Suppliers] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [Products] (
    [Id] uniqueidentifier NOT NULL,
    [Name] varchar(200) NOT NULL,
    [Description] varchar(1000) NOT NULL,
    [Image] varchar(100) NOT NULL,
    [Price] decimal(18,2) NOT NULL,
    [RegisterDate] datetime2 NOT NULL DEFAULT (GETDATE()),
    [Active] bit NOT NULL,
    [SupplierId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_Products] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Products_Suppliers_SupplierId] FOREIGN KEY ([SupplierId]) REFERENCES [Suppliers] ([Id]) ON DELETE NO ACTION
);

GO

CREATE UNIQUE INDEX [IX_Addresses_SupplierId] ON [Addresses] ([SupplierId]);

GO

CREATE INDEX [IX_Products_SupplierId] ON [Products] ([SupplierId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210609124826_Initial', N'3.1.13');

GO

