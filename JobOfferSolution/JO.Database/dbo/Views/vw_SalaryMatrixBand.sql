Create View vw_SalaryMatrixBand
As
Select smb.*
,smx.MatrixCode,smx.MatrixName
,jbl.JobLevelName
,jbf.JobFamilyName
,jpg.PositionGrade
From SalaryMatrixBand smb
Left Join SalaryMatrix smx On smx.Id=smb.SalaryMatrixId
Left Join JobLevels jbl On jbl.Id=smb.JobLevelId
Left Join JobFamilies jbf On jbf.Id=smb.JobFamilyId
Left Join JobPositionGrades jpg On jpg.Id=smb.PositionGradeId;