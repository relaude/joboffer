
CREATE View [dbo].[vw_JobOfferWorkFlow]
As
Select jowf.* 
,wfs.ShortName,wfs.Icon
,wfa.ActionName,wfa.BootstrapClass ActionClass
From JobOfferWorkFlow jowf
Left Join WorkFlowStatus wfs On wfs.Id=jowf.WorkFlowStatusId
Left Join WorkFlowActions wfa On wfa.Id=jowf.WorkFlowActionId;