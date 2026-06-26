CREATE TABLE [dbo].[Permissions] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [PermissionCode] NVARCHAR (150) NULL,
    [PermissionName] NVARCHAR (150) NULL,
    CONSTRAINT [PK_Permissions] PRIMARY KEY CLUSTERED ([Id] ASC)
);

