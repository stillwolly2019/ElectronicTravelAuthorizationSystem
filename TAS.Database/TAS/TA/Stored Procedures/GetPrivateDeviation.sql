-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [TA].[GetPrivateDeviation] 
	@TravelAuthorizationNumber nvarchar(14)
AS
BEGIN
	
--	  select  Cast(FromLocationCode as nvarchar(max))+';'+CAST(Ordering as nvarchar(20))+';'+CAST(ToLocationCode as nvarchar(max)) as RowId,TravelAuthorizationNumber,TravelItineraryID,FromLocationDate,ToLocationDate ,ToLocationCode, Ordering,
--CONVERT(varchar, FromLocationDate, 106) + '-' +CONVERT(varchar, ToLocationDate, 106) as Date from TA.PrivateDeviation  pd
--  cross Apply(
--  select 
--  FromLocationDate ,ToLocationCode, Ordering,ToLocationDate,FromLocationCode from ta.TravelItinerary 
--  where TravelItineraryID=pd.TravelItineraryID
--  ) TI
--  where TravelAuthorizationNumber=@TravelAuthorizationNumber and isDeleted=0



  select
  
   Cast(FromLocationCode as nvarchar(max))+';'+CAST(Ordering as nvarchar(20))+';'+CAST(ToLocationCode as nvarchar(max)) as RowId,
ti.TravelAuthorizationNumber,ti.TravelItineraryID,FromLocationDate,ToLocationDate ,ToLocationCode, Ordering,
CONVERT(varchar, FromLocationDate, 106) + '-' +CONVERT(varchar, ToLocationDate, 106) as Date  ,

case when PrivateDeviationId is null then 0 else 1 end IsSelected,Pd.isDeleted
   
   from Ta.TravelItinerary  ti
  
  left join ta.PrivateDeviation pd on pd.TravelItineraryId=ti.TravelItineraryID
   where ti.TravelAuthorizationNumber=@travelAuthorizationNumber and ti.isDeleted=0 --and Pd.isDeleted=0

    
order by ti.Ordering asc

END
