CREATE TABLE [dbo].[Benefits] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [BenefitName] NVARCHAR (100) NULL,
    [IsActive]    BIT            NULL,
    CONSTRAINT [PK_Benefits] PRIMARY KEY CLUSTERED ([Id] ASC)
);

