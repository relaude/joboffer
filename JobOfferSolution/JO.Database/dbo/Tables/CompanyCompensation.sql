CREATE TABLE [dbo].[CompanyCompensation] (
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    [CompanyId]        INT            NULL,
    [CmpnyCmpnstnName] NVARCHAR (200) NULL,
    [IsActive]         BIT            NULL,
    [CreatedAt]        DATETIME       NULL,
    [CreatedBy]        INT            NULL,
    [ModifiedAt]       DATETIME       NULL,
    [ModifiedBy]       INT            NULL,
    CONSTRAINT [PK_CompanyCompensation] PRIMARY KEY CLUSTERED ([Id] ASC)
);

