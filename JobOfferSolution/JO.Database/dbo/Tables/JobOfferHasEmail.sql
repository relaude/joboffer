CREATE TABLE [dbo].[JobOfferHasEmail] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [JobOfferId]   INT            NULL,
    [CandidateId]  INT            NULL,
    [StatusId]     INT            NULL,
    [ToRecipient]  NVARCHAR (200) NULL,
    [CCRecipient]  NVARCHAR (200) NULL,
    [Subject]      NVARCHAR (500) NULL,
    [EmailMessage] NTEXT          NULL,
    [CreatedAt]    DATETIME       NULL,
    [CreatedBy]    INT            NULL,
    [ModifiedAt]   DATETIME       NULL,
    [ModifiedBy]   INT            NULL,
    CONSTRAINT [PK_JobOfferHasEmail] PRIMARY KEY CLUSTERED ([Id] ASC)
);

