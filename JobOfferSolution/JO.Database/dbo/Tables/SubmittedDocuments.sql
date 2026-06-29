CREATE TABLE [dbo].[SubmittedDocuments] (
    [Id]              INT             IDENTITY (1, 1) NOT NULL,
    [CandidateId]     INT             NULL,
    [MSFormRequestId] INT             NULL,
    [FileName]        NVARCHAR (255)  NULL,
    [FilePath]        NVARCHAR (1000) NULL,
    [FileSize]        INT             NULL,
    [FileTypeId]      INT             NULL,
    [UploadedBy]      INT             NULL,
    [UploadedDate]    DATETIME        NULL,
    CONSTRAINT [PK_CandidateDocuments] PRIMARY KEY CLUSTERED ([Id] ASC)
);

