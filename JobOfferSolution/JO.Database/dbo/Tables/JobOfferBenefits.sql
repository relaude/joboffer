CREATE TABLE [dbo].[JobOfferBenefits] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [JobOfferId]   INT            NULL,
    [BenefitId]    INT            NULL,
    [BenefitValue] NVARCHAR (500) NULL,
    CONSTRAINT [PK_JobOfferBenefits] PRIMARY KEY CLUSTERED ([Id] ASC)
);

