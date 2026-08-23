CREATE PROCEDURE [dbo].[TruncateJobofferCompensation]	
AS
BEGIN
	truncate table JobOffers;

	truncate table CompensationPackage;
	truncate table CompensationOptions;

	truncate table JOCompanyCompensation;
	truncate table JOCompanyCompensationItems;
END