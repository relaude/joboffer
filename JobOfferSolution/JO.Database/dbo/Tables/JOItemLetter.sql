CREATE TABLE [dbo].[JOItemLetter] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL,
    [ItemId]       INT           NULL,
    [DisplayOrder] INT           NULL,
    [ItemName]     NVARCHAR (50) NULL,
    [MessageBody]  NTEXT         NULL,
    CONSTRAINT [PK_JOItemLetter] PRIMARY KEY CLUSTERED ([Id] ASC)
);

