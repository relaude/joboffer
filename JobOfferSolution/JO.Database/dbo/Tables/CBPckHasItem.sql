CREATE TABLE [dbo].[CBPckHasItem] (
    [Id]     INT IDENTITY (1, 1) NOT NULL,
    [PckgId] INT NULL,
    [ItemId] INT NULL,
    CONSTRAINT [PK_CBPckHasItem] PRIMARY KEY CLUSTERED ([Id] ASC)
);

