CREATE PROCEDURE [dbo].[TruncateJobofferCompensation]	
AS
BEGIN
	truncate table JobOffers;
	truncate table JOAnalysis;
	truncate table JOCompanyCompensation;
	truncate table JOCompanyCompensationItems;
	truncate table JOActionLogs;
END