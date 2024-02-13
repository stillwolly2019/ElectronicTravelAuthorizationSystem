CREATE PROCEDURE [TA].[GetStaffTravelAuthorization_old]
@UserID nvarchar(100),
@TravelAuthorizationNumber nvarchar(14),
@CreatedDate date
AS
BEGIN
declare @UserName nvarchar(50)
select @UserName=username from sec.Users where userID=@UserID
 and IsDeleted=0

declare @UserIDOLD  nvarchar(50) 

select @UserIDOLD=UserID from sec.Users where Username='as_'+@UserName
 and IsDeleted=0 


IF @CreatedDate = CONVERT(DATE,'1900-01-01')
BEGIN
	SELECT	TravelAuthorizationID, TravelAuthorizationNumber, UserID,  UPPER (TravelersName) as TravelersName , PurposeOfTravel, 
			TripSchemaCode, ModeOfTravelCode, SecurityClearance, SecurityTraining, REPLACE(CONVERT(nvarchar(11), CreatedDate,106),' ','-') AS CreatedDate, 
			CreatedBy, UpdatedDate, UpdatedBy, isDeleted ,status.Description as StatusCode,travel.FromLocationDate,travel.ToLocationDate
	FROM TA.TravelAuthorization  t

	cross apply
				(
				
				SELECT TOP 1 s.Description,statusCode  FROM TA.StatusChangeHistory 
				inner join lkp.Lookups s on s.Code=TA.StatusChangeHistory.StatusCode
				WHERE TravelAuthorizationID =t.[TravelAuthorizationID] 
				 ORDER BY CreatedDate DESC  
				)status
				cross apply(
				select min(FromLocationDate) as  FromLocationDate , max(ToLocationDate) as  ToLocationDate from Ta.TravelItinerary 
				where TravelAuthorizationNumber=t.[TravelAuthorizationNumber]  

				group by  TravelAuthorizationNumber )travel

	WHERE 
		 
		UserID in ( 
		@UserID,@UserIDOLD
		)  
		AND 
		 isDeleted = 0 
		AND TravelAuthorizationNumber LIKE '%' + @TravelAuthorizationNumber + '%'
	ORDER BY CreatedDate DESC
END
ELSE
BEGIN
	SELECT	TravelAuthorizationID, TravelAuthorizationNumber, UserID, UPPER (TravelersName) as TravelersName, PurposeOfTravel, 
			TripSchemaCode, ModeOfTravelCode, SecurityClearance, SecurityTraining, REPLACE(CONVERT(nvarchar(11), CreatedDate,106),' ','-') AS CreatedDate, 
			CreatedBy, UpdatedDate, UpdatedBy, isDeleted ,status.Description as StatusCode,travel.FromLocationDate,travel.ToLocationDate
	FROM TA.TravelAuthorization  t

	cross apply
				(
				
				SELECT TOP 1 s.Description,statusCode  FROM TA.StatusChangeHistory 
				inner join lkp.Lookups s on s.Code=TA.StatusChangeHistory.StatusCode
				WHERE TravelAuthorizationID =t.[TravelAuthorizationID] 
				 ORDER BY CreatedDate DESC  
				)status
				cross apply(
				select min(FromLocationDate) as  FromLocationDate , max(ToLocationDate) as  ToLocationDate from Ta.TravelItinerary 
				where TravelAuthorizationNumber=t.[TravelAuthorizationNumber]  

				group by  TravelAuthorizationNumber )travel

	WHERE 
		 
		UserID in ( 
		@UserID,@UserIDOLD
		)  
		AND isDeleted = 0 
		AND TravelAuthorizationNumber LIKE '%' + @TravelAuthorizationNumber + '%'
		AND CONVERT(DATE,CreatedDate,103) = CONVERT(DATE,@CreatedDate,103)
	ORDER BY CreatedDate DESC
END
END
