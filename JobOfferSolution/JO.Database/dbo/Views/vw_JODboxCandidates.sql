CREATE View [dbo].[vw_JODboxCandidates]
As
Select jof.Id, jof.RefNum
,vwcan.DboxRefNum,vwcan.CandidateName
,jof.WorkFlowId,wfs.FlowName
,jof.OfferRangeId,ofr.RangeName
,jouc.Name CreatedByName,jof.CreatedAt
,joum.Name ModifiedByName,jof.ModifiedAt
,(select top 1 (vwjol.ActionName + ' by ' + vwjol.RoleName) from vw_JOActionLogs vwjol where vwjol.JobOfferId=jof.Id order by vwjol.ActionAt desc) ActionByName
From JobOffers jof
Left Join vw_DboxCandidates vwcan On vwcan.Id=jof.CandidateId
Left Join JOWorkFlowStatus wfs On wfs.Id=jof.WorkFlowId
Left Join OfferRange ofr On ofr.Id=jof.OfferRangeId
Left Join JobOfferUsers jouc On jouc.Id=jof.CreatedBy
Left Join JobOfferUsers joum On joum.Id=jof.ModifiedBy;