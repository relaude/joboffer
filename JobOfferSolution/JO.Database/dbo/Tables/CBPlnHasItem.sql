CREATE TABLE [dbo].[CBPlnHasItem] (
    [Id]     INT IDENTITY (1, 1) NOT NULL,
    [PlanId] INT NULL,
    [ItemId] INT NULL,
    CONSTRAINT [PK_CBPlnHasItem] PRIMARY KEY CLUSTERED ([Id] ASC)
);

