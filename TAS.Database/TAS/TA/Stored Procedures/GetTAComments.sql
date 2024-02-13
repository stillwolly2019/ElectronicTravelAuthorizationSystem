-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [TA].[GetTAComments]
	@TravelAuthorizationNumber nvarchar(50)
AS
BEGIN
	
	--select CommentId ,Comment ,CommentType , CreatedBy , DateCreated  , CommentID as RowID  from lkp.Comment 
	--where TANumber=@TravelAuthorizationNumber and CommentType='TA' and IsDeleted=0



SELECT  
		ROW_NUMBER() OVER(ORDER BY CommentID) AS RowID , 
	    n.CommentID , 
		case when n.DateModified='1900-01-01 00:00:00.000' then Comment + '<br/>' + 'Created By: '
                + CAST(umb.ModifiedBy AS NVARCHAR(50)) + '<br/>'
                + 'Created Date: ' + CONVERT(VARCHAR, n.DateCreated, 106)

				else 
	Comment + '<br/>' + 'Modified By: '
                + CAST(umb.ModifiedBy AS NVARCHAR(50)) + '<br/>'
                + 'Modified Date: ' + CONVERT(VARCHAR, n.DateModified, 106)  end 'CommentNotes' ,
           
		   
		        Comment,
                case when n.DateModified='1900-01-01 00:00:00.000'  then n.DateCreated else  n.DateModified    end  DateModified,
                case when n.ModifiedBy is null then n.CreatedBy   else n.ModifiedBy end ModifiedBy
				
        FROM    lkp.Comment  n
                OUTER APPLY ( SELECT    Username 'ModifiedBy'
                              FROM      Sec.Users
                              WHERE     UserID = case when n.ModifiedBy is null then n.CreatedBy else n.ModifiedBy end
                            ) umb
                
        WHERE   n.TANumber = @TravelAuthorizationNumber
                and CommentType='TA' and IsDeleted=0
        
		ORDER BY n.DateModified DESC;  





END
