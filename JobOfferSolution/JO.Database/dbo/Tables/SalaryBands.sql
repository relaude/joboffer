CREATE TABLE [dbo].[SalaryBands] (
    [Id]       INT             IDENTITY (1, 1) NOT NULL,
    [MatrixId] INT             NULL,
    [CSGId]    INT             NULL,
    [Minimum]  DECIMAL (18, 2) NULL,
    [Midpoint] DECIMAL (18, 2) NULL,
    [Maximum]  DECIMAL (18, 2) NULL,
    CONSTRAINT [PK_SalaryBands] PRIMARY KEY CLUSTERED ([Id] ASC)
);

