CREATE TABLE [dbo].[JobOffers] (
    [Id]                INT             IDENTITY (1, 1) NOT NULL,
    [CreatedAt]         DATETIME        NULL,
    [JobOfferNumber]    NVARCHAR (50)   NULL,
    [Candidate_Id]      INT             NULL,
    [JobPosition_Id]    INT             NULL,
    [Department_Id]     INT             NULL,
    [BasicSalary]       DECIMAL (18, 2) NULL,
    [Allowance]         DECIMAL (18, 2) NULL,
    [SigningBonus]      DECIMAL (18, 2) NULL,
    [OfferDate]         DATETIME        NULL,
    [ProposedStartDate] DATETIME        NULL,
    [Remarks]           NVARCHAR (500)  NULL,
    [MainStatus_Id]     INT             NULL,
    CONSTRAINT [PK_JobOffers] PRIMARY KEY CLUSTERED ([Id] ASC)
);

