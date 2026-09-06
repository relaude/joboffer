CREATE TABLE [dbo].[DiscussionStatus] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [StatusName] NVARCHAR (200) NULL,
    CONSTRAINT [PK_DiscussionStatus] PRIMARY KEY CLUSTERED ([Id] ASC)
);

