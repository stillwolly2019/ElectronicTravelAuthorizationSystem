CREATE TABLE [TEC].[TECExpenditure] (
    [TECExpenditureID]          UNIQUEIDENTIFIER CONSTRAINT [DF_TECExpenditure_TECExpenditureID] DEFAULT (newid()) NOT NULL,
    [TravelAuthorizationNumber] NVARCHAR (14)    NULL,
    [ExpenditureDate]           DATE             NULL,
    [ExpenditureDetails]        NVARCHAR (500)   NULL,
    [CurrencyID]                UNIQUEIDENTIFIER NULL,
    [ExpenseAmount]             FLOAT (53)       NULL,
    [Rate]                      FLOAT (53)       NULL,
    [RateAmount]                FLOAT (53)       NULL,
    [LocalAmount]               FLOAT (53)       NULL,
    [CreatedDate]               DATETIME         CONSTRAINT [DF_TECExpenditure_CreatedDate] DEFAULT (getdate()) NULL,
    [CreatedBy]                 UNIQUEIDENTIFIER NULL,
    [UpdatedDate]               DATETIME         NULL,
    [UpdatedBy]                 UNIQUEIDENTIFIER NULL,
    [isDeleted]                 BIT              CONSTRAINT [DF_TECExpenditure_isDeleted] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_TECExpenditure] PRIMARY KEY CLUSTERED ([TECExpenditureID] ASC) WITH (FILLFACTOR = 80)
);



