Create View vw_JOApprovalFlow
As
Select jaf.* 
,vwjur.RoleName
From JOApprovalFlow jaf
Left Join vw_JOUserRoles vwjur On vwjur.Id=jaf.RoleId;