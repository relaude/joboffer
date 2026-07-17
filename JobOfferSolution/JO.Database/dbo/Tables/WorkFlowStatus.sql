CREATE TABLE [dbo].[WorkFlowStatus] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [WorkFlowName] NVARCHAR (150) NULL,
    [ShortName]    NVARCHAR (50)  NULL,
    [Icon]         NVARCHAR (50)  NULL,
    [DisplayOrder] INT            NULL,
    CONSTRAINT [PK_WorkFlowStatus] PRIMARY KEY CLUSTERED ([Id] ASC)
);

