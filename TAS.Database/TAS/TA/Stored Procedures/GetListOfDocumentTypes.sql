CREATE PROCEDURE [TA].[GetListOfDocumentTypes]
	@Scope nvarchar(10) = null
AS
	SELECT [DocumentTypeID], [Code], [Description], [Scope], [IsRequired], [IsDeleted], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy] 
	FROM TA.DocumentTypes
	WHERE 
	(@Scope IS NULL OR Scope = @Scope) AND
	IsDeleted = 0
	ORDER BY IsRequired