CREATE TABLE [TA].[WBS] (
    [WBSID]                 UNIQUEIDENTIFIER CONSTRAINT [DF_WBS_WBSID] DEFAULT (newid()) NOT NULL,
    [TravelAuthorizationID] UNIQUEIDENTIFIER NULL,
    [WBSCode]               NVARCHAR (150)   NULL,
    [PercentageOrAmount]    FLOAT (53)       NULL,
    [IsPercentage]          BIT              NULL,
    [Note]                  NVARCHAR (500)   NULL,
    [CreatedDate]           DATETIME         CONSTRAINT [DF_WBS_CreatedDate] DEFAULT (getdate()) NULL,
    [CreatedBy]             UNIQUEIDENTIFIER NULL,
    [UpdatedDate]           DATETIME         NULL,
    [UpdatedBy]             UNIQUEIDENTIFIER NULL,
    [isDeleted]             BIT              CONSTRAINT [DF_WBS_isDeleted] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_WBS] PRIMARY KEY CLUSTERED ([WBSID] ASC) WITH (FILLFACTOR = 80)
);



