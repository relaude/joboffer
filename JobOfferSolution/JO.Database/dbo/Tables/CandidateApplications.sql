CREATE TABLE [dbo].[CandidateApplications] (
    [Id]                      INT      IDENTITY (1, 1) NOT NULL,
    [CandidateId]             INT      NULL,
    [RequisitionId]           INT      NULL,
    [HiringCompanyId]         INT      NULL,
    [LegalEntityId]           INT      NULL,
    [ApplicationStatusId]     INT      NULL,
    [CurrentWorkflowStatusId] INT      NULL,
    [ApplicationDate]         DATETIME NULL,
    [EndorsementDate]         DATETIME NULL,
    [TAOwnerId]               INT      NULL,
    [CreatedAt]               DATETIME NULL,
    CONSTRAINT [PK_CandidateApplications] PRIMARY KEY CLUSTERED ([Id] ASC)
);

