CREATE View [dbo].[vw_TAPartnerDboxCandidates]
As
Select vwdc.* From vw_DboxCandidates vwdc
Where vwdc.DivisionId <> 3;