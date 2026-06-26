Create View vw_Divisions
As
Select div.*,cmp.CompanyCode,cmp.CompanyName
From Divisions div
Left Join Companies cmp On cmp.Id=div.CompanyId;