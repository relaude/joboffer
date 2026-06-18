CREATE TABLE [dbo].[RolePermissions] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [AspNetRoleId] NVARCHAR (450) NULL,
    [PermissionId] INT            NULL,
    CONSTRAINT [PK_RolePermissions] PRIMARY KEY CLUSTERED ([Id] ASC)
);

