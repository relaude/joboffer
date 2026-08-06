CREATE TABLE [dbo].[CompBenEmpType] (
    [Id]       INT            IDENTITY (1, 1) NOT NULL,
    [TypeName] NVARCHAR (200) NULL,
    CONSTRAINT [PK_CompBenEmpType] PRIMARY KEY CLUSTERED ([Id] ASC)
);

