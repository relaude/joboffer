/*
    Salary Matrix table design for Online Job Offer System
    Source context: "Online Job Offer System - User Stories V2.xlsx", sheet "02_User_Stories"
*/

CREATE TABLE dbo.SalaryMatrix (
    SalaryMatrixId       bigint IDENTITY(1,1) PRIMARY KEY,
    MatrixCode           varchar(50) NOT NULL,
    CompanyId            bigint NOT NULL,
    LegalEntityId        bigint NULL,
    MatrixName           nvarchar(200) NOT NULL,
    CurrencyCode         char(3) NOT NULL,
    EffectiveFrom        date NOT NULL,
    EffectiveTo          date NULL,
    VersionNo            int NOT NULL,
    ApprovalStatus       varchar(30) NOT NULL, -- Draft, PendingApproval, Approved, Expired, Archived
    IsActive             bit NOT NULL DEFAULT 0,
    ApprovedByUserId     bigint NULL,
    ApprovedAt           datetime2(0) NULL,
    CreatedByUserId      bigint NOT NULL,
    CreatedAt            datetime2(0) NOT NULL DEFAULT sysdatetime(),
    UpdatedByUserId      bigint NULL,
    UpdatedAt            datetime2(0) NULL,
    RowVersion           rowversion NOT NULL,

    CONSTRAINT UQ_SalaryMatrix_Version
        UNIQUE (CompanyId, LegalEntityId, MatrixCode, VersionNo),

    CONSTRAINT CK_SalaryMatrix_Dates
        CHECK (EffectiveTo IS NULL OR EffectiveTo >= EffectiveFrom),

    CONSTRAINT CK_SalaryMatrix_Status
        CHECK (ApprovalStatus IN ('Draft', 'PendingApproval', 'Approved', 'Expired', 'Archived'))
);

CREATE TABLE dbo.SalaryMatrixBand (
    SalaryMatrixBandId   bigint IDENTITY(1,1) PRIMARY KEY,
    SalaryMatrixId       bigint NOT NULL,
    JobLevelId           bigint NOT NULL,
    JobFamilyId          bigint NULL,
    PositionGrade        varchar(50) NULL,
    BandMinimum          decimal(18,2) NOT NULL,
    BandMidpoint         decimal(18,2) NOT NULL,
    BandMaximum          decimal(18,2) NOT NULL,
    BelowBandFlagEnabled bit NOT NULL DEFAULT 1,
    AboveBandFlagEnabled bit NOT NULL DEFAULT 1,
    CreatedAt            datetime2(0) NOT NULL DEFAULT sysdatetime(),
    UpdatedAt            datetime2(0) NULL,

    CONSTRAINT FK_SalaryMatrixBand_SalaryMatrix
        FOREIGN KEY (SalaryMatrixId)
        REFERENCES dbo.SalaryMatrix (SalaryMatrixId),

    CONSTRAINT UQ_SalaryMatrixBand_LevelFamily
        UNIQUE (SalaryMatrixId, JobLevelId, JobFamilyId, PositionGrade),

    CONSTRAINT CK_SalaryMatrixBand_Amounts
        CHECK (
            BandMinimum >= 0
            AND BandMidpoint >= BandMinimum
            AND BandMaximum >= BandMidpoint
        )
);

CREATE TABLE dbo.SalaryMatrixRule (
    SalaryMatrixRuleId    bigint IDENTITY(1,1) PRIMARY KEY,
    SalaryMatrixId        bigint NOT NULL,
    RuleType              varchar(50) NOT NULL, -- CompaRatio, BelowBand, AboveBand, PresidentEscalation
    ThresholdValue        decimal(18,6) NULL,
    ThresholdAmount       decimal(18,2) NULL,
    RequiresJustification bit NOT NULL DEFAULT 0,
    RequiresExtraApproval bit NOT NULL DEFAULT 0,
    ExtraApproverRole     varchar(100) NULL,
    IsActive              bit NOT NULL DEFAULT 1,
    CreatedAt             datetime2(0) NOT NULL DEFAULT sysdatetime(),

    CONSTRAINT FK_SalaryMatrixRule_SalaryMatrix
        FOREIGN KEY (SalaryMatrixId)
        REFERENCES dbo.SalaryMatrix (SalaryMatrixId),

    CONSTRAINT CK_SalaryMatrixRule_Type
        CHECK (RuleType IN ('CompaRatio', 'BelowBand', 'AboveBand', 'PresidentEscalation'))
);

CREATE INDEX IX_SalaryMatrix_Lookup
ON dbo.SalaryMatrix (
    CompanyId,
    LegalEntityId,
    ApprovalStatus,
    IsActive,
    EffectiveFrom,
    EffectiveTo
);

CREATE INDEX IX_SalaryMatrixBand_Lookup
ON dbo.SalaryMatrixBand (
    SalaryMatrixId,
    JobLevelId,
    JobFamilyId
);

