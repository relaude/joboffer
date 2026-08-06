CREATE TABLE [dbo].[GradeTypes] (
    [Id]       INT           IDENTITY (1, 1) NOT NULL,
    [TypeName] NVARCHAR (50) NULL,
    CONSTRAINT [PK_GradeType] PRIMARY KEY CLUSTERED ([Id] ASC)
);

