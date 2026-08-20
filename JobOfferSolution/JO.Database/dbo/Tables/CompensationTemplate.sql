CREATE TABLE [dbo].[CompensationTemplate] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL,
    [TemplateName] NVARCHAR (50) NULL,
    [CreatedAt]    DATETIME      NULL,
    [CreatedBy]    INT           NULL,
    [ModifiedAt]   DATETIME      NULL,
    [ModifiedBy]   INT           NULL
);

