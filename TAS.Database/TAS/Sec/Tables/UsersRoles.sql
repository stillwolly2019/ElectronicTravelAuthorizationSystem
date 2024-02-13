CREATE TABLE [Sec].[UsersRoles] (
    [UsersRolesID] UNIQUEIDENTIFIER CONSTRAINT [DF_UsersRoles_UsersRolesID] DEFAULT (newid()) NOT NULL,
    [UserID]       UNIQUEIDENTIFIER NULL,
    [RoleID]       UNIQUEIDENTIFIER NULL,
    [DateCreated]  SMALLDATETIME    CONSTRAINT [DF_UsersRoles_DateCreated] DEFAULT (getdate()) NULL,
    [CreatedBy]    UNIQUEIDENTIFIER NULL,
    [DateModified] SMALLDATETIME    CONSTRAINT [DF_UsersRoles_DateModified] DEFAULT ('1900-01-01') NULL,
    [ModifiedBy]   UNIQUEIDENTIFIER NULL,
    [IsDeleted]    BIT              CONSTRAINT [DF_UsersRoles_IsDeleted] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_UsersRoles_1] PRIMARY KEY CLUSTERED ([UsersRolesID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_UsersRoles_Roles] FOREIGN KEY ([RoleID]) REFERENCES [Sec].[Roles] ([RoleID]),
    CONSTRAINT [FK_UsersRoles_Users] FOREIGN KEY ([UserID]) REFERENCES [Sec].[Users] ([UserID])
);



