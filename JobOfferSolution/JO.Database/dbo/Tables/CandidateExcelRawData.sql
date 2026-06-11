CREATE TABLE [dbo].[CandidateExcelRawData] (
    [Id]                 INT             IDENTITY (1, 1) NOT NULL,
    [FirstName]          NVARCHAR (100)  NULL,
    [MiddleName]         NVARCHAR (100)  NULL,
    [LastName]           NVARCHAR (100)  NULL,
    [Email]              NVARCHAR (100)  NULL,
    [ContactNumber]      NVARCHAR (100)  NULL,
    [PositionAppliedFor] NVARCHAR (100)  NULL,
    [ExpectedSalary]     DECIMAL (18, 2) NULL,
    [MassUploadLogId]    INT             NULL,
    [CreatedBy]          INT             NULL,
    [CreatedAt]          DATETIME        NULL,
    CONSTRAINT [PK_CandidateExcelRawData] PRIMARY KEY CLUSTERED ([Id] ASC)
);

