CREATE TABLE [dbo].[Discussions] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [JobOfferId] INT            NULL,
    [ProposalId] INT            NULL,
    [StepId]     INT            NULL,
    [ChannelId]  INT            NULL,
    [ResponseId] INT            NULL,
    [Comments]   NVARCHAR (500) NULL,
    [FeedBack]   NVARCHAR (500) NULL,
    [DiscussAt]  DATETIME       NULL,
    [CreatedBy]  INT            NULL,
    [CreatedAt]  DATETIME       NULL,
    CONSTRAINT [PK_Discussions] PRIMARY KEY CLUSTERED ([Id] ASC)
);

