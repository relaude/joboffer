CREATE View vw_JobOffers
As
Select jof.Id
,jof.RefNum
,jos.StatusName 
,(can.FName + ' ' + can.LName) CandidateName, can.Email
,jos.BootstrapClass
,jof.StatusId
,jof.RequestId
From JobOffers jof
Left Join JobOfferStatus jos On jos.Id=jof.StatusId
Left Join Candidates can On can.Id=jof.CandidateId;