Create View vw_PckgTemp
As
Select pct.*
,cby.Name CreatedByName
,mdy.Name ModifiedByName
From PckgTemp pct
Left Join JobOfferUsers cby On cby.Id=pct.CreatedBy
Left Join JobOfferUsers mdy On mdy.Id=pct.ModifiedBy;