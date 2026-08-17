CREATE TABLE [dbo].[CompensationOptions] (
    [Id]            INT             IDENTITY (1, 1) NOT NULL,
    [PackageId]     INT             NULL,
    [ItemId]        INT             NULL,
    [MonthlyAmount] DECIMAL (18, 2) NULL,
    [AnnualAmount]  DECIMAL (18, 2) NULL,
    CONSTRAINT [PK_CompensationOptions] PRIMARY KEY CLUSTERED ([Id] ASC)
);

