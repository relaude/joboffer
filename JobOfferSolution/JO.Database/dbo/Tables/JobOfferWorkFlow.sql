CREATE TABLE [dbo].[JobOfferWorkFlow] (
    [Id]                     INT IDENTITY (1, 1) NOT NULL,
    [CandidateApplicationId] INT NULL,
    [WorkFlowStatusId]       INT NULL,
    [IsDone]                 BIT NULL,
    CONSTRAINT [PK_JobOfferWorkFlow] PRIMARY KEY CLUSTERED ([Id] ASC)
);

