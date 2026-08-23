CREATE TABLE [dbo].[OfferRange] (
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    [RangeName]        NVARCHAR (200) NULL,
    [RangeDescription] NVARCHAR (200) NULL,
    CONSTRAINT [PK_OfferRange] PRIMARY KEY CLUSTERED ([Id] ASC)
);

