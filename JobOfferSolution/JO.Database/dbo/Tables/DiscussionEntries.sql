CREATE TABLE [dbo].[DiscussionEntries] (
    [Id]                     INT            IDENTITY (1, 1) NOT NULL,
    [CandidateApplicationId] INT            NULL,
    [JobOfferProposalId]     INT            NULL,
    [DiscussionStepId]       INT            NULL,
    [DiscussionDate]         DATETIME       NULL,
    [ChannelTypeId]          INT            NULL,
    [CandidateResponseId]    INT            NULL,
    [TANotes]                NVARCHAR (500) NULL,
    [CandidateFeedback]      NVARCHAR (500) NULL,
    [CreatedAt]              DATETIME       NULL,
    [CreatedBy]              INT            NULL,
    CONSTRAINT [PK_DiscussionEntries] PRIMARY KEY CLUSTERED ([Id] ASC)
);

