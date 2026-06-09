CREATE TABLE [dbo].[CandidateDocuments] (
    [Id]                     INT             IDENTITY (1, 1) NOT NULL,
    [CandidateApplicationId] INT             NULL,
    [DocumentTypeId]         INT             NULL,
    [FileName]               NVARCHAR (255)  NULL,
    [FilePath]               NVARCHAR (1000) NULL,
    [FileSize]               BIGINT          NULL,
    [ContentType]            NVARCHAR (100)  NULL,
    [VersionNo]              INT             NULL,
    [UploadedBy]             INT             NULL,
    [UploadedDate]           DATETIME        NULL,
    [IsActive]               BIT             NULL,
    CONSTRAINT [PK_CandidateDocuments] PRIMARY KEY CLUSTERED ([Id] ASC)
);

