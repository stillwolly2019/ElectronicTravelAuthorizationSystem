-- Procedure

CREATE PROCEDURE [Lkp].[SearchCity]
@Prefix nvarchar(100)
AS
BEGIN

SELECT [CityID], [CityDescription] 
FROM [Lookups].[Lkp].[City]
WHERE IsDeleted = 0 
and [CityDescription] LIKE '%' + @Prefix + '%' 
ORDER BY [CityDescription]

END

