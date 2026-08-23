CREATE TABLE [dbo].[JOUserRoles] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [AspNetRoleId]   NVARCHAR (450) NULL,
    [RoleCategoryId] INT            NULL,
    [IsActive]       BIT            NULL,
    [OrderBy]        INT            NULL,
    [Description]    NVARCHAR (500) NULL,
    CONSTRAINT [PK_JOUserRoles] PRIMARY KEY CLUSTERED ([Id] ASC)
);

