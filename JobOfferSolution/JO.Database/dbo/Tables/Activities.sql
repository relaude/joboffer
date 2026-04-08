CREATE TABLE [dbo].[Activities] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [ActionName] NVARCHAR (100) NOT NULL,
    CONSTRAINT [PK_Activities] PRIMARY KEY CLUSTERED ([Id] ASC)
);

