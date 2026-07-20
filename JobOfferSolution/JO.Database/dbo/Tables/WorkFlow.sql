CREATE TABLE [dbo].[WorkFlow] (
    [Id]         INT IDENTITY (1, 1) NOT NULL,
    [JobOfferId] INT NULL,
    [StatusId]   INT NULL,
    [ActionId]   INT NULL,
    CONSTRAINT [PK_WorkFlow] PRIMARY KEY CLUSTERED ([Id] ASC)
);

