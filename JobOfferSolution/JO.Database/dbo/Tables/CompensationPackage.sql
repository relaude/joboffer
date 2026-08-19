CREATE TABLE [dbo].[CompensationPackage] (
    [Id]              INT             IDENTITY (1, 1) NOT NULL,
    [JobOfferId]      INT             NULL,
    [PckgTempId]      INT             NULL,
    [OptionType]      NVARCHAR (50)   NULL,
    [OptionNumber]    INT             NULL,
    [MonthlyBasic]    DECIMAL (18, 2) NULL,
    [IncreasePercent] DECIMAL (18, 2) NULL,
    [CreatedAt]       DATETIME        NULL,
    [CreatedBy]       INT             NULL,
    [ModifiedAt]      DATETIME        NULL,
    [ModifiedBy]      INT             NULL,
    CONSTRAINT [PK_CompensationPackage] PRIMARY KEY CLUSTERED ([Id] ASC)
);

