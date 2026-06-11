CREATE TABLE [dbo].[CandidateMassUploadLogs] (
    [Id]                     INT      IDENTITY (1, 1) NOT NULL,
    [TotalExcelItems]        INT      NULL,
    [TotalCandidateTransfer] INT      NULL,
    [CreatedBy]              INT      NULL,
    [CreatedAt]              DATETIME NULL,
    CONSTRAINT [PK_CandidateMassUploadLogs] PRIMARY KEY CLUSTERED ([Id] ASC)
);

