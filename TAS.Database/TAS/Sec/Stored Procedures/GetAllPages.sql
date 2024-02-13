CREATE PROCEDURE [Sec].[GetAllPages]
AS
BEGIN
SELECT        Sec.Pages.PageID, Sec.Pages.PageName, Sec.Pages.PageURL, Sec.Pages.ParentID, Sec.Pages.PageOrder, Sec.Pages.IsDisplayedInMenu, 
                         ISNULL(Parents.PageName,'') AS Parent_Name,(CASE ISNULL(Parents.PageName,'') WHEN '' THEN '' ELSE ' -----------> ' END)AS PageSeparator
FROM            Sec.Pages LEFT OUTER JOIN
                         Sec.Pages AS Parents ON Sec.Pages.ParentID = Parents.PageID
						 WHERE Sec.Pages.IsDeleted=0
ORDER BY				
CASE WHEN Sec.Pages.ParentID = '00000000-0000-0000-0000-000000000000' THEN Pages.PageID
		ELSE Sec.Pages.ParentID END, ParentID, PageOrder, PageName	

END