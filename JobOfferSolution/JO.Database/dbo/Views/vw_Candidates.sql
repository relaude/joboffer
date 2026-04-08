CREATE View [dbo].[vw_Candidates]
As
Select can.*
,ems.StatusName EmploymentStatusName 
,woe.Description WorkExperienceName
,jp.PositionName JobPositionName
,dpt.DepartmentName
,cst.StatusName CandidateJOStatus
From Candidates can
Left Join EmploymentStatus ems On ems.Id=can.EmploymentStatus_Id
Left Join WorkExperience woe On woe.Id=can.WorkExperience_Id
Left Join JobPositions jp On jp.Id=can.JobPosition_Id
Left Join Departments dpt On dpt.Id=can.Department_Id
Left Join CandidateStatus cst On cst.Id=can.CandidateStatus_Id