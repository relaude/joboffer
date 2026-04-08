CREATE TABLE [dbo].[EmailNotifRoleRecipients] (
    [Id]                   INT            IDENTITY (1, 1) NOT NULL,
    [EmailNotification_Id] INT            NULL,
    [Role_Id]              NVARCHAR (450) NULL,
    CONSTRAINT [PK_EmailNotifRoleRecipients] PRIMARY KEY CLUSTERED ([Id] ASC)
);

