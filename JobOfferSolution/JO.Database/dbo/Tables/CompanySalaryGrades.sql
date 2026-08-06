CREATE TABLE [dbo].[CompanySalaryGrades] (
    [Id]        INT IDENTITY (1, 1) NOT NULL,
    [CompanyId] INT NULL,
    [GradeId]   INT NULL,
    CONSTRAINT [PK_CompanySalaryGrades] PRIMARY KEY CLUSTERED ([Id] ASC)
);

