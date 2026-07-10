CREATE View [dbo].[vw_CandidateApplications]
As
Select cap.Id,cap.ReferenceNumber
,cap.CandidateId,(cnd.FirstName + ' ' + cnd.LastName) CandidateName,cnd.Email CandidateEmail
,cmp.CompanyCode,cmp.CompanyName
,div.DivisionCode,div.DivisionName
,jop.PositionName PositionAppliedFor
,cap.SalaryMatrixId,smx.MatrixCode,smx.MatrixName,cur.Currency
,jou.Name CreatedByName,cap.CreatedAt
From CandidateApplications cap
Left Join Candidates cnd On cnd.Id=cap.CandidateId
Left Join Companies cmp On cmp.Id=cap.HiringCompanyId
Left Join Divisions div On div.Id=cap.HiringDivisionId
Left Join SalaryMatrix smx On smx.Id=cap.SalaryMatrixId
Left Join Currencies cur On cur.Id=smx.CurrencyId
Left Join JobPositions jop On jop.Id=cap.JobPositionId
Left Join JobOfferUsers jou On jou.Id=cap.CreatedBy;