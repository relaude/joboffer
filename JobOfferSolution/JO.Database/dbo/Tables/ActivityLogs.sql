CREATE TABLE [dbo].[ActivityLogs] (
    [Id]          INT      IDENTITY (1, 1) NOT NULL,
    [CreatedAt]   DATETIME NULL,
    [CreatedBy]   INT      NULL,
    [JobOffer_Id] INT      NULL,
    [Activity_Id] INT      NULL,
    CONSTRAINT [PK_ActivityLogs] PRIMARY KEY CLUSTERED ([Id] ASC)
);

