CREATE TABLE [dbo].[DBoxAsyncLogs] (
    [Id]        INT      IDENTITY (1, 1) NOT NULL,
    [SyncDate]  DATETIME NULL,
    [TotalRows] INT      NULL,
    CONSTRAINT [PK_DBoxAsyncLogs] PRIMARY KEY CLUSTERED ([Id] ASC)
);
