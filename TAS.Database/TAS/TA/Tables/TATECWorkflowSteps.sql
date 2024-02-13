CREATE TABLE [TA].[TATECWorkflowSteps] (
    [TATECWorkflowStepsID]  UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [TravelAuthorizationID] UNIQUEIDENTIFIER NULL,
    [WorkflowStepID]        UNIQUEIDENTIFIER NULL,
    [UserID]                UNIQUEIDENTIFIER NULL,
    [RoleID]                UNIQUEIDENTIFIER NULL,
    [TATECStatus]           NVARCHAR (50)    NULL,
    [SignedBy]              UNIQUEIDENTIFIER NULL,
    [SignedDate]            DATETIME         NULL,
    [RejectedBy]            UNIQUEIDENTIFIER NULL,
    [RejectedDate]          DATETIME         NULL,
    [CreatedBy]             UNIQUEIDENTIFIER NULL,
    [CreatedDate]           DATETIME         DEFAULT (getdate()) NULL,
    PRIMARY KEY CLUSTERED ([TATECWorkflowStepsID] ASC)
);

