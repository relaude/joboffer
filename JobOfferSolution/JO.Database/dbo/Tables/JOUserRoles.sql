CREATE TABLE [dbo].[JOUserRoles] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [AspNetRoleId]   NVARCHAR (450) NULL,
    [RoleCategoryId] INT            NULL,
    [OrderBy]        INT            NULL,
    CONSTRAINT [PK_JOUserRoles] PRIMARY KEY CLUSTERED ([Id] ASC)
);

