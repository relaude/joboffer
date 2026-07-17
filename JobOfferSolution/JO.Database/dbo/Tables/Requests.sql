CREATE TABLE [dbo].[Requests] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [CandidateId] INT            NULL,
    [SentAt]      DATETIME       NULL,
    [DueAt]       DATETIME       NULL,
    [Subject]     NVARCHAR (500) NULL,
    [Message]     TEXT           NULL,
    [Reminder1]   DATETIME       NULL,
    [Reminder2]   DATETIME       NULL,
    [CreatedAt]   DATETIME       NULL,
    [CreatedBy]   INT            NULL,
    CONSTRAINT [PK_CandidateMSFormRequests] PRIMARY KEY CLUSTERED ([Id] ASC)
);

