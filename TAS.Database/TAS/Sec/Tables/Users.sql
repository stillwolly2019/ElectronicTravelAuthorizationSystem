CREATE TABLE [Sec].[Users] (
    [UserID]       UNIQUEIDENTIFIER CONSTRAINT [DF_Users_UserID] DEFAULT (newid()) NOT NULL,
    [Username]     NVARCHAR (50)    NOT NULL,
    [Password]     NVARCHAR (500)   NULL,
    [FirstName]    NVARCHAR (50)    NOT NULL,
    [LastName]     NVARCHAR (50)    NOT NULL,
    [Email]        NVARCHAR (500)   NOT NULL,
    [DateCreated]  SMALLDATETIME    CONSTRAINT [DF_Users_DateCreated] DEFAULT (getdate()) NULL,
    [CreatedBy]    UNIQUEIDENTIFIER NULL,
    [DateModified] SMALLDATETIME    CONSTRAINT [DF_Users_DateModified] DEFAULT ('1900-01-01') NULL,
    [ModifiedBy]   UNIQUEIDENTIFIER NULL,
    [IsDeleted]    BIT              CONSTRAINT [DF_Users_IsDeleted] DEFAULT ((0)) NULL,
    [IsManager]    BIT              NULL,
    [IsLoggedIn]   BIT              DEFAULT ((1)) NULL,
    CONSTRAINT [PK_Users_1] PRIMARY KEY CLUSTERED ([UserID] ASC) WITH (FILLFACTOR = 80)
);



