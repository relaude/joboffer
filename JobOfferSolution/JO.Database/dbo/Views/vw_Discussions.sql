


CREATE View [dbo].[vw_Discussions]
As
Select dis.*
,jof.RefNum
,jcc.OptionNumber,jcc.ProposedSalary
,dst.StatusName
,jdr.ReasonName
From Discussions dis
Left Join JobOffers jof On jof.Id=dis.JobOfferId
Left Join JOCompanyCompensation jcc On jcc.Id=dis.ProposalId
Left Join DiscussionStatus dst On dst.Id=dis.StatusId
Left Join JODeclineReason jdr On jdr.Id=dis.DeclineReasonId;