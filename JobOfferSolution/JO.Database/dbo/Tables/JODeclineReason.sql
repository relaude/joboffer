CREATE TABLE [dbo].[JODeclineReason] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [ReasonName] NVARCHAR (200) NULL,
    CONSTRAINT [PK_JODeclineReason] PRIMARY KEY CLUSTERED ([Id] ASC)
);

