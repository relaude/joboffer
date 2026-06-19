/*
    Validation checks for SeedHybridRbacAbacSampleData.sql.
    Run this after running the seed script. A passing run returns zero rows in
    each failure result set and prints a success message.
*/

SET NOCOUNT ON;

DECLARE @ExpectedUsers TABLE (Email NVARCHAR(256) NOT NULL);
INSERT INTO @ExpectedUsers (Email)
VALUES
    (N'paula.santos@unilab.com.ph'),
    (N'carlo.reyes@amherst.com.ph'),
    (N'maria.delacruz@unilab.com.ph'),
    (N'victor.lim@unilab.com.ph'),
    (N'grace.tan@amherst.com.ph'),
    (N'enrique.sy@unilab.com.ph'),
    (N'nina.garcia@unilab.com.ph'),
    (N'ana.cruz@unilab.com.ph'),
    (N'allan.yu@unilab.com.ph'),
    (N'admin.security@unilab.com.ph');

DECLARE @ExpectedRoles TABLE (RoleName NVARCHAR(256) NOT NULL);
INSERT INTO @ExpectedRoles (RoleName)
VALUES
    (N'System Administrator'),
    (N'TA Partner'),
    (N'HROD Head'),
    (N'Division Head'),
    (N'President'),
    (N'Total Rewards Partner'),
    (N'HRBP'),
    (N'System Auditor');

DECLARE @ExpectedPermissions TABLE (PermissionCode NVARCHAR(50) NOT NULL);
INSERT INTO @ExpectedPermissions (PermissionCode)
VALUES
    (N'JO.CREATE'),
    (N'JO.EDIT'),
    (N'JO.ANALYZE'),
    (N'JO.SUBMIT_APPROVAL'),
    (N'JO.APPROVE'),
    (N'JO.REJECT'),
    (N'JO.RETURN'),
    (N'JO.GENERATE_OFFER'),
    (N'JO.VIEW_DIVISION_STATUS'),
    (N'JO.VIEW_ABOVE_BAND_REPORT'),
    (N'JO.MAINTAIN_SALARY_MATRIX'),
    (N'SECURITY.MANAGE_USERS'),
    (N'SECURITY.VIEW_AUDIT');

DECLARE @ExpectedUserRoles TABLE
(
    Email NVARCHAR(256) NOT NULL,
    RoleName NVARCHAR(256) NOT NULL
);

INSERT INTO @ExpectedUserRoles (Email, RoleName)
VALUES
    (N'paula.santos@unilab.com.ph', N'TA Partner'),
    (N'carlo.reyes@amherst.com.ph', N'TA Partner'),
    (N'maria.delacruz@unilab.com.ph', N'HROD Head'),
    (N'victor.lim@unilab.com.ph', N'Division Head'),
    (N'grace.tan@amherst.com.ph', N'Division Head'),
    (N'enrique.sy@unilab.com.ph', N'President'),
    (N'nina.garcia@unilab.com.ph', N'Total Rewards Partner'),
    (N'ana.cruz@unilab.com.ph', N'HRBP'),
    (N'allan.yu@unilab.com.ph', N'System Auditor'),
    (N'admin.security@unilab.com.ph', N'System Administrator');

SELECT N'Missing expected role' AS FailureType, roleSource.RoleName AS MissingValue
FROM @ExpectedRoles roleSource
WHERE NOT EXISTS
(
    SELECT 1
    FROM dbo.AspNetRoles roleTarget
    WHERE roleTarget.Name = roleSource.RoleName
);

SELECT N'Missing expected user' AS FailureType, userSource.Email AS MissingValue
FROM @ExpectedUsers userSource
WHERE NOT EXISTS
(
    SELECT 1
    FROM dbo.AspNetUsers userTarget
    WHERE userTarget.Email = userSource.Email
);

SELECT N'Missing JobOfferUsers link' AS FailureType, userSource.Email AS MissingValue
FROM @ExpectedUsers userSource
WHERE NOT EXISTS
(
    SELECT 1
    FROM dbo.JobOfferUsers jobUser
    INNER JOIN dbo.AspNetUsers identityUser
        ON identityUser.Id = jobUser.AspNetUserId
    WHERE jobUser.Email = userSource.Email
      AND identityUser.Email = userSource.Email
);

SELECT N'Missing expected permission' AS FailureType, permissionSource.PermissionCode AS MissingValue
FROM @ExpectedPermissions permissionSource
WHERE NOT EXISTS
(
    SELECT 1
    FROM dbo.Permissions permissionTarget
    WHERE permissionTarget.PermissionCode = permissionSource.PermissionCode
);

SELECT N'Missing expected user role' AS FailureType, userRoleSource.Email + N' / ' + userRoleSource.RoleName AS MissingValue
FROM @ExpectedUserRoles userRoleSource
WHERE NOT EXISTS
(
    SELECT 1
    FROM dbo.AspNetUsers identityUser
    INNER JOIN dbo.AspNetUserRoles userRole
        ON userRole.UserId = identityUser.Id
    INNER JOIN dbo.AspNetRoles roleTarget
        ON roleTarget.Id = userRole.RoleId
    WHERE identityUser.Email = userRoleSource.Email
      AND roleTarget.Name = userRoleSource.RoleName
);

SELECT N'Orphaned seeded AspNetUserRoles row' AS FailureType, userRole.UserId + N' / ' + userRole.RoleId AS MissingValue
FROM dbo.AspNetUserRoles userRole
INNER JOIN dbo.AspNetUsers identityUser
    ON identityUser.Id = userRole.UserId
WHERE identityUser.Email IN (SELECT Email FROM @ExpectedUsers)
  AND NOT EXISTS (SELECT 1 FROM dbo.AspNetRoles roleTarget WHERE roleTarget.Id = userRole.RoleId);

SELECT N'Orphaned seeded RolePermissions row' AS FailureType, rolePermission.AspNetRoleId AS MissingValue
FROM dbo.RolePermissions rolePermission
INNER JOIN dbo.AspNetRoles roleTarget
    ON roleTarget.Id = rolePermission.AspNetRoleId
WHERE roleTarget.Name IN (SELECT RoleName FROM @ExpectedRoles)
  AND NOT EXISTS (SELECT 1 FROM dbo.Permissions permissionTarget WHERE permissionTarget.Id = rolePermission.PermissionId);

SELECT N'HROD approval limit missing' AS FailureType, N'maria.delacruz@unilab.com.ph' AS MissingValue
WHERE NOT EXISTS
(
    SELECT 1
    FROM dbo.JobOfferUsers jobUser
    INNER JOIN dbo.UserAttributes attribute
        ON attribute.JobOfferUserId = jobUser.Id
    INNER JOIN dbo.UserApprovalLimits approvalLimit
        ON approvalLimit.JobOfferUserId = jobUser.Id
    WHERE jobUser.Email = N'maria.delacruz@unilab.com.ph'
      AND attribute.AttributeName = N'ApprovalLimit'
      AND attribute.AttributeValue = N'100000'
      AND approvalLimit.MaxSalary = 100000
);

SELECT N'Division Head scope missing' AS FailureType, N'victor.lim@unilab.com.ph' AS MissingValue
WHERE NOT EXISTS
(
    SELECT 1
    FROM dbo.JobOfferUsers jobUser
    INNER JOIN dbo.UserDivisionAccess accessRow
        ON accessRow.JobOfferUserId = jobUser.Id
    INNER JOIN dbo.Divisions division
        ON division.Id = accessRow.DivisionId
    WHERE jobUser.Email = N'victor.lim@unilab.com.ph'
      AND division.DivisionCode = N'UNI-COM'
);

SELECT N'Division Head scope missing' AS FailureType, N'grace.tan@amherst.com.ph' AS MissingValue
WHERE NOT EXISTS
(
    SELECT 1
    FROM dbo.JobOfferUsers jobUser
    INNER JOIN dbo.UserDivisionAccess accessRow
        ON accessRow.JobOfferUserId = jobUser.Id
    INNER JOIN dbo.Divisions division
        ON division.Id = accessRow.DivisionId
    WHERE jobUser.Email = N'grace.tan@amherst.com.ph'
      AND division.DivisionCode = N'AMH-MFG'
);

SELECT N'President above-band attribute missing' AS FailureType, N'enrique.sy@unilab.com.ph' AS MissingValue
WHERE NOT EXISTS
(
    SELECT 1
    FROM dbo.JobOfferUsers jobUser
    INNER JOIN dbo.UserAttributes attribute
        ON attribute.JobOfferUserId = jobUser.Id
    WHERE jobUser.Email = N'enrique.sy@unilab.com.ph'
      AND attribute.AttributeName = N'CanApproveAboveBand'
      AND attribute.AttributeValue = N'true'
);

SELECT N'TR permission missing' AS FailureType, N'nina.garcia@unilab.com.ph' AS MissingValue
WHERE NOT EXISTS
(
    SELECT 1
    FROM dbo.AspNetUsers identityUser
    INNER JOIN dbo.AspNetUserRoles userRole
        ON userRole.UserId = identityUser.Id
    INNER JOIN dbo.RolePermissions rolePermission
        ON rolePermission.AspNetRoleId = userRole.RoleId
    INNER JOIN dbo.Permissions permission
        ON permission.Id = rolePermission.PermissionId
    WHERE identityUser.Email = N'nina.garcia@unilab.com.ph'
      AND permission.PermissionCode IN (N'JO.VIEW_ABOVE_BAND_REPORT', N'JO.MAINTAIN_SALARY_MATRIX')
    GROUP BY identityUser.Email
    HAVING COUNT(DISTINCT permission.PermissionCode) = 2
);

SELECT N'HRBP has approval permission' AS FailureType, N'ana.cruz@unilab.com.ph' AS MissingValue
WHERE EXISTS
(
    SELECT 1
    FROM dbo.AspNetUsers identityUser
    INNER JOIN dbo.AspNetUserRoles userRole
        ON userRole.UserId = identityUser.Id
    INNER JOIN dbo.RolePermissions rolePermission
        ON rolePermission.AspNetRoleId = userRole.RoleId
    INNER JOIN dbo.Permissions permission
        ON permission.Id = rolePermission.PermissionId
    WHERE identityUser.Email = N'ana.cruz@unilab.com.ph'
      AND permission.PermissionCode = N'JO.APPROVE'
);

SELECT N'Duplicate seeded role name' AS FailureType, roleTarget.Name AS MissingValue
FROM dbo.AspNetRoles roleTarget
WHERE roleTarget.Name IN (SELECT RoleName FROM @ExpectedRoles)
GROUP BY roleTarget.Name
HAVING COUNT(*) > 1;

SELECT N'Duplicate seeded identity email' AS FailureType, identityUser.Email AS MissingValue
FROM dbo.AspNetUsers identityUser
WHERE identityUser.Email IN (SELECT Email FROM @ExpectedUsers)
GROUP BY identityUser.Email
HAVING COUNT(*) > 1;

SELECT N'Duplicate seeded JobOfferUsers email' AS FailureType, jobUser.Email AS MissingValue
FROM dbo.JobOfferUsers jobUser
WHERE jobUser.Email IN (SELECT Email FROM @ExpectedUsers)
GROUP BY jobUser.Email
HAVING COUNT(*) > 1;

SELECT N'Duplicate seeded permission' AS FailureType, permission.PermissionCode AS MissingValue
FROM dbo.Permissions permission
WHERE permission.PermissionCode IN (SELECT PermissionCode FROM @ExpectedPermissions)
GROUP BY permission.PermissionCode
HAVING COUNT(*) > 1;

SELECT N'Duplicate seeded attribute' AS FailureType, jobUser.Email + N' / ' + attribute.AttributeName AS MissingValue
FROM dbo.JobOfferUsers jobUser
INNER JOIN dbo.UserAttributes attribute
    ON attribute.JobOfferUserId = jobUser.Id
WHERE jobUser.Email IN (SELECT Email FROM @ExpectedUsers)
GROUP BY jobUser.Email, attribute.AttributeName
HAVING COUNT(*) > 1;

SELECT N'Duplicate seeded division access' AS FailureType, jobUser.Email AS MissingValue
FROM dbo.JobOfferUsers jobUser
INNER JOIN dbo.UserDivisionAccess accessRow
    ON accessRow.JobOfferUserId = jobUser.Id
WHERE jobUser.Email IN (SELECT Email FROM @ExpectedUsers)
GROUP BY jobUser.Email, accessRow.CompanyId, accessRow.DivisionId
HAVING COUNT(*) > 1;

SELECT N'Duplicate seeded legal entity access' AS FailureType, jobUser.Email AS MissingValue
FROM dbo.JobOfferUsers jobUser
INNER JOIN dbo.UserLegalEntityAccess accessRow
    ON accessRow.JobOfferUserId = jobUser.Id
WHERE jobUser.Email IN (SELECT Email FROM @ExpectedUsers)
GROUP BY jobUser.Email, accessRow.LegalEntityId
HAVING COUNT(*) > 1;

PRINT 'Hybrid RBAC + ABAC sample data validation completed. Review result sets above; passing checks return zero rows.';
