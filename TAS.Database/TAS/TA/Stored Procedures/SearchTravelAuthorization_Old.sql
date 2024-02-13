CREATE PROCEDURE [TA].[SearchTravelAuthorization_Old]
    @TravelAuthorizationNumber NVARCHAR(14) ,
    @CreatedDate DATETIME ,
	@CreatedDateTo Datetime,
    @EmployeeName NVARCHAR(50) ,
    @ProcessedBy NVARCHAR(50) ,
    @Status NVARCHAR(3)
AS
    BEGIN		
                SELECT TOP 10 [TravelAuthorizationID] ,
                        [TravelAuthorizationNumber] ,
                        Upper(Employee.[FirstName]) + ' ' + Upper(Employee.[LastName]) AS CreatedByName ,
                        Upper([TravelersName]) as [TravelersName] ,
                        [PurposeOfTravel] ,
                        [TripSchemaCode] ,
                        [ModeOfTravelCode] ,
                        [SecurityClearance] ,
                        [SecurityTraining] ,
                       status.Description as [StatusCode] ,
                        [CreatedDate] ,
                        t.[CreatedBy] ,
                        [UpdatedDate] ,
                        [UpdatedBy] ,
                        t.[isDeleted] ,
                        Upper(Employee.[FirstName]) + ' ' + Upper(Employee.[LastName] )AS UpdatedByName,travel.FromLocationDate ,travel.ToLocationDate
                FROM    [TA].[TravelAuthorization] t

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

                        INNER JOIN Sec.Users Employee ON Employee.UserID = t.UserID
                      left JOIN Sec.Users Processed ON Processed.UserID = t.UpdatedBy
                        INNER JOIN [Lkp].[Lookups] l ON l.Code =status.StatusCode
                        INNER JOIN [Lkp].[LookupsGroups] lg ON lg.LookupGroupID = l.LookupGroupID


							

                WHERE   t.isDeleted = 0
                        AND lg.LookupGroup = 'TA Status Code'
                        AND ( t.[TravelAuthorizationNumber] = @TravelAuthorizationNumber
                              OR @TravelAuthorizationNumber = ''
                            )
                        AND ( DATEDIFF(d, t.CreatedDate, @CreatedDate) = 0
                              OR @CreatedDate = CONVERT(DATETIME, '1900-01-01')
                            )
                        AND ( Employee.FirstName + ' ' + Employee.LastName LIKE '%'
                              + @EmployeeName + '%'
                              OR @EmployeeName = ''
                            )
                        AND ( Processed.FirstName + ' ' + Processed.LastName LIKE '%'
                              + @ProcessedBy + '%'
                              OR @ProcessedBy = ''
                            )
                        AND ( l.Code = @Status
                              OR @Status = ''
                            )
							ORDER BY [CreatedDate] DESC;
    END;