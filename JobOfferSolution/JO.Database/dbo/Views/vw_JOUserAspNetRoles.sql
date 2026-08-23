Create View vw_JOUserAspNetRoles
As
Select jur.Id
,jur.OrderBy
,jur.AspNetRoleId
,aspr.Name RoleName
,jur.IsActive
,jur.Description
From JOUserRoles jur
Left Join AspNetRoles aspr On aspr.Id=jur.AspNetRoleId;