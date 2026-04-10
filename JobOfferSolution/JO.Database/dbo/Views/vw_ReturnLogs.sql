Create View vw_ReturnLogs
As
Select rlg.* 
,rtr.Reason ReasonName
,jou.Name ReturnBy
From ReturnLogs rlg
Left Join ReturnReasons rtr On rtr.Id=rlg.ReturnReason_Id
Left Join JobOfferUsers jou On jou.Id=rlg.CreatedBy;