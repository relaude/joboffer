CREATE TABLE [dbo].[DiscussSteps] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [StepName]     NVARCHAR (150) NULL,
    [Icon]         NVARCHAR (150) NULL,
    [DisplayOrder] INT            NULL,
    CONSTRAINT [PK_DiscussionSteps] PRIMARY KEY CLUSTERED ([Id] ASC)
);

