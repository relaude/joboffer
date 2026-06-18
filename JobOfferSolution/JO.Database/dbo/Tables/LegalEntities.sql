CREATE TABLE [dbo].[LegalEntities] (
    [Id]              INT           IDENTITY (1, 1) NOT NULL,
    [CompanyId]       INT           NULL,
    [LegalEntityCode] NVARCHAR (50) NULL,
    [LegalEntityName] NVARCHAR (50) NULL,
    CONSTRAINT [PK_LegalEntities] PRIMARY KEY CLUSTERED ([Id] ASC)
);

