CREATE TABLE [dbo].[JobLevels] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [JobLevelName] NVARCHAR (100) NULL,
    CONSTRAINT [PK_JobLevels] PRIMARY KEY CLUSTERED ([Id] ASC)
);

