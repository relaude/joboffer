CREATE TABLE [dbo].[DeclineReasons] (
    [Id]     INT           IDENTITY (1, 1) NOT NULL,
    [Reason] NVARCHAR (50) NULL,
    CONSTRAINT [PK_DeclineReasons] PRIMARY KEY CLUSTERED ([Id] ASC)
);

