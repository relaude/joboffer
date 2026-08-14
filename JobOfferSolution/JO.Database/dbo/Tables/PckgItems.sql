CREATE TABLE [dbo].[PckgItems] (
    [Id]       INT            IDENTITY (1, 1) NOT NULL,
    [ItemName] NVARCHAR (200) NULL,
    CONSTRAINT [PK_PckgItems] PRIMARY KEY CLUSTERED ([Id] ASC)
);

