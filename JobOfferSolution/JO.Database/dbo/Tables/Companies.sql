CREATE TABLE [dbo].[Companies] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [CompanyCode] NVARCHAR (50)  NULL,
    [CompanyName] NVARCHAR (100) NULL,
    CONSTRAINT [PK_Companies] PRIMARY KEY CLUSTERED ([Id] ASC)
);

