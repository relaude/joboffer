CREATE TABLE [dbo].[CandidateApplications] (
    [Id]               INT      IDENTITY (1, 1) NOT NULL,
    [CandidateId]      INT      NULL,
    [MSFormRequestId]  INT      NULL,
    [HiringDivisionId] INT      NULL,
    [CurrencyId]       INT      NULL,
    [CreatedBy]        INT      NULL,
    [CreatedAt]        DATETIME NULL,
    CONSTRAINT [PK_CandidateApplications] PRIMARY KEY CLUSTERED ([Id] ASC)
);

