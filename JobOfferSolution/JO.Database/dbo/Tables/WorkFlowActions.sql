CREATE TABLE [dbo].[WorkFlowActions] (
    [Id]             INT           IDENTITY (1, 1) NOT NULL,
    [ActionName]     NVARCHAR (50) NULL,
    [BootstrapClass] NVARCHAR (50) NULL,
    CONSTRAINT [PK_WorkFlowActions] PRIMARY KEY CLUSTERED ([Id] ASC)
);

