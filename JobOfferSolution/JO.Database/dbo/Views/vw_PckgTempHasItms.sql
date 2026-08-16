
CREATE View [dbo].[vw_PckgTempHasItms]
As
Select pthi.Id,pthi.TempId,pthi.ItemId
,pct.TempName,pci.ItemName
,pthi.IsEnabled
,pci.Analysis,pci.Monthly,pci.Annualy
From PckgTempHasItms pthi
Left Join PckgTemp pct On pct.Id=pthi.TempId
Left Join PckgItems pci On pci.Id=pthi.ItemId;