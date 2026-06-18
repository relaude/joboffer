CREATE TABLE [dbo].[UserDivisionAccess] (
    [Id]             INT IDENTITY (1, 1) NOT NULL,
    [JobOfferUserId] INT NULL,
    [CompanyId]      INT NULL,
    [DivisionId]     INT NULL,
    CONSTRAINT [PK_UserCompanyAccess] PRIMARY KEY CLUSTERED ([Id] ASC)
);

