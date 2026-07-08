CREATE TABLE [dbo].[CompensationBenefits] (
    [Id]           INT             IDENTITY (1, 1) NOT NULL,
    [PackageId]    INT             NULL,
    [ItemName]     NVARCHAR (50)   NULL,
    [Description]  NVARCHAR (150)  NULL,
    [TypeId]       INT             NULL,
    [Amount]       DECIMAL (18, 2) NULL,
    [CurrencyId]   INT             NULL,
    [IsTaxable]    BIT             NULL,
    [Tax]          DECIMAL (18, 2) NULL,
    [IsRecurring]  BIT             NULL,
    [FrequencyId]  INT             NULL,
    [DisplayOrder] INT             NULL,
    [IsActive]     BIT             NULL,
    [CreatedBy]    INT             NULL,
    [CreatedAt]    DATETIME        NULL,
    [ModifiedBy]   INT             NULL,
    [ModifiedAt]   DATETIME        NULL,
    CONSTRAINT [PK_CompensationBenefits] PRIMARY KEY CLUSTERED ([Id] ASC)
);

