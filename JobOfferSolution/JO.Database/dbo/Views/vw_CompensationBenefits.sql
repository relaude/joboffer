CREATE View vw_CompensationBenefits
As
Select cbs.Id 
,cbs.PackageId
,cbp.PackageName 
,cbi.ItemName
,cbi.ItemDescription
,cbt.TypeName
,cur.Currency
,cbs.Amount
,cbs.Tax
,cbs.IsTaxable
,cbs.IsRecurring
,frq.FrequencyName
,frq.Multiplier
,cbs.IsActive ActiveItem
,cbp.IsActive ActivePackage
,cbs.DisplayOrder
From CompensationBenefits cbs
Left Join CompBenPackages cbp On cbp.Id=cbs.PackageId
Left Join CompBenItems cbi On cbi.Id=cbs.CompBenItemId
Left Join CompBenTypes cbt On cbt.Id=cbi.TypeId
Left Join Currencies cur On cur.Id=cbs.CurrencyId
Left Join Frequencies frq On frq.Id=cbs.FrequencyId;