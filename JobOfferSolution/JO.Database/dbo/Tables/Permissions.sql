CREATE TABLE [dbo].[Permissions] (
    [Id]             INT           IDENTITY (1, 1) NOT NULL,
    [PermissionCode] NVARCHAR (50) NULL,
    [PermissionName] NVARCHAR (50) NULL,
    CONSTRAINT [PK_Permissions] PRIMARY KEY CLUSTERED ([Id] ASC)
);

