CREATE TABLE [dbo].[JobOfferAnalysis] (
    [Id]                     INT             IDENTITY (1, 1) NOT NULL,
    [CandidateApplicationId] INT             NULL,
    [PackageId]              INT             NULL,
    [ExpectedSalary]         DECIMAL (18, 2) NULL,
    [BestProposalSalary]     DECIMAL (18, 2) NULL,
    [ValidationStatusId]     INT             NULL,
    [RecommendProposalId]    INT             NULL,
    [SalaryMatrixBandId]     INT             NULL,
    [AnalysisNotes]          NVARCHAR (200)  NULL,
    [CreatedBy]              INT             NULL,
    [CreatedAt]              DATETIME        NULL,
    CONSTRAINT [PK_JobOfferAnalysis] PRIMARY KEY CLUSTERED ([Id] ASC)
);

