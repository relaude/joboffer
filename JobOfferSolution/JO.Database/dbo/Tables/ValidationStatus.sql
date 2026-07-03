CREATE TABLE [dbo].[ValidationStatus] (
    [Id]                   INT           IDENTITY (1, 1) NOT NULL,
    [ValidationStatusName] NVARCHAR (50) NULL,
    [OrderBy]              INT           NULL,
    CONSTRAINT [PK_ValidationStatus] PRIMARY KEY CLUSTERED ([Id] ASC)
);

