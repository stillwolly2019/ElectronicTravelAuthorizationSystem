

CREATE PROCEDURE [TA].[GetTravelItineraryByTravelAuthorizationNumber]
@TravelAuthorizationNumber nvarchar(14)
AS
BEGIN

select ti.* ,ti.FromLocationCode as FromLocationCodeID, ti.ToLocationCode as ToLocationCodeID  , ti.ModeOfTravelID,
MOT.Description as ModeOfTravelName , FLC.CityDescription AS FromLocationCodeName, TLC.CityDescription as ToLocationCodeName, 
pv.IsPrivateDeviation as isSelected,CONVERT(date,ti.FromLocationDate) FromLocationDate, CONVERT(date,ti.ToLocationDate) AS ToLocationDate
 from [TA].TravelItinerary ti
INNER JOIN [Lkp].[Lookups] MOT on MOT.LookupsID = ti.ModeOfTravelID
INNER JOIN [Lookups].[Lkp].[City] FLC on FLC.CityID = ti.FromLocationCode 
INNER JOIN [Lookups].[Lkp].[City] TLC on TLC.CityID = ti.ToLocationCode 
cross apply(
select   TA.TravelAuthorization.IsPrivateDeviation from TA.TravelAuthorization where TravelAuthorizationNumber=ti.TravelAuthorizationNumber
) pv

where ti.isDeleted = 0 and ti.TravelAuthorizationNumber = @TravelAuthorizationNumber

order by ti.Ordering asc
	
END
