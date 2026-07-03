CREATE TABLE [dbo].[WorkFlowStatus] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL,
    [WorkFlowName] NVARCHAR (50) NULL,
    [ShortName]    NVARCHAR (50) NULL,
    [OrderBy]      INT           NULL,
    CONSTRAINT [PK_WorkFlowStatus] PRIMARY KEY CLUSTERED ([Id] ASC)
);

