CREATE TABLE [dbo].[CompBenItems] (
    [Id]      INT             IDENTITY (1, 1) NOT NULL,
    [CatId]   INT             NULL,
    [ItmName] NVARCHAR (200)  NULL,
    [ItmDesc] NVARCHAR (200)  NULL,
    [Amount]  DECIMAL (18, 2) NULL,
    CONSTRAINT [PK_CompBenItems] PRIMARY KEY CLUSTERED ([Id] ASC)
);

