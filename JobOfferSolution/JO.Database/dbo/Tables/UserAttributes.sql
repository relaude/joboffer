CREATE TABLE [dbo].[UserAttributes] (
    [Id]             INT           IDENTITY (1, 1) NOT NULL,
    [JobOfferUserId] INT           NULL,
    [AttributeName]  NVARCHAR (50) NULL,
    [AttributeValue] NVARCHAR (50) NULL,
    CONSTRAINT [PK_UserAttributes] PRIMARY KEY CLUSTERED ([Id] ASC)
);

