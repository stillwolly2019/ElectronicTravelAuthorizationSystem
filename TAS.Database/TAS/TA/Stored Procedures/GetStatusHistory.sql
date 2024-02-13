-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [TA].[GetStatusHistory]
	@TravelAuthorizationID nvarchar(max)


AS
BEGIN
	
	select    
	TravelAuthorizationID ,
	s.Description as [Status],
	case when RejectionReasons is null or RejectionReasons= '' then '------'  else  RejectionReasons end RejectionReasons ,
	UPPER(u.FirstName)  +' '+ UPPER(u.LastName) as EmployeeName,
	h.CreatedDate  as CreatedDate
	
	from TA.StatusChangeHistory h

	INNER JOIN lkp.Lookups s on s.Code=h.StatusCode
	INNER JOIN sec.Users u on u.UserID=h.CreatedBy

	where TravelAuthorizationID=@TravelAuthorizationID 
	AND s.IsDeleted = 0
	ORDER BY h.CreatedDate DESC

END
