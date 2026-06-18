CREATE TABLE [dbo].[UserApprovalLimits] (
    [Id]             INT             IDENTITY (1, 1) NOT NULL,
    [JobOfferUserId] INT             NULL,
    [MinSalary]      DECIMAL (18, 2) NULL,
    [MaxSalary]      DECIMAL (18, 2) NULL,
    CONSTRAINT [PK_UserApprovalLimits] PRIMARY KEY CLUSTERED ([Id] ASC)
);

