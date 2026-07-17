CREATE TABLE [dbo].[DHJOProposal] (
    [Id]                 INT            IDENTITY (1, 1) NOT NULL,
    [JobOfferProposalId] INT            NULL,
    [ProposalStatusId]   INT            NULL,
    [Comments]           NVARCHAR (500) NULL,
    [CreatedAt]          DATETIME       NULL,
    [CreatedBy]          INT            NULL,
    CONSTRAINT [PK_DHJOProposal] PRIMARY KEY CLUSTERED ([Id] ASC)
);

