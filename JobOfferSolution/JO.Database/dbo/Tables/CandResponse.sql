CREATE TABLE [dbo].[CandResponse] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL,
    [ResponseName] NVARCHAR (50) NULL,
    [DisplayOrder] INT           NULL,
    CONSTRAINT [PK_CandidateResponses] PRIMARY KEY CLUSTERED ([Id] ASC)
);

