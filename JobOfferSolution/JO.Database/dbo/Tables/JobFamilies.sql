CREATE TABLE [dbo].[JobFamilies] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [JobFamilyName] NVARCHAR (100) NULL,
    CONSTRAINT [PK_JobFamilies] PRIMARY KEY CLUSTERED ([Id] ASC)
);

