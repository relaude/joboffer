CREATE TABLE [dbo].[CompanyCompensationItems] (
    [Id]             INT             IDENTITY (1, 1) NOT NULL,
    [CmpnyCmpnstnId] INT             NULL,
    [ItemId]         INT             NULL,
    [MonthlyAmount]  DECIMAL (18, 2) NULL,
    [AnnualAmount]   DECIMAL (18, 2) NULL,
    [IsAnalysis]     BIT             NULL,
    [IsEditable]     BIT             NULL,
    CONSTRAINT [PK_CompanyCompensationItems] PRIMARY KEY CLUSTERED ([Id] ASC)
);

