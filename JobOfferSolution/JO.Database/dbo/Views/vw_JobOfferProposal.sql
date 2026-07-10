CREATE View vw_JobOfferProposal
As
Select jop.Id
,jop.JobOfferAnalysisId
,cap.ReferenceNumber
,jop.OptionNumber
,jop.CurrentSalary
,jop.ProposedSalary
,jop.SalaryMidpoint
,jop.CompaRatio
,jop.IncreasePercentage
,jop.AnnualSalary
,vst.ValidationStatusName
,jop.Justification
From JobOfferProposal jop
Left Join ValidationStatus vst On vst.Id=jop.ValidationStatusId
Left Join CandidateApplications cap On cap.Id=jop.CandidateApplicationId;