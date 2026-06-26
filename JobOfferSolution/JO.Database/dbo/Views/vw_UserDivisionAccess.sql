Create View vw_UserDivisionAccess
As
Select uda.* 
,jou.Name JOUserName
,cmp.CompanyCode
,cmp.CompanyName
,div.DivisionCode
,div.DivisionName
From UserDivisionAccess uda
Left Join JobOfferUsers jou On jou.Id=uda.JobOfferUserId
Left Join Divisions div On div.Id=uda.DivisionId
Left Join Companies cmp On cmp.Id=div.CompanyId;