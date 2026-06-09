CREATE TABLE [dbo].[SalaryMatrixStatus] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [StatusName] NVARCHAR (100) NULL,
    CONSTRAINT [PK_SalaryMatrixStatus] PRIMARY KEY CLUSTERED ([Id] ASC)
);

