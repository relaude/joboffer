CREATE TABLE [dbo].[CandidateStatus] (
    [Id]         INT           IDENTITY (1, 1) NOT NULL,
    [StatusName] NVARCHAR (50) NULL,
    [OrderBy]    INT           NULL,
    CONSTRAINT [PK_CandidateStatus] PRIMARY KEY CLUSTERED ([Id] ASC)
);

