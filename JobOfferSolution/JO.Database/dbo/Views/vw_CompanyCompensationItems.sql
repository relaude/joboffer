CREATE   VIEW [dbo].[vw_CompanyCompensationItems]
AS
SELECT
    cci.Id,
    cci.CmpnyCmpnstnId,
    cc.CmpnyCmpnstnName,
    cci.ItemId,
    ci.ItemName,
    ci.CategoryId,
    ci.DisplayOrder,
    cci.MonthlyAmount,
    cci.AnnualAmount,
    cci.IsAnalysis,
    cci.IsEditable
FROM dbo.CompanyCompensationItems AS cci
LEFT JOIN dbo.CompanyCompensation AS cc
    ON cc.Id = cci.CmpnyCmpnstnId
LEFT JOIN dbo.CompensationItems AS ci
    ON ci.Id = cci.ItemId;