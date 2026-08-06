Create View vw_SalaryBands
As
Select sbd.*
,vwcsg.TypeName,vwcsg.GradeName
From SalaryBands sbd
Left Join vw_CompanySalaryGrades vwcsg On vwcsg.Id=sbd.CSGId;