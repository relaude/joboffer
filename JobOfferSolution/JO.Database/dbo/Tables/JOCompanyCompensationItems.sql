CREATE TABLE [dbo].[JOCompanyCompensationItems] (
    [Id]               INT          IDENTITY (1, 1) NOT NULL,
    [JobOfferId]       INT          NULL,
    [JOCmpnyCmpnstnId] INT          NULL,
    [ItemId]           INT          NULL,
    [MonthlyAmount]    DECIMAL (18) NULL,
    [AnnualAmount]     DECIMAL (18) NULL,
    [IsAnalysis]       BIT          NULL,
    [IsEditable]       BIT          NULL,
    CONSTRAINT [PK_JOCmpnyCompensationItems] PRIMARY KEY CLUSTERED ([Id] ASC)
);

