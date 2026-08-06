CREATE TABLE [dbo].[SalaryGrades] (
    [Id]        INT           IDENTITY (1, 1) NOT NULL,
    [TypeId]    INT           NULL,
    [GradeName] NVARCHAR (50) NULL,
    CONSTRAINT [PK_SalaryGrades] PRIMARY KEY CLUSTERED ([Id] ASC)
);

