CREATE TABLE [dbo].[DboxCandidatesRawData] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [CandidateId]   NVARCHAR (MAX) NULL,
    [CandidateName] NVARCHAR (MAX) NULL,
    [Company]       NVARCHAR (MAX) NULL,
    [Division]      NVARCHAR (MAX) NULL,
    [Department]    NVARCHAR (MAX) NULL,
    [CostCenter]    NVARCHAR (MAX) NULL,
    [JobLevel]      NVARCHAR (MAX) NULL,
    [JobPosition]   NVARCHAR (MAX) NULL,
    [EmailAddress]  NVARCHAR (MAX) NULL,
    [ContactNumber] NVARCHAR (MAX) NULL,
    [CreatedAt]     DATETIME       NULL,
    [CreatedBy]     INT            NULL,
    CONSTRAINT [PK_DboxCandidatesRawData] PRIMARY KEY CLUSTERED ([Id] ASC)
);

