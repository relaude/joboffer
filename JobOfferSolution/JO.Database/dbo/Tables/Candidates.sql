CREATE TABLE [dbo].[Candidates] (
    [Id]        INT           IDENTITY (1, 1) NOT NULL,
    [Name]      NVARCHAR (50) NULL,
    [Email]     NVARCHAR (50) NULL,
    [CreatedAt] DATETIME      NULL,
    CONSTRAINT [PK_Candidates] PRIMARY KEY CLUSTERED ([Id] ASC)
);

