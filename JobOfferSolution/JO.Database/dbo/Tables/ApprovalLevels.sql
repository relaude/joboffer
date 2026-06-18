CREATE TABLE [dbo].[ApprovalLevels] (
    [Id]              INT           IDENTITY (1, 1) NOT NULL,
    [SequenceNo]      INT           NULL,
    [ApprovalRole]    NVARCHAR (50) NULL,
    [IsAboveBandOnly] BIT           NULL,
    CONSTRAINT [PK_ApprovalLevels] PRIMARY KEY CLUSTERED ([Id] ASC)
);

