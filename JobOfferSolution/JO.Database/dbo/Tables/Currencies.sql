CREATE TABLE [dbo].[Currencies] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [Currency]    CHAR (3)      NULL,
    [Description] NVARCHAR (50) NULL,
    CONSTRAINT [PK_Currencies] PRIMARY KEY CLUSTERED ([Id] ASC)
);

