CREATE TABLE [dbo].[DboxCandidates] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [ResponseId]    INT            NULL,
    [DboxId]        INT            NULL,
    [CSGId]         INT            NULL,
    [CompanyId]     INT            NULL,
    [DivisionId]    INT            NULL,
    [DepartmentId]  INT            NULL,
    [DboxRefNum]    NVARCHAR (50)  NULL,
    [CandidateName] NVARCHAR (500) NULL,
    [Company]       NVARCHAR (500) NULL,
    [Division]      NVARCHAR (500) NULL,
    [Department]    NVARCHAR (500) NULL,
    [CostCenter]    NVARCHAR (500) NULL,
    [JobLevel]      NVARCHAR (500) NULL,
    [JobPosition]   NVARCHAR (500) NULL,
    [EmailAddress]  NVARCHAR (500) NULL,
    [ContactNumber] NVARCHAR (500) NULL,
    [StatusId]      INT            NULL,
    CONSTRAINT [PK_DboxCandidates] PRIMARY KEY CLUSTERED ([Id] ASC)
);







GO


