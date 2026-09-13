CREATE TABLE [dbo].[JOAnalysis] (
    [Id]               INT             IDENTITY (1, 1) NOT NULL,
    [JobOfferId]       INT             NULL,
    [CandidateRemarks] NVARCHAR (2000) NULL,
    [CreatedAt]        DATETIME        NULL,
    [CreatedBy]        INT             NULL,
    [ModifiedAt]       DATETIME        NULL,
    [ModifiedBy]       INT             NULL,
    [ActivityRemarks]  NVARCHAR (500)  NULL,
    CONSTRAINT [PK_JOAnalysis] PRIMARY KEY CLUSTERED ([Id] ASC)
);

