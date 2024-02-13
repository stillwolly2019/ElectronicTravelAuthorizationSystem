-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [TA].[InsertTAStatus]
	
	@TravelAuthorizationID nvarchar(max),
	@StatusCode nvarchar(max), 
	@RejectionReasons nvarchar(max),
	@RejectionReasonType char(3),
	@CreatedBy nvarchar(max)


AS
BEGIN
    
	DECLARE @LatestStatusCode as nvarchar(10) = '';
	SET @LatestStatusCode = (SELECT TOP 1 StatusCode FROM TA.StatusChangeHistory WHERE TravelAuthorizationID = @TravelAuthorizationID  ORDER BY CreatedDate DESC)

	--Prevent saving current status twice
	IF(@LatestStatusCode <> @StatusCode)
	BEGIN

	insert into TA.StatusChangeHistory
	(TravelAuthorizationID,StatusCode,RejectionReasons,CreatedBy,CreatedDate)
	values
	(@TravelAuthorizationID,@StatusCode,@RejectionReasons,@CreatedBy,getDAte())

	if(@StatusCode='INC')
	BEGIN
	IF((select count(travelAuthorizationID) from ta.RejectionReason where TravelAuthorizationID=@TravelAuthorizationID and isDeleted=0)>0)
	BEGIN 
	update ta.RejectionReason set isDeleted=1 where TravelAuthorizationID=@TravelAuthorizationID AND RejectionReasonType = @RejectionReasonType
	END
	END

	SELECT 1
	END
	ELSE
	BEGIN
	SELECT 0
	END
	

END
