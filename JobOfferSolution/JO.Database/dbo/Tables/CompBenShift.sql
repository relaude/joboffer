CREATE TABLE [dbo].[CompBenShift] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [ShiftName] NVARCHAR (200) NULL,
    CONSTRAINT [PK_CompBenShift] PRIMARY KEY CLUSTERED ([Id] ASC)
);

