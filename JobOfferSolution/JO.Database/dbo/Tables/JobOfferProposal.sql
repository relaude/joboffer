CREATE TABLE [dbo].[JobOfferProposal] (
    [Id]                     INT             IDENTITY (1, 1) NOT NULL,
    [CandidateApplicationId] INT             NULL,
    [SalaryMatrixBandId]     INT             NULL,
    [OptionNumber]           INT             NULL,
    [CurrentSalary]          DECIMAL (18, 2) NULL,
    [ProposedSalary]         DECIMAL (18, 2) NULL,
    [SalaryMidpoint]         DECIMAL (18, 2) NULL,
    [CompaRatio]             DECIMAL (18, 2) NULL,
    [IncreasePercentage]     DECIMAL (18, 2) NULL,
    [AnnualSalary]           DECIMAL (18, 2) NULL,
    [ValidationStatusId]     INT             NULL,
    [Justification]          NVARCHAR (200)  NULL,
    [CreatedBy]              INT             NULL,
    [CreatedAt]              INT             NULL,
    CONSTRAINT [PK_JobOfferProposal] PRIMARY KEY CLUSTERED ([Id] ASC)
);

