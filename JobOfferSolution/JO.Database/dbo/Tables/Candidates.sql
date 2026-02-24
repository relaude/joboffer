CREATE TABLE [dbo].[Candidates] (
    [Id]                  INT             IDENTITY (1, 1) NOT NULL,
    [FirstName]           NVARCHAR (100)  NULL,
    [LastName]            NVARCHAR (100)  NULL,
    [Email]               NVARCHAR (100)  NULL,
    [ContactNumber]       NVARCHAR (100)  NULL,
    [PositionAppliedFor]  NVARCHAR (100)  NULL,
    [LastPositionHeld]    NVARCHAR (100)  NULL,
    [ExpectedSalary]      DECIMAL (18, 2) NULL,
    [CurrentEmployer]     NVARCHAR (100)  NULL,
    [EmploymentStatus_Id] INT             NULL,
    [WorkExperience_Id]   INT             NULL,
    [IsHROD]              BIT             NULL,
    [IsActive]            BIT             CONSTRAINT [DF_Candidates_IsActive] DEFAULT ((1)) NULL,
    [CreatedAt]           DATETIME        NULL,
    CONSTRAINT [PK_Candidates] PRIMARY KEY CLUSTERED ([Id] ASC)
);

