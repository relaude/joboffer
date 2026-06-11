CREATE TABLE [dbo].[CandidateTempData] (
    [Id]                 INT             IDENTITY (1, 1) NOT NULL,
    [FirstName]          NVARCHAR (100)  NULL,
    [MiddleName]         NVARCHAR (100)  NULL,
    [LastName]           NVARCHAR (100)  NULL,
    [Email]              NVARCHAR (100)  NULL,
    [ContactNumber]      NVARCHAR (100)  NULL,
    [JobPositionId]      INT             NULL,
    [PositionAppliedFor] NVARCHAR (50)   NULL,
    [ExpectedSalary]     DECIMAL (18, 2) NULL,
    [HasErrors]          BIT             NULL,
    [Errors]             NVARCHAR (MAX)  NULL,
    [MassUploadLogId]    INT             NULL,
    CONSTRAINT [PK_CandidateTempData] PRIMARY KEY CLUSTERED ([Id] ASC)
);

