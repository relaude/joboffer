CREATE TABLE [dbo].[DboxCandidateStatus] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [StatusName] NVARCHAR (200) NULL,
    CONSTRAINT [PK_DboxCandidateStatus] PRIMARY KEY CLUSTERED ([Id] ASC)
);

