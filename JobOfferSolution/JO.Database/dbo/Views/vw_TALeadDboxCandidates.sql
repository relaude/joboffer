CREATE View [dbo].[vw_TALeadDboxCandidates]
As
Select vwdc.* From vw_DboxCandidates vwdc
Where vwdc.DivisionId = 3;