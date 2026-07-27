CREATE View vw_LegalEntities
As
Select leg.Id,leg.JobOfferId,leg.MatrixId 
,jof.RefNum
,(cmp.CompanyCode + ' - ' + cmp.CompanyName) CompanyCodeName
,(div.DivisionCode + ' - ' + div.DivisionName) DivisionCodeName
,(mtx.MatrixCode + ' - ' + mtx.MatrixName) MatrixCodeName
,jbp.PositionName
From LegalEntities leg
Left Join JobOffers jof On jof.Id=leg.JobOfferId
Left Join Companies cmp On cmp.Id=leg.CompanyId
Left Join Divisions div On div.Id=leg.DivisionId
Left Join SalaryMatrix mtx On mtx.Id=leg.MatrixId
Left Join JobPositions jbp On jbp.Id=leg.JobPositionId;