Create View vw_JobOfferHasEmail
As
Select johe.Id,johe.StatusId,johe.Subject
,johes.StatusName
,jof.RefNum JORefNum
,dcan.CandidateName
,jouc.Name CreatedByName,johe.CreatedAt
,joum.Name ModifiedByName,johe.ModifiedAt
From JobOfferHasEmail johe
Left Join JOHasEmailStatus johes On johes.Id=johe.StatusId
Left Join DboxCandidates dcan On dcan.Id=johe.CandidateId
Left Join JobOffers jof On jof.Id=johe.JobOfferId
Left Join JobOfferUsers jouc On jouc.Id=johe.CreatedBy
Left Join JobOfferUsers joum On joum.Id=johe.ModifiedBy;