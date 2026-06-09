CREATE TABLE [dbo].[SalaryMatrix] (
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    [CompanyId]        INT            NULL,
    [DivisionId]       INT            NULL,
    [MatrixName]       NVARCHAR (200) NULL,
    [MatrixCode]       NVARCHAR (50)  NULL,
    [CurrencyId]       INT            NULL,
    [EffectiveFrom]    DATETIME       NULL,
    [EffectiveTo]      DATETIME       NULL,
    [ApprovalStatusId] INT            NULL,
    [IsActive]         BIT            NULL,
    [ApprovedByUserId] INT            NULL,
    [ApprovedAt]       DATETIME       NULL,
    [CreatedAt]        DATETIME       NULL,
    [CreatedBy]        INT            NULL,
    [ModifiedAt]       DATETIME       NULL,
    [ModifiedBy]       INT            NULL,
    CONSTRAINT [PK_SalaryMatrix] PRIMARY KEY CLUSTERED ([Id] ASC)
);

