CREATE TABLE [dbo].[EmailTemplate] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [WorkFlowId]     INT            NULL,
    [EmailSubject]   NVARCHAR (500) NULL,
    [EmailMessage]   NTEXT          NULL,
    [OtherRecipient] NVARCHAR (500) NULL,
    [IsActive]       BIT            NULL,
    [CreatedAt]      DATETIME       NULL,
    [CreatedBy]      INT            NULL,
    [ModifiedAt]     DATETIME       NULL,
    [ModifiedBy]     INT            NULL,
    CONSTRAINT [PK_EmailTemplate] PRIMARY KEY CLUSTERED ([Id] ASC)
);

