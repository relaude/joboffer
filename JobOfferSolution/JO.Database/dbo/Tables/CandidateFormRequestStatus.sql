CREATE TABLE [dbo].[CandidateFormRequestStatus] (
    [Id]         INT           IDENTITY (1, 1) NOT NULL,
    [StatusName] NVARCHAR (50) NULL,
    CONSTRAINT [PK_CandidateFormRequestStatus] PRIMARY KEY CLUSTERED ([Id] ASC)
);

