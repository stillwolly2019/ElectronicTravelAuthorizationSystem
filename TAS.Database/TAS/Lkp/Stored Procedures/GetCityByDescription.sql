-- Procedure

CREATE PROCEDURE [Lkp].[GetCityByDescription]
@CityDescription nvarchar(100)
AS
BEGIN

SELECT *
FROM [Lookups].[Lkp].[City]
WHERE IsDeleted = 0 
and [CityDescription] = @CityDescription
ORDER BY [CityDescription]

END

