CREATE TABLE [dbo].[CandidateApplications] (
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    [CandidateId]      INT            NULL,
    [MSFormRequestId]  INT            NULL,
    [HiringCompanyId]  INT            NULL,
    [HiringDivisionId] INT            NULL,
    [SalaryMatrixId]   INT            NULL,
    [ReferenceNumber]  NVARCHAR (150) NULL,
    [CreatedBy]        INT            NULL,
    [CreatedAt]        DATETIME       NULL,
    CONSTRAINT [PK_CandidateApplications] PRIMARY KEY CLUSTERED ([Id] ASC)
);

