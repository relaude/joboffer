CREATE TABLE [dbo].[CompBenItmCat] (
    [Id]      INT            IDENTITY (1, 1) NOT NULL,
    [CatName] NVARCHAR (200) NULL,
    CONSTRAINT [PK_CompBenItmCat] PRIMARY KEY CLUSTERED ([Id] ASC)
);

