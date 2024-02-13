CREATE TABLE [TEC].[TECAdvances] (
    [TECAdvancesID]             UNIQUEIDENTIFIER CONSTRAINT [DF_TECAdvances_TECAdvancesID] DEFAULT (newid()) NOT NULL,
    [TravelAuthorizationNumber] NVARCHAR (14)    NULL,
    [PayOfficeCode]             NVARCHAR (150)   NULL,
    [DatePaid]                  DATE             NULL,
    [CurrencyID]                UNIQUEIDENTIFIER NULL,
    [AdvanceAmount]             FLOAT (53)       NULL,
    [Rate]                      FLOAT (53)       NULL,
    [RateAmount]                FLOAT (53)       NULL,
    [LocalAmount]               FLOAT (53)       NULL,
    [CreatedDate]               DATETIME         CONSTRAINT [DF_TECAdvances_CreatedDate] DEFAULT (getdate()) NULL,
    [CreatedBy]                 UNIQUEIDENTIFIER NULL,
    [UpdatedDate]               DATETIME         NULL,
    [UpdatedBy]                 UNIQUEIDENTIFIER NULL,
    [isDeleted]                 BIT              CONSTRAINT [DF_TECAdvances_isDeleted] DEFAULT ((0)) NULL,
    [PayOfficeCodeID]           UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_TECAdvances] PRIMARY KEY CLUSTERED ([TECAdvancesID] ASC) WITH (FILLFACTOR = 80)
);



