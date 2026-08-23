Create View vw_JobOfferUsers
As
Select jou.Id
,jou.Name
,jou.Email
,jou.IsActive
,jouc.Name CreatedByName,jou.CreatedAt
,joum.Name ModifiedByName,jou.ModifiedAt
,(select count(RoleId) from AspNetUserRoles where UserId=jou.AspNetUserId) CountRoles
From JobOfferUsers jou
Left Join JobOfferUsers jouc On jouc.Id=jou.CreatedBy
Left Join JobOfferUsers joum On joum.Id=jou.ModifiedBy;