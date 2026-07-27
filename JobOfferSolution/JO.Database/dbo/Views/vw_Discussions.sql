Create View vw_Discussions
As
Select dis.*
,jof.RefNum
,prp.OptionNum,prp.ProposeSalary
,dst.StepName,dst.Icon
,cht.ChannelName
,car.ResponseName
From Discussions dis
Left Join JobOffers jof On jof.Id=dis.JobOfferId
Left Join Proposal prp On prp.Id=dis.ProposalId
Left Join DiscussSteps dst On dst.Id=dis.StepId 
Left Join ChannelTypes cht On cht.Id=dis.ChannelId
Left Join CandResponse car On car.Id=dis.ResponseId;