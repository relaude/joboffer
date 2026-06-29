

CREATE View [dbo].[vw_CandidateMSFormRequests]
As
Select cmr.Id
,cmr.CandidateId
,cmr.ReferenceNumber
,cmr.RequestSentDate
,cmr.Reminder1SentDate
,cmr.Reminder2SentDate
,cnd.FirstName
,cnd.MiddleName
,cnd.LastName
,cnd.Email
,cnd.PositionAppliedFor
,cfrs.StatusName
,jou.Name TAOwner
From CandidateMSFormRequests cmr
Left Join Candidates cnd On cnd.Id=cmr.CandidateId
Left Join CandidateFormRequestStatus cfrs On cfrs.Id=cmr.StatusId
Left Join JobOfferUsers jou On jou.Id=cmr.CreatedBy;