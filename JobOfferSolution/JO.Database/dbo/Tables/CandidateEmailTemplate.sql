CREATE TABLE [dbo].[CandidateEmailTemplate] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [EmailSubject]   NVARCHAR (500) NULL,
    [EmailMessage]   NTEXT          NULL,
    [OtherRecipient] NVARCHAR (500) NULL,
    [CCRecipient]    NVARCHAR (500) NULL,
    [IsActive]       BIT            NULL,
    [CreatedAt]      DATETIME       NULL,
    [CreatedBy]      INT            NULL,
    [ModifiedAt]     DATETIME       NULL,
    [ModifiedBy]     INT            NULL,
    CONSTRAINT [PK_CandidateEmailTemplate] PRIMARY KEY CLUSTERED ([Id] ASC)
);

