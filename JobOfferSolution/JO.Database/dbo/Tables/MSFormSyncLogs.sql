CREATE TABLE [dbo].[MSFormSyncLogs] (
    [Id]           INT      IDENTITY (1, 1) NOT NULL,
    [SyncDate]     DATETIME NULL,
    [TotalRows]    INT      NULL,
    [InvalidCount] INT      NULL,
    CONSTRAINT [PK_MSFormSyncLogs] PRIMARY KEY CLUSTERED ([Id] ASC)
);

