CREATE View vw_JOUserRoles
As
Select jor.Id,jor.AspNetRoleId 
,anr.Name RoleName
,urc.CategoryName RoleCategory
,jor.OrderBy
From JOUserRoles jor
Left Join AspNetRoles anr On anr.Id=jor.AspNetRoleId
Left Join UserRoleCategories urc On urc.Id=jor.RoleCategoryId;