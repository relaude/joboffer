Create View vw_TALeadDboxCandidates
As
Select vwdc.* From vw_DboxCandidates vwdc
Where vwdc.DivisionId = 3
Or CSGId In (select Id from CompanySalaryGrades where GradeId=1);