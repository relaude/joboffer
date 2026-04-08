CREATE TABLE [dbo].[EmailNotifications] (
    [Id]              INT            IDENTITY (1, 1) NOT NULL,
    [CreatedBy]       INT            NULL,
    [CreatedAt]       DATETIME       NULL,
    [ModifiedBy]      INT            NULL,
    [ModifiedAt]      DATETIME       NULL,
    [Activity_Id]     INT            NULL,
    [EmailSubject]    NVARCHAR (100) NULL,
    [EmailMessage]    TEXT           NULL,
    [OtherRecipients] NVARCHAR (550) NULL,
    [Active]          BIT            NULL,
    CONSTRAINT [PK_EmailNotifications] PRIMARY KEY CLUSTERED ([Id] ASC)
);

