CREATE TABLE [dbo].[Approvals] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [JobOfferId] INT            NULL,
    [ProposalId] INT            NULL,
    [TypeId]     INT            NULL,
    [StatusId]   INT            NULL,
    [Comments]   NVARCHAR (500) NULL,
    [ApproveAt]  DATETIME       NULL,
    [ApproveBy]  INT            NULL,
    CONSTRAINT [PK_Approvals] PRIMARY KEY CLUSTERED ([Id] ASC)
);

