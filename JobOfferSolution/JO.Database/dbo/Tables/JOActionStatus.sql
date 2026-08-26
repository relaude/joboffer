CREATE TABLE [dbo].[JOActionStatus] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [ActionName] NVARCHAR (200) NULL,
    CONSTRAINT [PK_JOActionStatus] PRIMARY KEY CLUSTERED ([Id] ASC)
);

