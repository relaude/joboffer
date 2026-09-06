Create View vw_TAPartnerDboxCandidates
As
Select vwdc.* From vw_DboxCandidates vwdc
Where vwdc.DivisionId <> 3
And CSGId Not In (select Id from CompanySalaryGrades where GradeId=1);