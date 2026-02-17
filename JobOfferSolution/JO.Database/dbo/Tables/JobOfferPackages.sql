CREATE TABLE [dbo].[JobOfferPackages] (
    [Id]             INT             IDENTITY (1, 1) NOT NULL,
    [Transaction_Id] INT             NULL,
    [PackageName]    NVARCHAR (50)   NULL,
    [PackageAmount]  DECIMAL (18, 2) NULL,
    [Priority]       INT             NULL,
    [CreatedAt]      DATETIME        NULL,
    CONSTRAINT [PK_JobOfferPackages_1] PRIMARY KEY CLUSTERED ([Id] ASC)
);

