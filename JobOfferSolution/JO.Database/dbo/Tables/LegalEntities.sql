CREATE TABLE [dbo].[LegalEntities] (
    [Id]            INT IDENTITY (1, 1) NOT NULL,
    [JobOfferId]    INT NULL,
    [CandidateId]   INT NULL,
    [CompanyId]     INT NULL,
    [DivisionId]    INT NULL,
    [MatrixId]      INT NULL,
    [JobPositionId] INT NULL,
    CONSTRAINT [PK_LegalEntities] PRIMARY KEY CLUSTERED ([Id] ASC)
);

