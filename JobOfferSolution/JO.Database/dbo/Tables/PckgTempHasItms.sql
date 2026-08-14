CREATE TABLE [dbo].[PckgTempHasItms] (
    [Id]        INT IDENTITY (1, 1) NOT NULL,
    [TempId]    INT NULL,
    [ItemId]    INT NULL,
    [IsEnabled] BIT NULL,
    CONSTRAINT [PK_PckgTempHasItms] PRIMARY KEY CLUSTERED ([Id] ASC)
);

