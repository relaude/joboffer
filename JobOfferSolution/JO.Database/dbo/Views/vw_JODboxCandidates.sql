CREATE View vw_JODboxCandidates
As
Select jof.Id, jof.RefNum
,vwcan.CandidateFullName
,jof.StatusId, jos.StatusName, jos.BootstrapClass
,jouc.Name CreatedByName,jof.CreatedAt
From JobOffers jof
Left Join vw_DboxCandidates vwcan On vwcan.Id=jof.CandidateId
Left Join JobOfferStatus jos On jos.Id=jof.StatusId
Left Join JobOfferUsers jouc On jouc.Id=jof.CreatedBy;