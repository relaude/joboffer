CREATE TABLE [dbo].[CompBenItems] (
    [Id]         INT             IDENTITY (1, 1) NOT NULL,
    [PlanId]     INT             NULL,
    [CatId]      INT             NULL,
    [ItmName]    NVARCHAR (200)  NULL,
    [ItmDesc]    NVARCHAR (200)  NULL,
    [Amount]     DECIMAL (18, 2) NULL,
    [Multiplier] DECIMAL (18, 2) NULL,
    CONSTRAINT [PK_CompBenItems] PRIMARY KEY CLUSTERED ([Id] ASC)
);

