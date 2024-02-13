CREATE TABLE [TA].[TATECWorkflowStepsRejectionReasons] (
    [TATECWorkflowStepsRejectionReason] UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [TATECWorkflowStepID]               UNIQUEIDENTIFIER NOT NULL,
    [RejectionReasonID]                 UNIQUEIDENTIFIER NOT NULL,
    [IsDeleted]                         BIT              DEFAULT ((0)) NULL,
    [DateCreated]                       DATETIME         DEFAULT (getdate()) NULL,
    [CreatedBy]                         UNIQUEIDENTIFIER NULL,
    PRIMARY KEY CLUSTERED ([TATECWorkflowStepsRejectionReason] ASC)
);

