CREATE TABLE [dbo].[TransactionAttachments] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [Transaction_Id] INT            NULL,
    [FileType_Id]    INT            NULL,
    [FileName]       NVARCHAR (50)  NULL,
    [FilePath]       NVARCHAR (200) NULL,
    [CreatedAt]      DATETIME       NULL,
    CONSTRAINT [PK_TransactionAttachments] PRIMARY KEY CLUSTERED ([Id] ASC)
);

