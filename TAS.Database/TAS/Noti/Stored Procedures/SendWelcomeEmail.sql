

CREATE PROCEDURE [Noti].[SendWelcomeEmail]
AS
BEGIN

    IF OBJECT_ID('tempdb.dbo.#ArrivedToday', 'U') IS NOT NULL
        DROP TABLE #ArrivedToday;

    SELECT u.Username,
           u.FirstName + ' ' + u.LastName AS FullName,
           u.Email,
           t.TravelAuthorizationNumber,
           Travel.FromLocationDate,
           Travel.ToLocationDate,
           CityDescription AS FromCity
    INTO #ArrivedToday
    FROM [TA].[TravelAuthorization] t
        CROSS APPLY
    (
        SELECT TOP 1
               MIN(FromLocationDate) AS FromLocationDate,
               MAX(ToLocationDate) AS ToLocationDate,
               ti.FromLocationCode,
               isDeleted
        FROM TA.TravelItinerary ti
        WHERE ti.TravelAuthorizationNumber = t.[TravelAuthorizationNumber]
              AND ti.isDeleted = 0
        GROUP BY ti.TravelAuthorizationNumber,
                 FromLocationCode,
                 isDeleted
        ORDER BY ToLocationDate DESC
    ) Travel
        INNER JOIN Lookups.Lkp.City CityTo
            ON CityTo.CityID = Travel.FromLocationCode
        OUTER APPLY
    (
        SELECT TOP 1
               *
        FROM TA.StatusChangeHistory sch
        WHERE sch.TravelAuthorizationID = t.TravelAuthorizationID
        ORDER BY CreatedDate DESC
    ) StatusCode
        INNER JOIN Sec.Users u
            ON u.UserID = t.UserID
    WHERE CONVERT(DATE, Travel.ToLocationDate) = CONVERT(DATE, DATEADD(DAY,-1,GETDATE()) )
          AND Travel.isDeleted = 0
          AND StatusCode.StatusCode = 'TADC'
    GROUP BY TravelAuthorizationNumber,
             u.Username,
             u.FirstName,
             u.LastName,
             Email,
             FromLocationDate,
             ToLocationDate,
             CityDescription;
    --SELECT 
    --	u.Username,
    --	u.FirstName + ' ' + u.LastName AS FullName,
    --	u.Email,
    --	t.TravelAuthorizationNumber,
    --	Travel.FromLocationDate,
    --	Travel.ToLocationDate,
    --	CityDescription AS FromCity
    --INTO 
    --	#ArrivedToday
    --FROM 
    --	[TA].[TravelAuthorization] t CROSS APPLY
    --	(SELECT MIN(FromLocationDate) as  FromLocationDate, MAX(ToLocationDate) AS ToLocationDate, ToLocationCode, isDeleted
    --FROM 
    --	Ta.TravelItinerary ti
    --WHERE 
    --	ti.TravelAuthorizationNumber= t.[TravelAuthorizationNumber] 
    --	AND
    --	ti.isDeleted = 0
    --	GROUP BY  ti.TravelAuthorizationNumber,ToLocationCode, isDeleted) Travel INNER JOIN 
    --	Lookups.Lkp.City CityTo ON CityTo.CityID = Travel.ToLocationCode OUTER APPLY
    --    (SELECT TOP 1 * FROM TA.StatusChangeHistory sch WHERE sch.TravelAuthorizationID =  t.TravelAuthorizationID ORDER BY CreatedDate DESC) StatusCode
    --	INNER JOIN Sec.Users u ON u.UserID = t.UserID
    --WHERE 
    --	Travel.ToLocationDate  = CONVERT(date,GETDATE())
    --	AND
    --	Travel.isDeleted =0
    --	AND
    --	StatusCode.StatusCode = 'TADC'

    DECLARE @Email NVARCHAR(50);
    DECLARE @TANo NVARCHAR(150);
    DECLARE @FullName NVARCHAR(150);
    DECLARE @TDYLocation NVARCHAR(150);
    DECLARE @EmailSubject NVARCHAR(150);
    DECLARE @body_content NVARCHAR(MAX);

    WHILE
    (SELECT COUNT(*) FROM #ArrivedToday) > 0
    BEGIN

        SET @Email =
        (
            SELECT TOP (1) Email FROM #ArrivedToday ORDER BY Username
        );
        SET @TANo =
        (
            SELECT TOP (1)
                   TravelAuthorizationNumber
            FROM #ArrivedToday
            ORDER BY Username
        );
        SET @FullName =
        (
            SELECT TOP (1) FullName FROM #ArrivedToday ORDER BY Username
        );
        SET @TDYLocation =
        (
            SELECT TOP (1) FromCity FROM #ArrivedToday ORDER BY Username
        );
        SET @EmailSubject = @TANo + N'/ ' + @FullName;
        --SET @EmailSubject = @TANo + '/ ' + @FullName + '/ ' + 'to' + '/ ' +  @TDYLocation

        SET @body_content
            = N'<html><head>' + N'<style>' + N'p {font-family:calibri;} ' + N'</style>'
              + N'</head><body><p>Dear Colleague,</p><br><br>
		   <p> Welcome back!  We hope you had a wonderful and productive duty travel.  
			Your travel expense claim (TEC) is now available for your completion in <a href="http://ammappsvr01/TAS/Login.aspx">http://ammappsvr01/TAS/Login.aspx</a>.  
			Please complete your TEC online and hand deliver the signed form, 
			including all required supporting documents to TEC Admin (6th Floor, Main office Building, Room 603 by <b>'
              + (CONVERT(VARCHAR(10), DATEADD(DAY, 14, GETDATE()), 103))
              + N'</b>).</p><br><br>
			
		   <p>For your information, your travel expense claim will be reviewed and entitlements calculated according to <a href="https://intranetportal/Pages/ControlNo.aspx?controlNo=IN/00006">IOM Travel Rules and Regulations</a>
		    and related internal instructions.</p>
			<br><br>
           <p> If you have any question, please feel free to contact TEC Admin team.  Hope to hear from you soon! </p>
           <br><br>
           <p>Best regards,<br><br>
           TEC Admin Team</p>
		   </body></html>';


        EXEC msdb.dbo.sp_send_dbmail @profile_name = 'Email',
                                     @recipients = @Email,
                                     @subject = @EmailSubject,
                                     @body = @body_content,
                                     @file_attachments = '\\ammappsvr01\DocumentsUploads\TDY REPORT TEMPLATE JO10-TA-XX-XXX SURNAME TO LOCATION.docx.doc',
                                     @body_format = 'HTML';

        DELETE FROM #ArrivedToday
        WHERE #ArrivedToday.TravelAuthorizationNumber = @TANo;
    END;

    DROP TABLE #ArrivedToday;

END;
