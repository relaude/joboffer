CREATE TABLE [dbo].[CompBenPackages] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [PackageName] NVARCHAR (150) NULL,
    CONSTRAINT [PK_CompBenPackages] PRIMARY KEY CLUSTERED ([Id] ASC)
);

