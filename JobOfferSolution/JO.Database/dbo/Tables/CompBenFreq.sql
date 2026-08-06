CREATE TABLE [dbo].[CompBenFreq] (
    [Id]       INT            IDENTITY (1, 1) NOT NULL,
    [FreqName] NVARCHAR (200) NULL,
    CONSTRAINT [PK_CompBenFreq] PRIMARY KEY CLUSTERED ([Id] ASC)
);

