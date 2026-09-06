CREATE TABLE [dbo].[EmailTemplateRoles] (
    [Id]              INT IDENTITY (1, 1) NOT NULL,
    [EmailTemplateId] INT NULL,
    [RoleId]          INT NULL,
    CONSTRAINT [PK_EmailTemplateRoles] PRIMARY KEY CLUSTERED ([Id] ASC)
);

