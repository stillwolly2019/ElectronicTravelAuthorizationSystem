-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [Rpt].[GetSupervisors]
	--@SupervisorEmail nvarchar(50)
AS
BEGIN



	
	;WITH Supervisor AS (

  SELECT DISTINCT 
s.ManagerEmail,
s.StaffID

FROM [AttendanceMonitoringSystem].Lkp.ReportSubscription s 
),
SupervisorList
AS (
SELECT DISTINCT  a.EmployeeID,a.PersonnelNumber,a.[Name],a.EndDate,
REPLACE( REPLACE (ManagerEmail,'<xx>',''),'</xx>','') as ManagerEmail
    FROM  [AttendanceMonitoringSystem].Att.EmployeesInfo a

  OUTER APPLY (SELECT ManagerEmail+';' as xx 
FROM Supervisor
WHERE StaffID= a.EmployeeID 

FOR XML PATH(''))D ( ManagerEmail ) 
)
--SELECT * from SupervisorList  


SELECT DISTINCT LEFT(s.ManagerEmail, LEN(s.ManagerEmail) - 1) [Supervisor Email]
--ta.[TravelAuthorizationID],
--ta.[TravelAuthorizationNumber],
--ta.[TravelersName],
--LatestStatus.[Description] [Status],
--Arr.ArrivalDate,
--u.FirstName +' ' + u.LastName [Name],
--u.Email,
--ta.UserID,
--ta.[TravelAuthorizationNumber] + ' / ' + u.FirstName +' ' + u.LastName  [Subject],
--s.PersonnelNumber , ad.PRISM_Number,
--ad.departmentID,ad.unitId,ad.subunitId

FROM 
TA.TravelAuthorization ta
CROSS APPLY (SELECT TOP (1) l.[Description] FROM TA.StatusChangeHistory h INNER JOIN Lkp.Lookups l ON l.Code = h.StatusCode WHERE h.TravelAuthorizationID=ta.TravelAuthorizationID ORDER BY h.CreatedDate DESC) LatestStatus
CROSS APPLY (SELECT TOP (1) ArrivalDate=ToLocationDate FROM TA.TravelItinerary WHERE TravelAuthorizationNumber=ta.TravelAuthorizationNumber ORDER BY CreatedDate DESC) Arr
INNER JOIN sec.Users u ON u.UserID = ta.UserID 
left JOIN [ActiveDirectoryUsers].Sec.UsersWithPRISMNo ad ON ad.Email = u.Email
LEFT JOIN SupervisorList s ON s.PersonnelNumber = ad.PRISM_Number

WHERE 
ta.isDeleted=0 AND 
LatestStatus.[Description] = 'TA Documents Complete' AND
s.ManagerEmail IS NOT NULL AND
s.EndDate IS NULL AND  
--DATEDIFF(d,Arr.ArrivalDate,GETDATE()) = 31  AND
ta.IsNotForDSA = 0 -- AND LEFT(s.ManagerEmail, LEN(s.ManagerEmail) - 1)=@SupervisorEmail


end
