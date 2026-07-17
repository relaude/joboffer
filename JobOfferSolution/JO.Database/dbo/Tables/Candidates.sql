CREATE TABLE [dbo].[Candidates] (
    [Id]          INT             IDENTITY (1, 1) NOT NULL,
    [FName]       NVARCHAR (100)  NULL,
    [MName]       NVARCHAR (100)  NULL,
    [LName]       NVARCHAR (100)  NULL,
    [Email]       NVARCHAR (100)  NULL,
    [Contact]     NVARCHAR (100)  NULL,
    [ApplyFor]    NVARCHAR (100)  NULL,
    [LstPosition] NVARCHAR (100)  NULL,
    [Employer]    NVARCHAR (100)  NULL,
    [Salary]      DECIMAL (18, 2) NULL,
    [Expected]    DECIMAL (18, 2) NULL,
    [CreatedAt]   DATETIME        NULL,
    [CreatedBy]   INT             NULL,
    CONSTRAINT [PK_Candidates] PRIMARY KEY CLUSTERED ([Id] ASC)
);

