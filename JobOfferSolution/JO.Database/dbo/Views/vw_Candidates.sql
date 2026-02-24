CREATE View vw_Candidates
As
Select can.*
,ems.StatusName EmploymentStatusName 
,woe.Description WorkExperienceName
From Candidates can
Left Join EmploymentStatus ems On ems.Id=can.EmploymentStatus_Id
Left Join WorkExperience woe On woe.Id=can.WorkExperience_Id;