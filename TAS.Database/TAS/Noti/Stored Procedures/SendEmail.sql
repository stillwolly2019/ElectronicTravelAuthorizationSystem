
CREATE PROCEDURE [Noti].[SendEmail]
@TO nvarchar(max),
@CC nvarchar(max),
@Subject nvarchar(max),
@Body nvarchar(max),
@Attachment nvarchar(max)
AS
BEGIN
	EXEC	msdb.dbo.sp_send_dbmail
		@profile_name = 'Email',
		@recipients = @TO,
		@copy_recipients= @CC,
		@subject = @Subject,
		@body = @Body,
		@file_attachments=@Attachment,
		@body_format = 'HTML';


End 