CREATE TABLE [Sec].[RolesPermissions] (
    [RolesPermissionsID] UNIQUEIDENTIFIER CONSTRAINT [DF_RolesPermissions_RolesPermissionsID] DEFAULT (newid()) NOT NULL,
    [PageID]             UNIQUEIDENTIFIER NULL,
    [RoleID]             UNIQUEIDENTIFIER NULL,
    [Permissions]        INT              NULL,
    CONSTRAINT [PK_RolesPermissions_1] PRIMARY KEY CLUSTERED ([RolesPermissionsID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_RolesPermissions_Pages] FOREIGN KEY ([PageID]) REFERENCES [Sec].[Pages] ([PageID]),
    CONSTRAINT [FK_RolesPermissions_Roles] FOREIGN KEY ([RoleID]) REFERENCES [Sec].[Roles] ([RoleID])
);



