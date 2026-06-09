CREATE View [dbo].[vw_SalaryMatrix]
As
Select smx.* 
,cmp.CompanyName
,dvn.DivisionName
,(crn.Currency + ' - ' + crn.Description) CurrencyName
,crn.Currency
,sms.StatusName
,apr.Name ApproverName
,crb.Name CreatedByName
,mdb.Name ModifiedByName
From SalaryMatrix smx
Left Join Companies cmp On cmp.Id=smx.CompanyId
Left Join Divisions dvn On dvn.Id=smx.DivisionId
Left Join Currencies crn On crn.Id=smx.CurrencyId
Left Join SalaryMatrixStatus sms On sms.Id=smx.ApprovalStatusId
Left Join JobOfferUsers apr On apr.Id=smx.ApprovedByUserId
Left Join JobOfferUsers crb On crb.Id=smx.CreatedBy
Left Join JobOfferUsers mdb On mdb.Id=smx.ModifiedBy;