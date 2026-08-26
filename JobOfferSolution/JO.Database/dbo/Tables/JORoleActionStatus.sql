CREATE TABLE [dbo].[JORoleActionStatus] (
    [Id]         INT IDENTITY (1, 1) NOT NULL,
    [JobOfferId] INT NULL,
    [RoleId]     INT NULL,
    [ActionId]   INT NULL,
    CONSTRAINT [PK_JORoleActionStatus] PRIMARY KEY CLUSTERED ([Id] ASC)
);

