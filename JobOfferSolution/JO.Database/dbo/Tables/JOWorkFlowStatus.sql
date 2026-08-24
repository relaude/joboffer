CREATE TABLE [dbo].[JOWorkFlowStatus] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL,
    [FlowName]     NVARCHAR (50) NULL,
    [DisplayOrder] INT           NULL,
    CONSTRAINT [PK_JOWorkFlowStatus] PRIMARY KEY CLUSTERED ([Id] ASC)
);

