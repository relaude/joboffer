CREATE TABLE [dbo].[UserPermissions] (
    [Id]             INT IDENTITY (1, 1) NOT NULL,
    [JobOfferUserId] INT NULL,
    [PermissionId]   INT NULL,
    CONSTRAINT [PK_UserPermissions] PRIMARY KEY CLUSTERED ([Id] ASC)
);

