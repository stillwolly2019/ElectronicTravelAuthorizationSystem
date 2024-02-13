CREATE TABLE [Sec].[Roles] (
    [RoleID]       UNIQUEIDENTIFIER CONSTRAINT [DF_Roles_RoleID] DEFAULT (newid()) NOT NULL,
    [RoleName]     NVARCHAR (500)   NULL,
    [DateCreated]  SMALLDATETIME    CONSTRAINT [DF_Roles_DateCreated] DEFAULT (getdate()) NULL,
    [CreatedBy]    UNIQUEIDENTIFIER NULL,
    [DateModified] SMALLDATETIME    CONSTRAINT [DF_Roles_DateModified] DEFAULT ('1900-01-01') NULL,
    [ModifiedBy]   UNIQUEIDENTIFIER NULL,
    [IsDeleted]    BIT              CONSTRAINT [DF_Roles_IsDeleted] DEFAULT ((0)) NULL,
    [IsAdmin]      BIT              NULL,
    [IsFinance]    BIT              NULL,
    CONSTRAINT [PK_Roles_1] PRIMARY KEY CLUSTERED ([RoleID] ASC) WITH (FILLFACTOR = 80)
);



