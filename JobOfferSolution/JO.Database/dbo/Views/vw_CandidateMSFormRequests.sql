CREATE View vw_CandidateMSFormRequests
As
Select cmr.Id
,cmr.ReferenceNumber
,cmr.RequestSentDate
,cnd.FirstName
,cnd.MiddleName
,cnd.LastName
,cnd.Email
,cnd.PositionAppliedFor
,cfrs.StatusName
From CandidateMSFormRequests cmr
Left Join Candidates cnd On cnd.Id=cmr.CandidateId
Left Join CandidateFormRequestStatus cfrs On cfrs.Id=cmr.StatusId;