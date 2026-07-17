CREATE TABLE [dbo].[JobOffers] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [RefNum]      NVARCHAR (50) NULL,
    [CandidateId] INT           NULL,
    [RequestId]   INT           NULL,
    [DocumentId]  INT           NULL,
    [LegalId]     INT           NULL,
    [StatusId]    INT           NULL,
    [CreatedAt]   DATETIME      NULL,
    [CreatedBy]   INT           NULL,
    CONSTRAINT [PK_JobOffers] PRIMARY KEY CLUSTERED ([Id] ASC)
);

