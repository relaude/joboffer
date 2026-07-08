CREATE TABLE [dbo].[CompensationBenefitType] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [BenefitName] NVARCHAR (50) NULL,
    CONSTRAINT [PK_CompensationBenefitType] PRIMARY KEY CLUSTERED ([Id] ASC)
);

