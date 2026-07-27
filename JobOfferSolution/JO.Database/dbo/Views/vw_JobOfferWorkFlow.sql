
CREATE View [dbo].[vw_JobOfferWorkFlow]
As
Select jowf.Id,jowf.JobOfferId,jowf.ActionId 
,wfs.DisplayOrder,wfs.ShortName,wfs.Icon
,wfa.ActionName,wfa.BootstrapClass ActionClass
From WorkFlow jowf
Left Join WorkFlowStatus wfs On wfs.Id=jowf.StatusId
Left Join WorkFlowActions wfa On wfa.Id=jowf.ActionId;