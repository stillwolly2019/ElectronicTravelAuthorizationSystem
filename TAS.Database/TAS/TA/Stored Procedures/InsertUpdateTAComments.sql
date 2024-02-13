-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE TA.InsertUpdateTAComments 
	@CommentId nvarchar(max),
	@TANumber nvarchar(50),
	@CommentType nvarchar(3),
	@Comment ntext,
	@CreatedBy nvarchar(max)
AS
BEGIN

If @CommentId='00000000-0000-0000-0000-000000000000'
Begin

insert into lkp.Comment 
(
TANumber,CommentType,Comment,CreatedBy,DateCreated)
values (@TANumber,@CommentType,@Comment,@CreatedBy,GETDATE())

End



else 

begin 

update 
lkp.Comment

set TANumber=@TANumber ,
CommentType=@CommentType,Comment=@Comment,
ModifiedBy=@CreatedBy,
DateModified=GETDATE() where CommentId=@CommentId
end


END
