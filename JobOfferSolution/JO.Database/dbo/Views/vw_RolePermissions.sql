Create View vw_RolePermissions
AS
Select rpm.* 
,jor.Id JORoleId
,jor.RoleName
,prm.PermissionCode
,prm.PermissionName
From RolePermissions rpm
Left Join vw_JOUserRoles jor On jor.AspNetRoleId=rpm.AspNetRoleId
Left Join [Permissions] prm On prm.Id=rpm.PermissionId;