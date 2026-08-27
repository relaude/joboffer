CREATE TABLE [dbo].[JOAnalysis] (
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    [JobOfferId]       INT            NULL,
    [CandidateReamrks] NVARCHAR (500) NULL,
    [CreatedAt]        DATETIME       NULL,
    [CreatedBy]        INT            NULL,
    [ModifiedAt]       DATETIME       NULL,
    [ModifiedBy]       INT            NULL,
    CONSTRAINT [PK_JOAnalysis] PRIMARY KEY CLUSTERED ([Id] ASC)
);

