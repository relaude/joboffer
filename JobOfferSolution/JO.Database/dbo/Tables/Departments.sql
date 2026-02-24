CREATE TABLE [dbo].[Departments] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [CreatedAt]      DATETIME       NULL,
    [DepartmentName] NVARCHAR (100) NULL,
    [Division_Id]    INT            NULL,
    CONSTRAINT [PK_Departments] PRIMARY KEY CLUSTERED ([Id] ASC)
);

