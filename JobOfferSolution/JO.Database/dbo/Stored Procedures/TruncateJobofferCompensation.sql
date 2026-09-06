CREATE PROCEDURE [dbo].[TruncateJobofferCompensation]	
AS
BEGIN
	truncate table JobOffers;
	truncate table JOAnalysis;
	truncate table JOCompanyCompensation;
	truncate table JOCompanyCompensationItems;
	truncate table JOActionLogs;
	truncate table JOApprovalFlow;
	truncate table Discussions;

	update DboxCandidates set StatusId=1;
	update DboxCandidates set StatusId=null where ResponseId=0 Or ResponseId=null;
END