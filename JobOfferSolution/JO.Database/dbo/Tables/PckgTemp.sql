CREATE TABLE [dbo].[PckgTemp] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [TempName]   NVARCHAR (200) NULL,
    [CreatedAt]  DATETIME       NULL,
    [CreatedBy]  INT            NULL,
    [ModifiedAt] DATETIME       NULL,
    [ModifiedBy] INT            NULL,
    CONSTRAINT [PK_PckgTemp] PRIMARY KEY CLUSTERED ([Id] ASC)
);

