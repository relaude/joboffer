
CREATE View [dbo].[vw_SalaryBands]
As
Select sbd.*
,vwcsg.TypeName,vwcsg.GradeName
,vwcsg.CompanyId,vwcsg.GradeId
From SalaryBands sbd
Left Join vw_CompanySalaryGrades vwcsg On vwcsg.Id=sbd.CSGId;