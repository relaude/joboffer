CREATE TABLE [dbo].[CandApplStatus] (
    [Id]             INT           IDENTITY (1, 1) NOT NULL,
    [StatusName]     NVARCHAR (50) NULL,
    [BootstrapClass] NVARCHAR (50) NULL,
    CONSTRAINT [PK_CandApplStatus] PRIMARY KEY CLUSTERED ([Id] ASC)
);

