CREATE TABLE [dbo].[CompensationItem] (
    [Id]           INT             IDENTITY (1, 1) NOT NULL,
    [ItemName]     NVARCHAR (150)  NULL,
    [CategoryId]   INT             NULL,
    [Monthly]      DECIMAL (18, 2) NULL,
    [Annualy]      DECIMAL (18, 2) NULL,
    [DisplayOrder] INT             NULL,
    CONSTRAINT [PK_CompensationItem] PRIMARY KEY CLUSTERED ([Id] ASC)
);

