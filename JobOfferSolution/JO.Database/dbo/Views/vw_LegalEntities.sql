
CREATE View [dbo].[vw_LegalEntities]
As
Select leg.Id,leg.JobOfferId,leg.MatrixId 
,jof.RefNum
,(cmp.CompanyCode + ' - ' + cmp.CompanyName) CompanyCodeName
,(div.DivisionCode + ' - ' + div.DivisionName) DivisionCodeName
,'-' MatrixCodeName
,jbp.PositionName
From LegalEntities leg
Left Join JobOffers jof On jof.Id=leg.JobOfferId
Left Join Companies cmp On cmp.Id=leg.CompanyId
Left Join Divisions div On div.Id=leg.DivisionId
Left Join JobPositions jbp On jbp.Id=leg.JobPositionId;