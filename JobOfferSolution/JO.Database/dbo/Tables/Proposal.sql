CREATE TABLE [dbo].[Proposal] (
    [Id]            INT             IDENTITY (1, 1) NOT NULL,
    [JobOfferId]    INT             NULL,
    [SalaryBandId]  INT             NULL,
    [OptionNum]     INT             NULL,
    [CurrentSalary] DECIMAL (18, 2) NULL,
    [ProposeSalary] DECIMAL (18, 2) NULL,
    [CompaRatio]    DECIMAL (18, 2) NULL,
    [Increase]      DECIMAL (18, 2) NULL,
    [Annual]        DECIMAL (18, 2) NULL,
    [PackageId]     INT             NULL,
    [Recommend]     BIT             NULL,
    [StatusId]      INT             NULL,
    [Escalate]      BIT             NULL,
    [CreatedAt]     DATETIME        NULL,
    [CreatedBy]     INT             NULL,
    CONSTRAINT [PK_Proposal] PRIMARY KEY CLUSTERED ([Id] ASC)
);

