CREATE TABLE [dbo].[AuthorizationPolicies] (
    [Id]             INT           IDENTITY (1, 1) NOT NULL,
    [PolicyName]     NVARCHAR (50) NULL,
    [PermissionId]   INT           NULL,
    [PermissionCode] NVARCHAR (50) NULL,
    [Expression]     NVARCHAR (50) NULL,
    [IsActive]       BIT           NULL,
    CONSTRAINT [PK_AuthorizationPolicies] PRIMARY KEY CLUSTERED ([Id] ASC)
);

