CREATE TABLE [dbo].[CompensationItems] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [ItemName]     NVARCHAR (150) NULL,
    [CategoryId]   INT            NULL,
    [DisplayOrder] INT            NULL,
    CONSTRAINT [PK_CompensationItems] PRIMARY KEY CLUSTERED ([Id] ASC)
);

