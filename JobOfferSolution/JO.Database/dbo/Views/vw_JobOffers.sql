
CREATE View [dbo].[vw_JobOffers]
As
Select jof.* 
,(can.FirstName + ' ' +can.LastName) CandidateName
,jop.PositionName
,dpt.DepartmentName
,mst.StatusName
,drs.Reason DeclineReason
From JobOffers jof 
Left Join Candidates can On can.Id=jof.Candidate_Id
Left Join JobPositions jop On jop.Id=jof.JobPosition_Id
Left Join Departments dpt On dpt.Id=jof.Department_Id
Left Join MainStatus mst On mst.Id=jof.MainStatus_Id
Left Join DeclineReasons drs On drs.Id=jof.DeclineReason_Id;