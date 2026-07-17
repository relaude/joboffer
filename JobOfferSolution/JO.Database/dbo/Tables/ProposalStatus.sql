CREATE TABLE [dbo].[ProposalStatus] (
    [Id]         INT           IDENTITY (1, 1) NOT NULL,
    [StatusName] NVARCHAR (50) NULL,
    CONSTRAINT [PK_ProposalStatus] PRIMARY KEY CLUSTERED ([Id] ASC)
);

