CREATE TABLE [dbo].[Divisions] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [CreatedAt]    DATETIME       NULL,
    [DivisionCode] NVARCHAR (50)  NULL,
    [DivisionName] NVARCHAR (100) NULL,
    [Company_Id]   INT            NULL,
    CONSTRAINT [PK_Divisions] PRIMARY KEY CLUSTERED ([Id] ASC)
);

