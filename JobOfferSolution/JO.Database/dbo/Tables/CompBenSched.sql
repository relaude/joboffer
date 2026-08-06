CREATE TABLE [dbo].[CompBenSched] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [SchedName] NVARCHAR (200) NULL,
    CONSTRAINT [PK_CompBenSched] PRIMARY KEY CLUSTERED ([Id] ASC)
);

