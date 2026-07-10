CREATE TABLE [dbo].[CompBenItems] (
    [Id]              INT            IDENTITY (1, 1) NOT NULL,
    [ItemName]        NVARCHAR (50)  NULL,
    [ItemDescription] NVARCHAR (150) NULL,
    [TypeId]          INT            NULL,
    CONSTRAINT [PK_CompBenItems] PRIMARY KEY CLUSTERED ([Id] ASC)
);

