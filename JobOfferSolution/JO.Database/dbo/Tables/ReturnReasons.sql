CREATE TABLE [dbo].[ReturnReasons] (
    [Id]     INT           IDENTITY (1, 1) NOT NULL,
    [Reason] NVARCHAR (50) NULL,
    CONSTRAINT [PK_ReturnReasons] PRIMARY KEY CLUSTERED ([Id] ASC)
);

