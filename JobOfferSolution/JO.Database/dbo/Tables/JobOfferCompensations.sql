CREATE TABLE [dbo].[JobOfferCompensations] (
    [Id]                      INT             IDENTITY (1, 1) NOT NULL,
    [JobOfferId]              INT             NULL,
    [MonthlySalary]           DECIMAL (18, 2) NULL,
    [AnnualSalary]            DECIMAL (18, 2) NULL,
    [RiceAllowance]           DECIMAL (18, 2) NULL,
    [TransportationAllowance] DECIMAL (18, 2) NULL,
    [CommunicationAllowance]  DECIMAL (18, 2) NULL,
    [ClothingAllowance]       DECIMAL (18, 2) NULL,
    [SigningBonus]            DECIMAL (18, 2) NULL,
    [Remarks]                 NVARCHAR (1000) NULL,
    CONSTRAINT [PK_JobOfferCompensations] PRIMARY KEY CLUSTERED ([Id] ASC)
);

