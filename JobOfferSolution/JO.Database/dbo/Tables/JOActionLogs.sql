CREATE TABLE [dbo].[JOActionLogs] (
    [Id]         INT      IDENTITY (1, 1) NOT NULL,
    [JobOfferId] INT      NULL,
    [RoleId]     INT      NULL,
    [ActionId]   INT      NULL,
    [ActionAt]   DATETIME NULL,
    [ActionBy]   INT      NULL,
    CONSTRAINT [PK_JOActionLogs] PRIMARY KEY CLUSTERED ([Id] ASC)
);

