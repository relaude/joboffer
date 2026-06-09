CREATE TABLE [dbo].[JobPositionGrades] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [PositionGrade] NVARCHAR (100) NULL,
    CONSTRAINT [PK_JobPositionGrades] PRIMARY KEY CLUSTERED ([Id] ASC)
);

