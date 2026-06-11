CREATE TABLE [dbo].[CandidateMSFormRequests] (
    [Id]                     INT            IDENTITY (1, 1) NOT NULL,
    [CandidateId]            INT            NULL,
    [CandidateApplicationId] INT            NULL,
    [RequestSentDate]        DATETIME       NULL,
    [DueDate]                DATETIME       NULL,
    [SubmissionReferenceNo]  NVARCHAR (50)  NULL,
    [StatusId]               INT            NULL,
    [EmailSubject]           NVARCHAR (500) NULL,
    [EmailBody]              NVARCHAR (MAX) NULL,
    [Reminder1SentDate]      DATETIME       NULL,
    [Reminder2SentDate]      DATETIME       NULL,
    [CreatedAt]              DATETIME       NULL,
    [CreatedBy]              INT            NULL,
    CONSTRAINT [PK_CandidateMSFormRequests] PRIMARY KEY CLUSTERED ([Id] ASC)
);

