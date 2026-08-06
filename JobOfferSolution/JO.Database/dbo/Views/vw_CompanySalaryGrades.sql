Create View vw_CompanySalaryGrades
AS
Select csg.*
,cmp.CompanyCode,cmp.CompanyName
,gty.TypeName,sgd.GradeName
From CompanySalaryGrades csg
Left Join Companies cmp On cmp.Id=csg.CompanyId
Left Join SalaryGrades sgd On sgd.Id=csg.GradeId
Left Join GradeTypes gty On gty.Id=sgd.TypeId;