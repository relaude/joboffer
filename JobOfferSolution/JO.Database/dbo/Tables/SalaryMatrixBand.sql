CREATE TABLE [dbo].[SalaryMatrixBand] (
    [Id]                   INT             IDENTITY (1, 1) NOT NULL,
    [SalaryMatrixId]       INT             NULL,
    [JobLevelId]           INT             NULL,
    [JobFamilyId]          INT             NULL,
    [PositionGradeId]      INT             NULL,
    [BandMinimum]          DECIMAL (18, 2) NULL,
    [BandMidpoint]         DECIMAL (18, 2) NULL,
    [BandMaximum]          DECIMAL (18, 2) NULL,
    [BelowBandFlagEnabled] BIT             NULL,
    [AboveBandFlagEnabled] BIT             NULL,
    [CreatedAt]            DATETIME        NULL,
    [CreatedBy]            INT             NULL,
    [ModifiedAt]           DATETIME        NULL,
    [ModifiedBy]           INT             NULL,
    CONSTRAINT [PK_SalaryMatrixBand] PRIMARY KEY CLUSTERED ([Id] ASC)
);

