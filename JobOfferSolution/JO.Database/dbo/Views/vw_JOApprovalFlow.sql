
CREATE View [dbo].[vw_JOApprovalFlow]
As
Select jaf.* 
,vwjur.RoleName
,jas.ActionName
From JOApprovalFlow jaf
Left Join vw_JOUserRoles vwjur On vwjur.Id=jaf.RoleId
Left Join JOActionStatus jas On jas.Id=jaf.ActionId;