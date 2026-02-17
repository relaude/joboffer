CREATE TABLE [dbo].[FileTypes] (
    [Id]       INT           IDENTITY (1, 1) NOT NULL,
    [TypeName] NVARCHAR (50) NULL,
    CONSTRAINT [PK_FileTypes] PRIMARY KEY CLUSTERED ([Id] ASC)
);

