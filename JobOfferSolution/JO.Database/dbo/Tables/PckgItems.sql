CREATE TABLE [dbo].[PckgItems] (
    [Id]           INT             IDENTITY (1, 1) NOT NULL,
    [CompenItemId] INT             NULL,
    [ItemName]     NVARCHAR (200)  NULL,
    [Analysis]     BIT             NULL,
    [Monthly]      DECIMAL (18, 2) NULL,
    [Annualy]      DECIMAL (18, 2) NULL,
    CONSTRAINT [PK_PckgItems] PRIMARY KEY CLUSTERED ([Id] ASC)
);

