CREATE TABLE [dbo].[CompenItemCategory] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL,
    [CategoryName] NVARCHAR (50) NULL,
    CONSTRAINT [PK_CompenItemCategory] PRIMARY KEY CLUSTERED ([Id] ASC)
);

