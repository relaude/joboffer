


CREATE View [dbo].[vw_JODboxCandidates]
As
Select jof.Id, jof.RefNum
,vwcan.DboxRefNum,vwcan.CandidateName
,jof.StatusId, jos.StatusName, jos.BootstrapClass
,jof.OfferRangeId,ofr.RangeName
,jouc.Name CreatedByName,jof.CreatedAt
,joum.Name ModifiedByName,jof.ModifiedAt
From JobOffers jof
Left Join vw_DboxCandidates vwcan On vwcan.Id=jof.CandidateId
Left Join JobOfferStatus jos On jos.Id=jof.StatusId
Left Join OfferRange ofr On ofr.Id=jof.OfferRangeId
Left Join JobOfferUsers jouc On jouc.Id=jof.CreatedBy
Left Join JobOfferUsers joum On joum.Id=jof.ModifiedBy;