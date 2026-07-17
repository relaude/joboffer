
CREATE View [dbo].[vw_JobOfferProposal]
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
,prs.StatusName DHProposalStatus
,dhjop.Comments DHComments
,dhjop.ProposalStatusId DHProposalStatusId
From JobOfferProposal jop
Left Join ValidationStatus vst On vst.Id=jop.ValidationStatusId
Left Join CandidateApplications cap On cap.Id=jop.CandidateApplicationId
Left Join DHJOProposal dhjop On dhjop.JobOfferProposalId=jop.Id
Left Join ProposalStatus prs On prs.Id=dhjop.ProposalStatusId;