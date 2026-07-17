CREATE TABLE [dbo].[DiscussionSteps] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [StepName]     NVARCHAR (150) NULL,
    [DisplayOrder] INT            NULL,
    [Icon]         NVARCHAR (150) NULL,
    CONSTRAINT [PK_DiscussionSteps] PRIMARY KEY CLUSTERED ([Id] ASC)
);

