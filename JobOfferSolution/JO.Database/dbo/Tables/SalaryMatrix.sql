CREATE TABLE [dbo].[SalaryMatrix] (
    [Id]         INT      IDENTITY (1, 1) NOT NULL,
    [CompanyId]  INT      NULL,
    [IsActive]   BIT      NULL,
    [CreatedAt]  DATETIME NULL,
    [CreatedBy]  INT      NULL,
    [ModifiedAt] DATETIME NULL,
    [ModifiedBy] INT      NULL,
    CONSTRAINT [PK_SalaryMatrix] PRIMARY KEY CLUSTERED ([Id] ASC)
);

