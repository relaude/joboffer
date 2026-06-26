
CREATE View vw_JOUsersInRoles
As
Select jou.Id
,jou.Name FullName
,jou.Email
,vwjour.RoleName
,vwjour.RoleCategory
,vwjour.AspNetRoleId
,jou.IsActive
From JobOfferUsers jou
Left Join AspNetUserRoles aur On aur.UserId=jou.AspNetUserId
Left Join vw_JOUserRoles vwjour On vwjour.AspNetRoleId=aur.RoleId;