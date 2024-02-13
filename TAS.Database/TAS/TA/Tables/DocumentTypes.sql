CREATE TABLE [TA].[DocumentTypes] (
    [DocumentTypeID] UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [Code]           NVARCHAR (10)    NOT NULL,
    [Description]    NVARCHAR (50)    NULL,
    [Scope]          NVARCHAR (10)    NULL,
    [IsRequired]     BIT              NULL,
    [IsDeleted]      BIT              NULL,
    [CreatedDate]    DATETIME         DEFAULT (getdate()) NULL,
    [CreatedBy]      UNIQUEIDENTIFIER NULL,
    [UpdatedDate]    DATETIME         NULL,
    [UpdatedBy]      UNIQUEIDENTIFIER NULL,
    PRIMARY KEY CLUSTERED ([DocumentTypeID] ASC)
);

