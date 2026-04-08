CREATE TABLE [dbo].[JobPositions] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL,
    [PositionName] NVARCHAR (50) NULL,
    CONSTRAINT [PK_JobPositions] PRIMARY KEY CLUSTERED ([Id] ASC)
);

