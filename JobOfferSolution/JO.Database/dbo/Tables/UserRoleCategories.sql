CREATE TABLE [dbo].[UserRoleCategories] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL,
    [CategoryName] NVARCHAR (50) NULL,
    CONSTRAINT [PK_UserRoleCategories] PRIMARY KEY CLUSTERED ([Id] ASC)
);

