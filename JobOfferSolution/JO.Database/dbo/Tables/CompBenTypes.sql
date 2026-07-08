CREATE TABLE [dbo].[CompBenTypes] (
    [Id]       INT           IDENTITY (1, 1) NOT NULL,
    [TypeName] NVARCHAR (50) NULL,
    CONSTRAINT [PK_CompBenTypes] PRIMARY KEY CLUSTERED ([Id] ASC)
);

