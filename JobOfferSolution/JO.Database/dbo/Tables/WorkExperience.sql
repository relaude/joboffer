CREATE TABLE [dbo].[WorkExperience] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Description] NVARCHAR (100) NULL,
    CONSTRAINT [PK_WorkExperience] PRIMARY KEY CLUSTERED ([Id] ASC)
);

