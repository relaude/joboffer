/*
    Hybrid RBAC + ABAC sample data for the Job Offer system.

    This script is intentionally idempotent. It can be run repeatedly against a
    development database without creating duplicate roles, users, permissions,
    policy rows, approval levels, attributes, or access grants.
*/

SET NOCOUNT ON;
SET XACT_ABORT ON;

BEGIN TRANSACTION;

DECLARE @Now DATETIME = GETDATE();

DECLARE @Roles TABLE
(
    RoleId NVARCHAR(450) NOT NULL,
    RoleName NVARCHAR(256) NOT NULL,
    NormalizedName NVARCHAR(256) NOT NULL,
    SortOrder INT NOT NULL
);

INSERT INTO @Roles (RoleId, RoleName, NormalizedName, SortOrder)
VALUES
    (N'00000000-0000-0000-0000-000000000101', N'System Administrator', N'SYSTEM ADMINISTRATOR', 10),
    (N'00000000-0000-0000-0000-000000000102', N'TA Partner', N'TA PARTNER', 20),
    (N'00000000-0000-0000-0000-000000000103', N'HROD Head', N'HROD HEAD', 30),
    (N'00000000-0000-0000-0000-000000000104', N'Division Head', N'DIVISION HEAD', 40),
    (N'00000000-0000-0000-0000-000000000105', N'President', N'PRESIDENT', 50),
    (N'00000000-0000-0000-0000-000000000106', N'Total Rewards Partner', N'TOTAL REWARDS PARTNER', 60),
    (N'00000000-0000-0000-0000-000000000107', N'HRBP', N'HRBP', 70),
    (N'00000000-0000-0000-0000-000000000108', N'System Auditor', N'SYSTEM AUDITOR', 80);

INSERT INTO dbo.AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp)
SELECT RoleId, RoleName, NormalizedName, RoleId
FROM @Roles source
WHERE NOT EXISTS
(
    SELECT 1
    FROM dbo.AspNetRoles target
    WHERE target.Id = source.RoleId
       OR target.NormalizedName = source.NormalizedName
);

DECLARE @Companies TABLE
(
    CompanyCode NVARCHAR(50) NOT NULL,
    CompanyName NVARCHAR(100) NOT NULL
);

INSERT INTO @Companies (CompanyCode, CompanyName)
VALUES
    (N'UNI', N'Unilab Inc.'),
    (N'AMH', N'Amherst Laboratories'),
    (N'ALI', N'Allied Services'),
    (N'CORP', N'Corporate Office');

INSERT INTO dbo.Companies (CompanyCode, CompanyName)
SELECT CompanyCode, CompanyName
FROM @Companies source
WHERE NOT EXISTS
(
    SELECT 1
    FROM dbo.Companies target
    WHERE target.CompanyCode = source.CompanyCode
);

DECLARE @Divisions TABLE
(
    DivisionCode NVARCHAR(50) NOT NULL,
    DivisionName NVARCHAR(100) NOT NULL,
    CompanyCode NVARCHAR(50) NOT NULL
);

INSERT INTO @Divisions (DivisionCode, DivisionName, CompanyCode)
VALUES
    (N'UNI-COM', N'Commercial Division', N'UNI'),
    (N'UNI-HR', N'Human Resources Division', N'UNI'),
    (N'AMH-MFG', N'Manufacturing Division', N'AMH'),
    (N'ALI-SVC', N'Shared Services Division', N'ALI'),
    (N'CORP-EXEC', N'Executive Office', N'CORP');

INSERT INTO dbo.Divisions (DivisionCode, DivisionName, CompanyId)
SELECT source.DivisionCode, source.DivisionName, company.Id
FROM @Divisions source
INNER JOIN dbo.Companies company
    ON company.CompanyCode = source.CompanyCode
WHERE NOT EXISTS
(
    SELECT 1
    FROM dbo.Divisions target
    WHERE target.DivisionCode = source.DivisionCode
);

DECLARE @Departments TABLE
(
    DepartmentName NVARCHAR(100) NOT NULL,
    DivisionCode NVARCHAR(50) NOT NULL
);

INSERT INTO @Departments (DepartmentName, DivisionCode)
VALUES
    (N'Sales', N'UNI-COM'),
    (N'Marketing', N'UNI-COM'),
    (N'Talent Acquisition', N'UNI-HR'),
    (N'Total Rewards', N'UNI-HR'),
    (N'Plant Operations', N'AMH-MFG'),
    (N'Quality Assurance', N'AMH-MFG'),
    (N'HR Business Partnering', N'ALI-SVC'),
    (N'Executive Review', N'CORP-EXEC');

INSERT INTO dbo.Departments (DepartmentName, Division_Id)
SELECT source.DepartmentName, division.Id
FROM @Departments source
INNER JOIN dbo.Divisions division
    ON division.DivisionCode = source.DivisionCode
WHERE NOT EXISTS
(
    SELECT 1
    FROM dbo.Departments target
    WHERE target.DepartmentName = source.DepartmentName
      AND ISNULL(target.Division_Id, 0) = division.Id
);

DECLARE @LegalEntities TABLE
(
    LegalEntityCode NVARCHAR(50) NOT NULL,
    LegalEntityName NVARCHAR(50) NOT NULL,
    CompanyCode NVARCHAR(50) NOT NULL
);

INSERT INTO @LegalEntities (LegalEntityCode, LegalEntityName, CompanyCode)
VALUES
    (N'UL-PH', N'Unilab Philippines', N'UNI'),
    (N'UL-CH', N'UL Consumer Health', N'UNI'),
    (N'AMH-MFG', N'Amherst Manufacturing', N'AMH'),
    (N'AMH-DIST', N'Amherst Distribution', N'AMH'),
    (N'ALI-SVC', N'Allied Services', N'ALI'),
    (N'CORP-HQ', N'Corporate Headquarters', N'CORP');

INSERT INTO dbo.LegalEntities (CompanyId, LegalEntityCode, LegalEntityName)
SELECT company.Id, source.LegalEntityCode, source.LegalEntityName
FROM @LegalEntities source
INNER JOIN dbo.Companies company
    ON company.CompanyCode = source.CompanyCode
WHERE NOT EXISTS
(
    SELECT 1
    FROM dbo.LegalEntities target
    WHERE target.LegalEntityCode = source.LegalEntityCode
);

DECLARE @Permissions TABLE
(
    PermissionCode NVARCHAR(50) NOT NULL,
    PermissionName NVARCHAR(50) NOT NULL
);

INSERT INTO @Permissions (PermissionCode, PermissionName)
VALUES
    (N'JO.CREATE', N'Create Job Offer'),
    (N'JO.EDIT', N'Edit Job Offer'),
    (N'JO.ANALYZE', N'Analyze Offer'),
    (N'JO.SUBMIT_APPROVAL', N'Submit Approval'),
    (N'JO.APPROVE', N'Approve Offer'),
    (N'JO.REJECT', N'Reject Offer'),
    (N'JO.RETURN', N'Return Offer'),
    (N'JO.GENERATE_OFFER', N'Generate Offer'),
    (N'JO.VIEW_DIVISION_STATUS', N'View Division Status'),
    (N'JO.VIEW_ABOVE_BAND_REPORT', N'Above Band Report'),
    (N'JO.MAINTAIN_SALARY_MATRIX', N'Maintain Salary Matrix'),
    (N'SECURITY.MANAGE_USERS', N'Manage Users'),
    (N'SECURITY.VIEW_AUDIT', N'View Audit');

INSERT INTO dbo.Permissions (PermissionCode, PermissionName)
SELECT PermissionCode, PermissionName
FROM @Permissions source
WHERE NOT EXISTS
(
    SELECT 1
    FROM dbo.Permissions target
    WHERE target.PermissionCode = source.PermissionCode
);

DECLARE @Users TABLE
(
    UserId NVARCHAR(450) NOT NULL,
    Email NVARCHAR(256) NOT NULL,
    FullName NVARCHAR(50) NOT NULL,
    RoleName NVARCHAR(256) NOT NULL
);

INSERT INTO @Users (UserId, Email, FullName, RoleName)
VALUES
    (N'10000000-0000-0000-0000-000000000201', N'paula.santos@unilab.com.ph', N'Paula Santos', N'TA Partner'),
    (N'10000000-0000-0000-0000-000000000202', N'carlo.reyes@amherst.com.ph', N'Carlo Reyes', N'TA Partner'),
    (N'10000000-0000-0000-0000-000000000203', N'maria.delacruz@unilab.com.ph', N'Maria Dela Cruz', N'HROD Head'),
    (N'10000000-0000-0000-0000-000000000204', N'victor.lim@unilab.com.ph', N'Victor Lim', N'Division Head'),
    (N'10000000-0000-0000-0000-000000000205', N'grace.tan@amherst.com.ph', N'Grace Tan', N'Division Head'),
    (N'10000000-0000-0000-0000-000000000206', N'enrique.sy@unilab.com.ph', N'Enrique Sy', N'President'),
    (N'10000000-0000-0000-0000-000000000207', N'nina.garcia@unilab.com.ph', N'Nina Garcia', N'Total Rewards Partner'),
    (N'10000000-0000-0000-0000-000000000208', N'ana.cruz@unilab.com.ph', N'Ana Cruz', N'HRBP'),
    (N'10000000-0000-0000-0000-000000000209', N'allan.yu@unilab.com.ph', N'Allan Yu', N'System Auditor'),
    (N'10000000-0000-0000-0000-000000000210', N'admin.security@unilab.com.ph', N'Admin Security', N'System Administrator');

INSERT INTO dbo.AspNetUsers
(
    Id,
    UserName,
    NormalizedUserName,
    Email,
    NormalizedEmail,
    EmailConfirmed,
    PasswordHash,
    SecurityStamp,
    ConcurrencyStamp,
    PhoneNumber,
    PhoneNumberConfirmed,
    TwoFactorEnabled,
    LockoutEnd,
    LockoutEnabled,
    AccessFailedCount
)
SELECT
    UserId,
    Email,
    UPPER(Email),
    Email,
    UPPER(Email),
    1,
    NULL,
    UserId,
    UserId,
    NULL,
    0,
    0,
    NULL,
    1,
    0
FROM @Users source
WHERE NOT EXISTS
(
    SELECT 1
    FROM dbo.AspNetUsers target
    WHERE target.Id = source.UserId
       OR target.NormalizedUserName = UPPER(source.Email)
);

INSERT INTO dbo.AspNetUserRoles (UserId, RoleId)
SELECT identityUser.Id, roleTarget.Id
FROM @Users users
INNER JOIN dbo.AspNetUsers identityUser
    ON identityUser.NormalizedUserName = UPPER(users.Email)
INNER JOIN @Roles roles
    ON roles.RoleName = users.RoleName
INNER JOIN dbo.AspNetRoles roleTarget
    ON roleTarget.NormalizedName = roles.NormalizedName
WHERE NOT EXISTS
(
    SELECT 1
    FROM dbo.AspNetUserRoles target
    WHERE target.UserId = identityUser.Id
      AND target.RoleId = roleTarget.Id
);

INSERT INTO dbo.JobOfferUsers (AspNetUserId, Name, Email, IsActive, CreatedAt, CreatedBy, ModifiedAt, ModifiedBy)
SELECT identityUser.Id, source.FullName, source.Email, 1, @Now, NULL, NULL, NULL
FROM @Users source
INNER JOIN dbo.AspNetUsers identityUser
    ON identityUser.NormalizedUserName = UPPER(source.Email)
WHERE NOT EXISTS
(
    SELECT 1
    FROM dbo.JobOfferUsers target
    WHERE target.AspNetUserId = identityUser.Id
       OR target.Email = source.Email
);

DECLARE @RolePermissions TABLE
(
    RoleName NVARCHAR(256) NOT NULL,
    PermissionCode NVARCHAR(50) NOT NULL
);

INSERT INTO @RolePermissions (RoleName, PermissionCode)
VALUES
    (N'TA Partner', N'JO.CREATE'),
    (N'TA Partner', N'JO.EDIT'),
    (N'TA Partner', N'JO.ANALYZE'),
    (N'TA Partner', N'JO.SUBMIT_APPROVAL'),
    (N'TA Partner', N'JO.GENERATE_OFFER'),
    (N'HROD Head', N'JO.APPROVE'),
    (N'HROD Head', N'JO.REJECT'),
    (N'HROD Head', N'JO.RETURN'),
    (N'Division Head', N'JO.APPROVE'),
    (N'Division Head', N'JO.REJECT'),
    (N'Division Head', N'JO.RETURN'),
    (N'President', N'JO.APPROVE'),
    (N'President', N'JO.REJECT'),
    (N'President', N'JO.RETURN'),
    (N'Total Rewards Partner', N'JO.VIEW_ABOVE_BAND_REPORT'),
    (N'Total Rewards Partner', N'JO.MAINTAIN_SALARY_MATRIX'),
    (N'HRBP', N'JO.VIEW_DIVISION_STATUS'),
    (N'System Auditor', N'SECURITY.VIEW_AUDIT'),
    (N'System Administrator', N'JO.CREATE'),
    (N'System Administrator', N'JO.EDIT'),
    (N'System Administrator', N'JO.ANALYZE'),
    (N'System Administrator', N'JO.SUBMIT_APPROVAL'),
    (N'System Administrator', N'JO.APPROVE'),
    (N'System Administrator', N'JO.REJECT'),
    (N'System Administrator', N'JO.RETURN'),
    (N'System Administrator', N'JO.GENERATE_OFFER'),
    (N'System Administrator', N'JO.VIEW_DIVISION_STATUS'),
    (N'System Administrator', N'JO.VIEW_ABOVE_BAND_REPORT'),
    (N'System Administrator', N'JO.MAINTAIN_SALARY_MATRIX'),
    (N'System Administrator', N'SECURITY.MANAGE_USERS'),
    (N'System Administrator', N'SECURITY.VIEW_AUDIT');

INSERT INTO dbo.RolePermissions (AspNetRoleId, PermissionId)
SELECT roleTarget.Id, permission.Id
FROM @RolePermissions source
INNER JOIN @Roles roleMap
    ON roleMap.RoleName = source.RoleName
INNER JOIN dbo.AspNetRoles roleTarget
    ON roleTarget.NormalizedName = roleMap.NormalizedName
INNER JOIN dbo.Permissions permission
    ON permission.PermissionCode = source.PermissionCode
WHERE NOT EXISTS
(
    SELECT 1
    FROM dbo.RolePermissions target
    WHERE target.AspNetRoleId = roleTarget.Id
      AND target.PermissionId = permission.Id
);

DECLARE @ApprovalLevels TABLE
(
    SequenceNo INT NOT NULL,
    ApprovalRole NVARCHAR(50) NOT NULL,
    IsAboveBandOnly BIT NOT NULL
);

INSERT INTO @ApprovalLevels (SequenceNo, ApprovalRole, IsAboveBandOnly)
VALUES
    (1, N'HROD Head', 0),
    (2, N'Division Head', 0),
    (3, N'President', 1);

INSERT INTO dbo.ApprovalLevels (SequenceNo, ApprovalRole, IsAboveBandOnly)
SELECT SequenceNo, ApprovalRole, IsAboveBandOnly
FROM @ApprovalLevels source
WHERE NOT EXISTS
(
    SELECT 1
    FROM dbo.ApprovalLevels target
    WHERE target.ApprovalRole = source.ApprovalRole
);

DECLARE @Policies TABLE
(
    PolicyName NVARCHAR(50) NOT NULL,
    PermissionCode NVARCHAR(50) NOT NULL,
    Expression NVARCHAR(50) NOT NULL
);

INSERT INTO @Policies (PolicyName, PermissionCode, Expression)
VALUES
    (N'TA Create Own Division', N'JO.CREATE', N'ROLE:TA;DIV'),
    (N'TA Analyze Own Division', N'JO.ANALYZE', N'ROLE:TA;DIV'),
    (N'HROD Company Limit Approval', N'JO.APPROVE', N'ROLE:HROD;CO;LIMIT'),
    (N'Division Head Limit Approval', N'JO.APPROVE', N'ROLE:DIVHEAD;DIV;LIMIT'),
    (N'President Above Band Approval', N'JO.APPROVE', N'ROLE:PRES;ABOVE'),
    (N'TR Above Band Report', N'JO.VIEW_ABOVE_BAND_REPORT', N'ROLE:TR;ABOVE'),
    (N'HRBP Division Visibility', N'JO.VIEW_DIVISION_STATUS', N'ROLE:HRBP;DIV'),
    (N'Auditor Security Audit', N'SECURITY.VIEW_AUDIT', N'ROLE:AUDIT;READ'),
    (N'Admin Security Management', N'SECURITY.MANAGE_USERS', N'ROLE:ADMIN;ALL');

INSERT INTO dbo.AuthorizationPolicies (PolicyName, PermissionId, PermissionCode, Expression, IsActive)
SELECT source.PolicyName, permission.Id, source.PermissionCode, source.Expression, 1
FROM @Policies source
INNER JOIN dbo.Permissions permission
    ON permission.PermissionCode = source.PermissionCode
WHERE NOT EXISTS
(
    SELECT 1
    FROM dbo.AuthorizationPolicies target
    WHERE target.PolicyName = source.PolicyName
);

DECLARE @Attributes TABLE
(
    Email NVARCHAR(256) NOT NULL,
    AttributeName NVARCHAR(50) NOT NULL,
    AttributeValue NVARCHAR(50) NOT NULL
);

INSERT INTO @Attributes (Email, AttributeName, AttributeValue)
VALUES
    (N'paula.santos@unilab.com.ph', N'Department', N'Sales'),
    (N'paula.santos@unilab.com.ph', N'BusinessUnit', N'Commercial'),
    (N'paula.santos@unilab.com.ph', N'Region', N'NCR'),
    (N'paula.santos@unilab.com.ph', N'Location', N'Manila'),
    (N'paula.santos@unilab.com.ph', N'JobLevel', N'Specialist'),
    (N'paula.santos@unilab.com.ph', N'ApprovalLimit', N'0'),
    (N'paula.santos@unilab.com.ph', N'CanApproveAboveBand', N'false'),
    (N'paula.santos@unilab.com.ph', N'CanMaintainSalaryMatrix', N'false'),
    (N'paula.santos@unilab.com.ph', N'CanViewAboveBandReport', N'false'),
    (N'carlo.reyes@amherst.com.ph', N'Department', N'Plant Operations'),
    (N'carlo.reyes@amherst.com.ph', N'BusinessUnit', N'Manufacturing'),
    (N'carlo.reyes@amherst.com.ph', N'Region', N'Laguna'),
    (N'carlo.reyes@amherst.com.ph', N'Location', N'Binan'),
    (N'carlo.reyes@amherst.com.ph', N'JobLevel', N'Specialist'),
    (N'carlo.reyes@amherst.com.ph', N'ApprovalLimit', N'0'),
    (N'carlo.reyes@amherst.com.ph', N'CanApproveAboveBand', N'false'),
    (N'carlo.reyes@amherst.com.ph', N'CanMaintainSalaryMatrix', N'false'),
    (N'carlo.reyes@amherst.com.ph', N'CanViewAboveBandReport', N'false'),
    (N'maria.delacruz@unilab.com.ph', N'Department', N'Talent Acquisition'),
    (N'maria.delacruz@unilab.com.ph', N'BusinessUnit', N'Human Resources'),
    (N'maria.delacruz@unilab.com.ph', N'Region', N'NCR'),
    (N'maria.delacruz@unilab.com.ph', N'Location', N'Mandaluyong'),
    (N'maria.delacruz@unilab.com.ph', N'JobLevel', N'Director'),
    (N'maria.delacruz@unilab.com.ph', N'ApprovalLimit', N'100000'),
    (N'maria.delacruz@unilab.com.ph', N'CanApproveAboveBand', N'false'),
    (N'maria.delacruz@unilab.com.ph', N'CanMaintainSalaryMatrix', N'false'),
    (N'maria.delacruz@unilab.com.ph', N'CanViewAboveBandReport', N'false'),
    (N'victor.lim@unilab.com.ph', N'Department', N'Sales'),
    (N'victor.lim@unilab.com.ph', N'BusinessUnit', N'Commercial'),
    (N'victor.lim@unilab.com.ph', N'Region', N'NCR'),
    (N'victor.lim@unilab.com.ph', N'Location', N'Manila'),
    (N'victor.lim@unilab.com.ph', N'JobLevel', N'VP'),
    (N'victor.lim@unilab.com.ph', N'ApprovalLimit', N'250000'),
    (N'victor.lim@unilab.com.ph', N'CanApproveAboveBand', N'false'),
    (N'victor.lim@unilab.com.ph', N'CanMaintainSalaryMatrix', N'false'),
    (N'victor.lim@unilab.com.ph', N'CanViewAboveBandReport', N'false'),
    (N'grace.tan@amherst.com.ph', N'Department', N'Plant Operations'),
    (N'grace.tan@amherst.com.ph', N'BusinessUnit', N'Manufacturing'),
    (N'grace.tan@amherst.com.ph', N'Region', N'Laguna'),
    (N'grace.tan@amherst.com.ph', N'Location', N'Binan'),
    (N'grace.tan@amherst.com.ph', N'JobLevel', N'VP'),
    (N'grace.tan@amherst.com.ph', N'ApprovalLimit', N'250000'),
    (N'grace.tan@amherst.com.ph', N'CanApproveAboveBand', N'false'),
    (N'grace.tan@amherst.com.ph', N'CanMaintainSalaryMatrix', N'false'),
    (N'grace.tan@amherst.com.ph', N'CanViewAboveBandReport', N'false'),
    (N'enrique.sy@unilab.com.ph', N'Department', N'Executive Review'),
    (N'enrique.sy@unilab.com.ph', N'BusinessUnit', N'Corporate'),
    (N'enrique.sy@unilab.com.ph', N'Region', N'NCR'),
    (N'enrique.sy@unilab.com.ph', N'Location', N'Mandaluyong'),
    (N'enrique.sy@unilab.com.ph', N'JobLevel', N'President'),
    (N'enrique.sy@unilab.com.ph', N'ApprovalLimit', N'999999999'),
    (N'enrique.sy@unilab.com.ph', N'CanApproveAboveBand', N'true'),
    (N'enrique.sy@unilab.com.ph', N'CanMaintainSalaryMatrix', N'false'),
    (N'enrique.sy@unilab.com.ph', N'CanViewAboveBandReport', N'false'),
    (N'nina.garcia@unilab.com.ph', N'Department', N'Total Rewards'),
    (N'nina.garcia@unilab.com.ph', N'BusinessUnit', N'Human Resources'),
    (N'nina.garcia@unilab.com.ph', N'Region', N'NCR'),
    (N'nina.garcia@unilab.com.ph', N'Location', N'Mandaluyong'),
    (N'nina.garcia@unilab.com.ph', N'JobLevel', N'Manager'),
    (N'nina.garcia@unilab.com.ph', N'ApprovalLimit', N'0'),
    (N'nina.garcia@unilab.com.ph', N'CanApproveAboveBand', N'false'),
    (N'nina.garcia@unilab.com.ph', N'CanMaintainSalaryMatrix', N'true'),
    (N'nina.garcia@unilab.com.ph', N'CanViewAboveBandReport', N'true'),
    (N'ana.cruz@unilab.com.ph', N'Department', N'HR Business Partnering'),
    (N'ana.cruz@unilab.com.ph', N'BusinessUnit', N'Shared Services'),
    (N'ana.cruz@unilab.com.ph', N'Region', N'NCR'),
    (N'ana.cruz@unilab.com.ph', N'Location', N'Mandaluyong'),
    (N'ana.cruz@unilab.com.ph', N'JobLevel', N'Manager'),
    (N'ana.cruz@unilab.com.ph', N'ApprovalLimit', N'0'),
    (N'ana.cruz@unilab.com.ph', N'CanApproveAboveBand', N'false'),
    (N'ana.cruz@unilab.com.ph', N'CanMaintainSalaryMatrix', N'false'),
    (N'ana.cruz@unilab.com.ph', N'CanViewAboveBandReport', N'false'),
    (N'allan.yu@unilab.com.ph', N'Department', N'Internal Audit'),
    (N'allan.yu@unilab.com.ph', N'BusinessUnit', N'Corporate'),
    (N'allan.yu@unilab.com.ph', N'Region', N'NCR'),
    (N'allan.yu@unilab.com.ph', N'Location', N'Mandaluyong'),
    (N'allan.yu@unilab.com.ph', N'JobLevel', N'Manager'),
    (N'allan.yu@unilab.com.ph', N'ApprovalLimit', N'0'),
    (N'allan.yu@unilab.com.ph', N'CanApproveAboveBand', N'false'),
    (N'allan.yu@unilab.com.ph', N'CanMaintainSalaryMatrix', N'false'),
    (N'allan.yu@unilab.com.ph', N'CanViewAboveBandReport', N'false'),
    (N'admin.security@unilab.com.ph', N'Department', N'IT Security'),
    (N'admin.security@unilab.com.ph', N'BusinessUnit', N'Corporate'),
    (N'admin.security@unilab.com.ph', N'Region', N'NCR'),
    (N'admin.security@unilab.com.ph', N'Location', N'Mandaluyong'),
    (N'admin.security@unilab.com.ph', N'JobLevel', N'Administrator'),
    (N'admin.security@unilab.com.ph', N'ApprovalLimit', N'999999999'),
    (N'admin.security@unilab.com.ph', N'CanApproveAboveBand', N'true'),
    (N'admin.security@unilab.com.ph', N'CanMaintainSalaryMatrix', N'true'),
    (N'admin.security@unilab.com.ph', N'CanViewAboveBandReport', N'true');

INSERT INTO dbo.UserAttributes (JobOfferUserId, AttributeName, AttributeValue)
SELECT jobUser.Id, source.AttributeName, source.AttributeValue
FROM @Attributes source
INNER JOIN dbo.JobOfferUsers jobUser
    ON jobUser.Email = source.Email
WHERE NOT EXISTS
(
    SELECT 1
    FROM dbo.UserAttributes target
    WHERE target.JobOfferUserId = jobUser.Id
      AND target.AttributeName = source.AttributeName
);

DECLARE @ApprovalLimits TABLE
(
    Email NVARCHAR(256) NOT NULL,
    MinSalary DECIMAL(18, 2) NOT NULL,
    MaxSalary DECIMAL(18, 2) NOT NULL
);

INSERT INTO @ApprovalLimits (Email, MinSalary, MaxSalary)
VALUES
    (N'paula.santos@unilab.com.ph', 0, 0),
    (N'carlo.reyes@amherst.com.ph', 0, 0),
    (N'maria.delacruz@unilab.com.ph', 0, 100000),
    (N'victor.lim@unilab.com.ph', 0, 250000),
    (N'grace.tan@amherst.com.ph', 0, 250000),
    (N'enrique.sy@unilab.com.ph', 0, 999999999),
    (N'nina.garcia@unilab.com.ph', 0, 0),
    (N'ana.cruz@unilab.com.ph', 0, 0),
    (N'allan.yu@unilab.com.ph', 0, 0),
    (N'admin.security@unilab.com.ph', 0, 999999999);

INSERT INTO dbo.UserApprovalLimits (JobOfferUserId, MinSalary, MaxSalary)
SELECT jobUser.Id, source.MinSalary, source.MaxSalary
FROM @ApprovalLimits source
INNER JOIN dbo.JobOfferUsers jobUser
    ON jobUser.Email = source.Email
WHERE NOT EXISTS
(
    SELECT 1
    FROM dbo.UserApprovalLimits target
    WHERE target.JobOfferUserId = jobUser.Id
);

DECLARE @DivisionAccess TABLE
(
    Email NVARCHAR(256) NOT NULL,
    CompanyCode NVARCHAR(50) NOT NULL,
    DivisionCode NVARCHAR(50) NOT NULL
);

INSERT INTO @DivisionAccess (Email, CompanyCode, DivisionCode)
VALUES
    (N'paula.santos@unilab.com.ph', N'UNI', N'UNI-COM'),
    (N'carlo.reyes@amherst.com.ph', N'AMH', N'AMH-MFG'),
    (N'maria.delacruz@unilab.com.ph', N'UNI', N'UNI-HR'),
    (N'maria.delacruz@unilab.com.ph', N'UNI', N'UNI-COM'),
    (N'maria.delacruz@unilab.com.ph', N'ALI', N'ALI-SVC'),
    (N'victor.lim@unilab.com.ph', N'UNI', N'UNI-COM'),
    (N'grace.tan@amherst.com.ph', N'AMH', N'AMH-MFG'),
    (N'enrique.sy@unilab.com.ph', N'CORP', N'CORP-EXEC'),
    (N'enrique.sy@unilab.com.ph', N'UNI', N'UNI-COM'),
    (N'enrique.sy@unilab.com.ph', N'AMH', N'AMH-MFG'),
    (N'nina.garcia@unilab.com.ph', N'UNI', N'UNI-HR'),
    (N'nina.garcia@unilab.com.ph', N'UNI', N'UNI-COM'),
    (N'nina.garcia@unilab.com.ph', N'AMH', N'AMH-MFG'),
    (N'ana.cruz@unilab.com.ph', N'UNI', N'UNI-COM'),
    (N'ana.cruz@unilab.com.ph', N'ALI', N'ALI-SVC'),
    (N'allan.yu@unilab.com.ph', N'UNI', N'UNI-COM'),
    (N'allan.yu@unilab.com.ph', N'AMH', N'AMH-MFG'),
    (N'allan.yu@unilab.com.ph', N'ALI', N'ALI-SVC'),
    (N'admin.security@unilab.com.ph', N'UNI', N'UNI-COM'),
    (N'admin.security@unilab.com.ph', N'UNI', N'UNI-HR'),
    (N'admin.security@unilab.com.ph', N'AMH', N'AMH-MFG'),
    (N'admin.security@unilab.com.ph', N'ALI', N'ALI-SVC'),
    (N'admin.security@unilab.com.ph', N'CORP', N'CORP-EXEC');

INSERT INTO dbo.UserDivisionAccess (JobOfferUserId, CompanyId, DivisionId)
SELECT jobUser.Id, company.Id, division.Id
FROM @DivisionAccess source
INNER JOIN dbo.JobOfferUsers jobUser
    ON jobUser.Email = source.Email
INNER JOIN dbo.Companies company
    ON company.CompanyCode = source.CompanyCode
INNER JOIN dbo.Divisions division
    ON division.DivisionCode = source.DivisionCode
WHERE NOT EXISTS
(
    SELECT 1
    FROM dbo.UserDivisionAccess target
    WHERE target.JobOfferUserId = jobUser.Id
      AND target.CompanyId = company.Id
      AND target.DivisionId = division.Id
);

DECLARE @LegalEntityAccess TABLE
(
    Email NVARCHAR(256) NOT NULL,
    LegalEntityCode NVARCHAR(50) NOT NULL
);

INSERT INTO @LegalEntityAccess (Email, LegalEntityCode)
VALUES
    (N'paula.santos@unilab.com.ph', N'UL-PH'),
    (N'paula.santos@unilab.com.ph', N'UL-CH'),
    (N'carlo.reyes@amherst.com.ph', N'AMH-MFG'),
    (N'carlo.reyes@amherst.com.ph', N'AMH-DIST'),
    (N'maria.delacruz@unilab.com.ph', N'UL-PH'),
    (N'maria.delacruz@unilab.com.ph', N'UL-CH'),
    (N'maria.delacruz@unilab.com.ph', N'ALI-SVC'),
    (N'victor.lim@unilab.com.ph', N'UL-PH'),
    (N'victor.lim@unilab.com.ph', N'UL-CH'),
    (N'grace.tan@amherst.com.ph', N'AMH-MFG'),
    (N'grace.tan@amherst.com.ph', N'AMH-DIST'),
    (N'enrique.sy@unilab.com.ph', N'CORP-HQ'),
    (N'enrique.sy@unilab.com.ph', N'UL-PH'),
    (N'enrique.sy@unilab.com.ph', N'AMH-MFG'),
    (N'nina.garcia@unilab.com.ph', N'UL-PH'),
    (N'nina.garcia@unilab.com.ph', N'AMH-MFG'),
    (N'ana.cruz@unilab.com.ph', N'UL-PH'),
    (N'ana.cruz@unilab.com.ph', N'ALI-SVC'),
    (N'allan.yu@unilab.com.ph', N'UL-PH'),
    (N'allan.yu@unilab.com.ph', N'AMH-MFG'),
    (N'allan.yu@unilab.com.ph', N'ALI-SVC'),
    (N'admin.security@unilab.com.ph', N'UL-PH'),
    (N'admin.security@unilab.com.ph', N'UL-CH'),
    (N'admin.security@unilab.com.ph', N'AMH-MFG'),
    (N'admin.security@unilab.com.ph', N'AMH-DIST'),
    (N'admin.security@unilab.com.ph', N'ALI-SVC'),
    (N'admin.security@unilab.com.ph', N'CORP-HQ');

;WITH MissingLegalEntityAccess AS
(
    SELECT
        jobUser.Id AS JobOfferUserId,
        legalEntity.Id AS LegalEntityId,
        ROW_NUMBER() OVER (ORDER BY jobUser.Id, legalEntity.Id) AS RowNo
    FROM @LegalEntityAccess source
    INNER JOIN dbo.JobOfferUsers jobUser
        ON jobUser.Email = source.Email
    INNER JOIN dbo.LegalEntities legalEntity
        ON legalEntity.LegalEntityCode = source.LegalEntityCode
    WHERE NOT EXISTS
    (
        SELECT 1
        FROM dbo.UserLegalEntityAccess target
        WHERE target.JobOfferUserId = jobUser.Id
          AND target.LegalEntityId = legalEntity.Id
    )
)
INSERT INTO dbo.UserLegalEntityAccess (Id, JobOfferUserId, LegalEntityId)
SELECT
    ISNULL((SELECT MAX(Id) FROM dbo.UserLegalEntityAccess), 0) + RowNo,
    JobOfferUserId,
    LegalEntityId
FROM MissingLegalEntityAccess;

COMMIT TRANSACTION;

PRINT 'Hybrid RBAC + ABAC sample data seed completed.';
