CREATE TABLE [dbo].[MainStatus] (
    [Id]         INT           IDENTITY (1, 1) NOT NULL,
    [StatusName] NVARCHAR (50) NULL,
    CONSTRAINT [PK_MainStatus] PRIMARY KEY CLUSTERED ([Id] ASC)
);

