CREATE TABLE [dbo].[JobOfferUsers] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [AspNetUser_Id] NVARCHAR (MAX) NULL,
    [Name]          NVARCHAR (50)  NULL,
    [Email]         NVARCHAR (50)  NULL,
    [IsActive]      BIT            CONSTRAINT [DF_JobOfferUsers_IsActive] DEFAULT ((1)) NULL,
    [CreatedAt]     DATETIME       NULL,
    CONSTRAINT [PK_JobOfferUsers] PRIMARY KEY CLUSTERED ([Id] ASC)
);

