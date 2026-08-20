Create View vw_CompensationTemplateItems
As
Select cti.* 
,ct.TemplateName
,ci.ItemName,ci.Monthly,ci.Annualy
From CompensationTemplateItems cti
Left Join CompensationTemplate ct On ct.Id=cti.TemplateId
Left Join CompensationItem ci On ci.Id=cti.ItemId;