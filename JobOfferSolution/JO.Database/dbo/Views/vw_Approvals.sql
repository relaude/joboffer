

CREATE View [dbo].[vw_Approvals]
As
Select apv.Id
,prp.JobOfferId
,apv.StatusId
,jof.RefNum
,prps.StatusName
,apt.TypeName ApproverType
,prp.ProposeSalary
,jou.Name ApproverName
,apv.Comments
,apv.ApproveAt
,prps.BootstrapClass
From Approvals apv
Left Join Proposal prp On prp.Id=apv.ProposalId
Left Join JobOffers jof On jof.Id=prp.JobOfferId
Left Join ApproverTypes apt On apt.Id=apv.TypeId
Left Join ProposalStatus prps On prps.Id=apv.StatusId
Left Join JobOfferUsers jou On jou.Id=apv.ApproveBy;