CREATE TABLE [dbo].[JOApprovalFlow] (
    [Id]         INT IDENTITY (1, 1) NOT NULL,
    [JobOfferId] INT NULL,
    [RoleId]     INT NULL,
    [ActionId]   INT NULL,
    [IsAproved]  BIT NULL,
    CONSTRAINT [PK_JOApprovalFlow] PRIMARY KEY CLUSTERED ([Id] ASC)
);

