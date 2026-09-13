Create Procedure ResetJobOffer
AS
Begin
	truncate table CandidateResponseRawData;
	truncate table CandidateResponses;
	truncate table DboxCandidatesRawData;
	truncate table DboxCandidates;

	truncate table JobOffers;
	truncate table JOAnalysis;
	truncate table JOCompanyCompensation;
	truncate table JOCompanyCompensationItems;
	truncate table JOActionLogs;
	truncate table JOApprovalFlow;
	truncate table Discussions;
End