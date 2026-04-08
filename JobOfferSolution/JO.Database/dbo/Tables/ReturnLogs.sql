CREATE TABLE [dbo].[ReturnLogs] (
    [Id]              INT            IDENTITY (1, 1) NOT NULL,
    [CreatedAt]       DATETIME       NULL,
    [CreatedBy]       INT            NULL,
    [JobOffer_Id]     INT            NULL,
    [ReturnReason_Id] INT            NULL,
    [Remarks]         NVARCHAR (150) NULL,
    CONSTRAINT [PK_ReturnLogs] PRIMARY KEY CLUSTERED ([Id] ASC)
);

