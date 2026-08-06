CREATE TABLE [dbo].[CompBenMRI] (
    [Id]      INT            IDENTITY (1, 1) NOT NULL,
    [MRIName] NVARCHAR (200) NULL,
    CONSTRAINT [PK_CompBenMRI_1] PRIMARY KEY CLUSTERED ([Id] ASC)
);

