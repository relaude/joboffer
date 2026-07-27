CREATE TABLE [dbo].[ApproverTypes] (
    [Id]       INT           IDENTITY (1, 1) NOT NULL,
    [TypeName] NVARCHAR (50) NULL,
    CONSTRAINT [PK_ApproverTypes] PRIMARY KEY CLUSTERED ([Id] ASC)
);

