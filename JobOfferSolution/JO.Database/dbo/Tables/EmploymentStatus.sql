CREATE TABLE [dbo].[EmploymentStatus] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [StatusName] NVARCHAR (100) NULL,
    CONSTRAINT [PK_EmploymentStatus] PRIMARY KEY CLUSTERED ([Id] ASC)
);

