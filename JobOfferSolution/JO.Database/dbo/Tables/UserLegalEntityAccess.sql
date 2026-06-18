CREATE TABLE [dbo].[UserLegalEntityAccess] (
    [Id]             INT NOT NULL,
    [JobOfferUserId] INT NULL,
    [LegalEntityId]  INT NULL,
    CONSTRAINT [PK_UserLegalEntityAccess] PRIMARY KEY CLUSTERED ([Id] ASC)
);

