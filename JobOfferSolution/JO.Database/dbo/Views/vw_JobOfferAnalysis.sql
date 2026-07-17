
CREATE View [dbo].[vw_JobOfferAnalysis]
As
Select joa.Id
,joa.CandidateApplicationId
,joa.PackageId
,vwcap.CandidateId
,joa.RecommendProposalId

,vwcap.ReferenceNumber
,vwcap.CandidateName
,vwcap.CandidateEmail
,vwcap.PositionAppliedFor

,vwsmtx.Currency
,jop.CurrentSalary
,joa.ExpectedSalary
,joa.BestProposalSalary
,joa.AnalysisNotes
,cbp.PackageName

,vls.ValidationStatusName
,vls.RiskLevelName
,vls.RiskLevelReason

,jop.IncreasePercentage
,jop.CompaRatio

,vwsmb.MatrixCode
,vwsmb.MatrixName
,vwsmb.JobLevelName
,vwsmb.JobFamilyName
,vwsmb.PositionGrade
,vwsmb.BandMinimum
,vwsmb.BandMidpoint
,vwsmb.BandMaximum

,joa.CreatedAt
,jou.Name CreatedByName

From JobOfferAnalysis joa
Left Join CompBenPackages cbp On cbp.Id=joa.PackageId
Left Join JobOfferProposal jop On jop.Id=joa.RecommendProposalId
Left Join ValidationStatus vls On vls.Id=joa.ValidationStatusId
Left Join vw_SalaryMatrixBand vwsmb On vwsmb.Id=joa.SalaryMatrixBandId
Left Join vw_CandidateApplications vwcap On vwcap.Id=joa.CandidateApplicationId
Left Join vw_SalaryMatrix vwsmtx On vwsmtx.Id=vwsmb.SalaryMatrixId
Left Join JobOfferUsers jou On jou.Id=joa.CreatedBy;