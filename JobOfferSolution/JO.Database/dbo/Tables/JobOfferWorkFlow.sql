CREATE TABLE [dbo].[JobOfferWorkFlow] (
    [Id]                       INT IDENTITY (1, 1) NOT NULL,
    [CandidateMSFormRequestId] INT NULL,
    [CandidateApplicationId]   INT NULL,
    [JobOfferAnalysisId]       INT NULL,
    [WorkFlowStatusId]         INT NULL,
    [WorkFlowActionId]         INT NULL,
    [DisplayOrder]             INT NULL,
    CONSTRAINT [PK_JobOfferWorkFlow] PRIMARY KEY CLUSTERED ([Id] ASC)
);

