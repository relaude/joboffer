CREATE TABLE [dbo].[JobOffers] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL,
    [RefNum]       NVARCHAR (50) NULL,
    [CompanyId]    INT           NULL,
    [DivisionId]   INT           NULL,
    [DepartmentId] INT           NULL,
    [CandidateId]  INT           NULL,
    [RequestId]    INT           NULL,
    [DocumentId]   INT           NULL,
    [LegalId]      INT           NULL,
    [StatusId]     INT           NULL,
    [Options]      INT           NULL,
    [OfferRangeId] INT           NULL,
    [Escalate]     BIT           NULL,
    [CreatedAt]    DATETIME      NULL,
    [CreatedBy]    INT           NULL,
    [ModifiedAt]   DATETIME      NULL,
    [ModifiedBy]   INT           NULL,
    CONSTRAINT [PK_JobOffers] PRIMARY KEY CLUSTERED ([Id] ASC)
);

