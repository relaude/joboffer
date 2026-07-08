CREATE TABLE [dbo].[Frequencies] (
    [Id]            INT           NOT NULL,
    [FrequencyName] NVARCHAR (50) NULL,
    [Multiplier]    INT           NULL,
    CONSTRAINT [PK_Frequencies] PRIMARY KEY CLUSTERED ([Id] ASC)
);

