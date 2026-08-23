CREATE TABLE [dbo].[JOCompanyCompensation] (
    [Id]                INT             IDENTITY (1, 1) NOT NULL,
    [JobOfferId]        INT             NULL,
    [CSGId]             INT             NULL,
    [CmpnyCmpnstnId]    INT             NULL,
    [OptionNumber]      INT             NULL,
    [CurrentSalary]     DECIMAL (18, 2) NULL,
    [ProposedSalary]    DECIMAL (18, 2) NULL,
    [Increase]          DECIMAL (18, 2) NULL,
    [TotalMonthly]      DECIMAL (18, 2) NULL,
    [TotalAnnually]     DECIMAL (18, 2) NULL,
    [DiffTotalMonthly]  DECIMAL (18, 2) NULL,
    [DiffTotalAnnually] DECIMAL (18, 2) NULL,
    [BandStatus]        NVARCHAR (200)  NULL,
    [Escalate]          BIT             NULL,
    [OfferRangeId]      INT             NULL,
    [CreatedAt]         DATETIME        NULL,
    [CreatedBy]         INT             NULL,
    [ModifiedAt]        DATETIME        NULL,
    [ModifiedBy]        INT             NULL,
    CONSTRAINT [PK_JOCompanyCompensation] PRIMARY KEY CLUSTERED ([Id] ASC)
);

