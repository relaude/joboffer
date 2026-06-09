CREATE TABLE [dbo].[CandidateCompensationProfile] (
    [Id]                     INT             IDENTITY (1, 1) NOT NULL,
    [CandidateApplicationId] INT             NULL,
    [CurrentBaseSalary]      DECIMAL (18, 2) NULL,
    [CurrentAllowance]       DECIMAL (18, 2) NULL,
    [CurrentBonus]           DECIMAL (18, 2) NULL,
    [ExpectedSalary]         DECIMAL (18, 2) NULL,
    [OtherBenefits]          NVARCHAR (MAX)  NULL,
    [SubmissionReferenceNo]  NVARCHAR (50)   NULL,
    [SubmissionDate]         DATETIME        NULL,
    [IsValidated]            BIT             NULL,
    [ValidationDate]         DATETIME        NULL,
    [ValidatedBy]            INT             NULL,
    [Remarks]                NVARCHAR (100)  NULL,
    CONSTRAINT [PK_CandidateCompensationProfile] PRIMARY KEY CLUSTERED ([Id] ASC)
);

