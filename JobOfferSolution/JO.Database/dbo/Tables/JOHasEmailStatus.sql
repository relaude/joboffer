CREATE TABLE [dbo].[JOHasEmailStatus] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL,
    [StatusName]   NVARCHAR (50) NULL,
    [DisplayOrder] INT           NULL,
    CONSTRAINT [PK_JOHasEmailStatus] PRIMARY KEY CLUSTERED ([Id] ASC)
);

