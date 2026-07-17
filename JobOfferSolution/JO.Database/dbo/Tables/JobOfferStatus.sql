CREATE TABLE [dbo].[JobOfferStatus] (
    [Id]             INT           IDENTITY (1, 1) NOT NULL,
    [StatusName]     NVARCHAR (50) NULL,
    [BootstrapClass] NVARCHAR (50) NULL,
    [DisplayOrder]   INT           NULL,
    CONSTRAINT [PK_MainStatus] PRIMARY KEY CLUSTERED ([Id] ASC)
);

