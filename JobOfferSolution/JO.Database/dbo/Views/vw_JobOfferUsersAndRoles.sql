Create View vw_JobOfferUsersAndRoles
As
Select jou.Id
,jou.Name
,jou.Email
,jou.IsActive
,jouc.Name CreatedByName,jou.CreatedAt
,joum.Name ModifiedByName,jou.ModifiedAt
,anr.Name RoleName
From JobOfferUsers jou
Left Join JobOfferUsers jouc On jouc.Id=jou.CreatedBy
Left Join JobOfferUsers joum On joum.Id=jou.ModifiedBy
Left Join AspNetUserRoles anur On anur.UserId=jou.AspNetUserId
Left Join AspNetRoles anr On anr.Id=anur.RoleId;