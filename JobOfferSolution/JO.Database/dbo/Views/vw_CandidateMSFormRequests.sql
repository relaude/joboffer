Create View vw_CandidateMSFormRequests
AS
Select vcnd.Id
,(vcnd.FirstName + ' ' + vcnd.LastName) CandidateName
,vcnd.Email
,vcnd.PositionAppliedFor
,cmr.SubmissionReferenceNo
,cmr.RequestSentDate
,ISNULL(cmrs.StatusName,'Awaiting Submission') MSFormRequestStatus
From vw_Candidates vcnd
Left Join CandidateMSFormRequests cmr On vcnd.Id=cmr.CandidateId
Left Join CandidateFormRequestStatus cmrs On cmrs.Id=cmr.StatusId;