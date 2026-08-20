CREATE TABLE [dbo].[CompensationTemplateItems] (
    [Id]         INT IDENTITY (1, 1) NOT NULL,
    [TemplateId] INT NULL,
    [ItemId]     INT NULL,
    [IsEnabled]  BIT NULL,
    CONSTRAINT [PK_CompensationTemplateItems] PRIMARY KEY CLUSTERED ([Id] ASC)
);

