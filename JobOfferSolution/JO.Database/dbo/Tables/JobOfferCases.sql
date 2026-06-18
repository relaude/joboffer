CREATE TABLE [dbo].[JobOfferCases] (
    [Id]             INT             IDENTITY (1, 1) NOT NULL,
    [CandidateId]    INT             NULL,
    [CompanyId]      INT             NULL,
    [LegalEntityId]  INT             NULL,
    [DepartmentId]   INT             NULL,
    [PositionLevel]  NVARCHAR (50)   NULL,
    [ProposedSalary] DECIMAL (18, 2) NULL,
    [AboveBand]      BIT             NULL,
    [CaseStatusId]   INT             NULL,
    CONSTRAINT [PK_JobOfferCases] PRIMARY KEY CLUSTERED ([Id] ASC)
);

