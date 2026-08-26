Create View vw_Companies
As
Select cmp.* 
,(select count(Id) from Divisions where CompanyId=cmp.Id) CountDivision
From Companies cmp;