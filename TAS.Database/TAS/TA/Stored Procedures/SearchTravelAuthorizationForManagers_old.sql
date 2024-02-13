

CREATE PROCEDURE [TA].[SearchTravelAuthorizationForManagers_old]
    @TravelAuthorizationNumber NVARCHAR(14) ,
    @CreatedDate DATETIME ,
    @EmployeeName NVARCHAR(50) ,
    @ProcessedBy NVARCHAR(50) ,
    @Status NVARCHAR(3) ,
    @Department NVARCHAR(30)
AS
    BEGIN      
        SELECT  [TravelAuthorizationID] ,
                [TravelAuthorizationNumber] ,
                UPPER(Employee.CreatedByName) 'CreatedByName' ,
                UPPER([TravelersName]) 'TravelersName' ,
                [PurposeOfTravel] ,
                [TripSchemaCode] ,
                [ModeOfTravelCode] ,
                [SecurityClearance] ,
                [SecurityTraining] ,
                
				status.Description as StatusCode
				
				 ,
                [CreatedDate] ,
                t.[CreatedBy] ,
                [UpdatedDate] ,
                [UpdatedBy] ,
                t.[isDeleted] ,
                UPPER(Employee.CreatedByName) AS UpdatedByName,
				travel.FromLocationDate ,travel.ToLocationDate
        FROM    [TA].[TravelAuthorization] t
                CROSS APPLY ( SELECT DISTINCT
                                        u.[FirstName] + ' ' + u.[LastName] AS CreatedByName ,
                                        u.UserID ,
                                        r.RoleName
                              FROM      Sec.Users u
                                        INNER JOIN Sec.UsersRoles ur ON ur.UserID = u.UserID
                                        INNER JOIN Sec.Roles r ON r.RoleID = ur.RoleID
                                        INNER JOIN ActiveDirectoryUsers.Sec.Users asu  ON u.Username like  N'%'+ asu.UserName +'%'
                                                              AND asu.Department LIKE @Department
                                                              + '%'
                              WHERE     ( r.RoleName = 'Manager'
                                          OR r.RoleName = 'System Administrator'
                                        )
                                        AND u.UserID = t.UserID
                            ) Employee
				
				cross apply
				(
				
				SELECT TOP 1 s.Description,statusCode  FROM TA.StatusChangeHistory 
				inner join lkp.Lookups s on s.Code=TA.StatusChangeHistory.StatusCode
				WHERE TravelAuthorizationID =t.[TravelAuthorizationID] 
				 ORDER BY CreatedDate DESC  
				)status
               
			   
			    INNER JOIN [Lkp].[Lookups] l ON l.Code = status.StatusCode
                INNER JOIN [Lkp].[LookupsGroups] lg ON lg.LookupGroupID = l.LookupGroupID

				cross apply(
				select min(FromLocationDate) as  FromLocationDate , max(ToLocationDate) as  ToLocationDate from Ta.TravelItinerary 
				where TravelAuthorizationNumber=t.[TravelAuthorizationNumber]  

				group by  TravelAuthorizationNumber )travel


			


        WHERE   t.isDeleted = 0
                AND lg.LookupGroup = 'TA Status Code'
                AND ( t.[TravelAuthorizationNumber] = @TravelAuthorizationNumber
                      OR @TravelAuthorizationNumber = ''
                    )
                AND ( DATEDIFF(d, t.CreatedDate, @CreatedDate) = 0
                      OR @CreatedDate = CONVERT(DATETIME, '1900-01-01')
                    )

		
   


                AND ( Employee.CreatedByName LIKE '%' + @EmployeeName + '%'
                      OR @EmployeeName = ''
                    )
                AND ( l.Code = @Status
                      OR @Status = ''
                    );
    END;
	