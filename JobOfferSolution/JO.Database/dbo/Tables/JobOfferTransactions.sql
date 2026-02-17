CREATE TABLE [dbo].[JobOfferTransactions] (
    [Id]                INT           IDENTITY (1, 1) NOT NULL,
    [TransactionNumber] NVARCHAR (50) NULL,
    [MainStatus_Id]     INT           NULL,
    [Candidate_Id]      INT           NULL,
    [CreatedAt]         DATETIME      NULL,
    CONSTRAINT [PK_JobOfferTransactions] PRIMARY KEY CLUSTERED ([Id] ASC)
);

