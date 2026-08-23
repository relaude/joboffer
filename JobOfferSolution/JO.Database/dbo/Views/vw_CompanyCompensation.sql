CREATE   VIEW [dbo].[vw_CompanyCompensation]
AS
SELECT
    cc.Id,
    cc.CompanyId,
    c.CompanyCode,
    c.CompanyName,
    cc.CmpnyCmpnstnName,
    cc.IsActive,
    (
        SELECT COUNT(*)
        FROM dbo.CompanyCompensationItems AS cci
        WHERE cci.CmpnyCmpnstnId = cc.Id
    ) AS ItemCount,
    cc.CreatedBy,
    createdUser.Name AS CreatedByName,
    cc.CreatedAt,
    cc.ModifiedBy,
    modifiedUser.Name AS ModifiedByName,
    cc.ModifiedAt
FROM dbo.CompanyCompensation AS cc
LEFT JOIN dbo.Companies AS c
    ON c.Id = cc.CompanyId
LEFT JOIN dbo.JobOfferUsers AS createdUser
    ON createdUser.Id = cc.CreatedBy
LEFT JOIN dbo.JobOfferUsers AS modifiedUser
    ON modifiedUser.Id = cc.ModifiedBy;