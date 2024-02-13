
CREATE PROCEDURE [Sec].[GetStaffMembersByDepartmentID]
	@DepartmentID nvarchar(100),
	@UnitID nvarchar(100),
	@SubUnitID nvarchar(100)
AS
BEGIN

	SELECT '00000000-0000-0000-0000-000000000000' AS [UserID], '-- Please Select --' AS FullName
    UNION ALL
	SELECT [UserID] , [FirstName] + ' ' + [LastName] as FullName
	FROM [Sec].[Users] u
	CROSS APPLY (SELECT DepartmentID,UnitID,SubUnitID FROM [ActiveDirectoryUsers].Sec.UsersWithPRISMNo WHERE Username = u.Username) Dep
	WHERE 
	u.IsDeleted = 0 
	AND (Dep.DepartmentID = @DepartmentID)
	AND (Dep.UnitID = @UnitID OR @UnitID is null)
	AND (Dep.SubUnitID = @SubUnitID OR @SubUnitID is null)

END
