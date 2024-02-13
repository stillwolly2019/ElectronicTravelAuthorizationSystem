CREATE TABLE [Lkp].[LookupsGroups] (
    [LookupGroupID] UNIQUEIDENTIFIER CONSTRAINT [DF_LookupsGroups_ID] DEFAULT (newid()) NOT NULL,
    [LookupGroup]   NVARCHAR (100)   NULL,
    [DateCreated]   SMALLDATETIME    CONSTRAINT [DF_LookupsGroups_DateCreated] DEFAULT (getdate()) NULL,
    [CreatedBy]     UNIQUEIDENTIFIER NULL,
    [DateModified]  SMALLDATETIME    CONSTRAINT [DF_LookupsGroups_DateModified] DEFAULT ('1900-01-01') NULL,
    [ModifiedBy]    UNIQUEIDENTIFIER NULL,
    [IsDeleted]     BIT              CONSTRAINT [DF_LookupsGroups_IsDeleted] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_LookupsGroups] PRIMARY KEY CLUSTERED ([LookupGroupID] ASC) WITH (FILLFACTOR = 80)
);



