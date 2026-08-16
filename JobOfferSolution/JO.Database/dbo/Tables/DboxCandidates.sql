CREATE TABLE [dbo].[DboxCandidates] (
    [Id]         INT           IDENTITY (1, 1) NOT NULL,
    [ResponseId] INT           NULL,
    [DboxId]     INT           NULL,
    [DboxRefNum] NVARCHAR (50) NULL,
    [CSGId]      INT           NULL,
    CONSTRAINT [PK_DboxCandidates] PRIMARY KEY CLUSTERED ([Id] ASC)
);

