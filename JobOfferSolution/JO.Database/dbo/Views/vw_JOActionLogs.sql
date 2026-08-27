CREATE View [dbo].[vw_JOActionLogs]
As
Select jal.* 
,jas.ActionName
,jof.Name ActionByName
,vwjur.RoleName
From JOActionLogs jal
Left Join vw_JOUserRoles vwjur On vwjur.Id=jal.RoleId
Left Join JOActionStatus jas On jas.Id=jal.ActionId
Left Join JobOfferUsers jof On jof.Id=jal.ActionBy;