CREATE TABLE [TA].[WorkflowSteps] (
    [WorkflowStepID]             UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [RoleID]                     UNIQUEIDENTIFIER NULL,
    [Scope]                      NVARCHAR (10)    NULL,
    [ApprovedWorkflowStepID]     UNIQUEIDENTIFIER NULL,
    [RejectionWorkflowStepID]    UNIQUEIDENTIFIER NULL,
    [TATECApprovedStatus]        NVARCHAR (50)    NULL,
    [TATECRejectedStatus]        NVARCHAR (50)    NULL,
    [WorkflowStepOrder]          INT              NULL,
    [IsSignedByRole]             BIT              DEFAULT ((1)) NULL,
    [NextApprovedWorkflowStepID] UNIQUEIDENTIFIER NULL,
    PRIMARY KEY CLUSTERED ([WorkflowStepID] ASC)
);

