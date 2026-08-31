CREATE TABLE [dbo].[JOActionLogs] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [JobOfferId] INT            NULL,
    [RoleId]     INT            NULL,
    [ActionId]   INT            NULL,
    [ActionAt]   DATETIME       NULL,
    [ActionBy]   INT            NULL,
    [Remarks]    NVARCHAR (500) NULL,
    CONSTRAINT [PK_JOActionLogs] PRIMARY KEY CLUSTERED ([Id] ASC)
);

