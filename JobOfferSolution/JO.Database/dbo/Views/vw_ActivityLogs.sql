Create View [vw_ActivityLogs]
As
Select alg.* 
,act.ActionName
,jou.Name ActionBy
From ActivityLogs alg
Left Join Activities act On act.Id=alg.Activity_Id
Left Join JobOfferUsers jou On jou.Id=alg.CreatedBy;