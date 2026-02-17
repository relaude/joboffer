CREATE View vw_JobOfferTransactions
As
Select jot.*,can.Name CandidateName,can.Email CandidateEmail,mst.StatusName 
From JobOfferTransactions jot 
Left Join Candidates can on can.Id=jot.Candidate_Id
Left Join MainStatus mst on mst.Id=jot.MainStatus_Id