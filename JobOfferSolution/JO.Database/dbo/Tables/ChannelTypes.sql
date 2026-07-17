CREATE TABLE [dbo].[ChannelTypes] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [ChannelName] NVARCHAR (50) NULL,
    CONSTRAINT [PK_ChannelTypes] PRIMARY KEY CLUSTERED ([Id] ASC)
);

