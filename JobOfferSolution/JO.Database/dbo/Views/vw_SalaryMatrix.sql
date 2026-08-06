


CREATE View [dbo].[vw_SalaryMatrix]
As
Select smx.Id,smx.IsActive,smx.CompanyId
,cmp.CompanyCode,cmp.CompanyName
,smx.CreatedAt,crb.Name CreatedByName
,smx.ModifiedAt,mdb.Name ModifiedByName
,(select count(sb.Id) from SalaryBands sb where sb.MatrixId=smx.Id) BandCount
From SalaryMatrix smx
Left Join Companies cmp On cmp.Id=smx.CompanyId
Left Join JobOfferUsers crb On crb.Id=smx.CreatedBy
Left Join JobOfferUsers mdb On mdb.Id=smx.ModifiedBy;