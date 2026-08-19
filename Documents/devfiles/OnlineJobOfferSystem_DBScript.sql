
/* =========================================================
   ONLINE JOB OFFER SYSTEM DATABASE SCRIPT
   Candidate, Application, Job Offer, and Salary Matrix
   ========================================================= */

CREATE TABLE Candidate (
    CandidateId BIGINT IDENTITY(1,1) PRIMARY KEY,
    CandidateNo VARCHAR(30) NOT NULL UNIQUE,
    FirstName VARCHAR(100) NOT NULL,
    MiddleName VARCHAR(100) NULL,
    LastName VARCHAR(100) NOT NULL,
    Suffix VARCHAR(20) NULL,
    PreferredName VARCHAR(100) NULL,
    Gender VARCHAR(20) NULL,
    CivilStatus VARCHAR(30) NULL,
    BirthDate DATE NULL,
    MobileNo VARCHAR(50) NULL,
    EmailAddress VARCHAR(255) NOT NULL,
    CurrentAddress NVARCHAR(500) NULL,
    ProvincialAddress NVARCHAR(500) NULL,
    CurrentEmployer VARCHAR(255) NULL,
    CurrentPosition VARCHAR(255) NULL,
    YearsOfExperience DECIMAL(5,2) NULL,
    SourceChannel VARCHAR(100) NULL,
    IsActive BIT NOT NULL DEFAULT(1),
    CreatedBy VARCHAR(100) NOT NULL,
    CreatedDate DATETIME2 NOT NULL DEFAULT(GETDATE()),
    ModifiedBy VARCHAR(100) NULL,
    ModifiedDate DATETIME2 NULL
);

CREATE TABLE CandidateApplication (
    ApplicationId BIGINT IDENTITY(1,1) PRIMARY KEY,
    CandidateId BIGINT NOT NULL,
    RequisitionId BIGINT NOT NULL,
    HiringCompanyId INT NOT NULL,
    LegalEntityId INT NOT NULL,
    ApplicationStatus VARCHAR(50) NOT NULL,
    CurrentWorkflowStatus VARCHAR(50) NOT NULL,
    ApplicationDate DATETIME2 NOT NULL,
    EndorsementDate DATETIME2 NULL,
    TAOwnerId INT NULL,
    CreatedDate DATETIME2 NOT NULL DEFAULT(GETDATE()),
    CONSTRAINT FK_Application_Candidate
        FOREIGN KEY (CandidateId) REFERENCES Candidate(CandidateId)
);

CREATE TABLE CandidateCompensationProfile (
    CompensationProfileId BIGINT IDENTITY(1,1) PRIMARY KEY,
    ApplicationId BIGINT NOT NULL,
    CurrentBaseSalary DECIMAL(18,2) NULL,
    CurrentAllowance DECIMAL(18,2) NULL,
    CurrentBonus DECIMAL(18,2) NULL,
    ExpectedSalary DECIMAL(18,2) NULL,
    OtherBenefits NVARCHAR(MAX) NULL,
    SubmissionReferenceNo VARCHAR(50) NOT NULL,
    SubmissionDate DATETIME2 NOT NULL,
    IsValidated BIT NOT NULL DEFAULT(0),
    ValidationDate DATETIME2 NULL,
    ValidatedBy VARCHAR(100) NULL,
    Remarks NVARCHAR(1000) NULL,
    CONSTRAINT FK_CompProfile_Application
        FOREIGN KEY (ApplicationId) REFERENCES CandidateApplication(ApplicationId)
);

CREATE TABLE CandidateDocument (
    DocumentId BIGINT IDENTITY(1,1) PRIMARY KEY,
    ApplicationId BIGINT NOT NULL,
    DocumentType VARCHAR(50) NOT NULL,
    FileName VARCHAR(255) NOT NULL,
    FilePath NVARCHAR(1000) NOT NULL,
    FileSize BIGINT NULL,
    ContentType VARCHAR(100) NULL,
    VersionNo INT NOT NULL DEFAULT(1),
    UploadedBy VARCHAR(100) NOT NULL,
    UploadedDate DATETIME2 NOT NULL,
    IsActive BIT NOT NULL DEFAULT(1),
    CONSTRAINT FK_Document_Application
        FOREIGN KEY (ApplicationId) REFERENCES CandidateApplication(ApplicationId)
);

CREATE TABLE JobOffer (
    JobOfferId BIGINT IDENTITY(1,1) PRIMARY KEY,
    OfferNo VARCHAR(30) NOT NULL UNIQUE,
    ApplicationId BIGINT NOT NULL,
    PositionId INT NOT NULL,
    DepartmentId INT NOT NULL,
    HiringManagerId INT NOT NULL,
    OfferStatus VARCHAR(50) NOT NULL,
    OfferVersion INT NOT NULL DEFAULT(1),
    ProposedStartDate DATE NULL,
    ExpirationDate DATE NULL,
    CreatedBy VARCHAR(100) NOT NULL,
    CreatedDate DATETIME2 NOT NULL DEFAULT(GETDATE()),
    ModifiedBy VARCHAR(100) NULL,
    ModifiedDate DATETIME2 NULL,
    CONSTRAINT FK_JobOffer_Application
        FOREIGN KEY (ApplicationId) REFERENCES CandidateApplication(ApplicationId)
);

CREATE TABLE JobOfferCompensation (
    CompensationId BIGINT IDENTITY(1,1) PRIMARY KEY,
    JobOfferId BIGINT NOT NULL,
    MonthlySalary DECIMAL(18,2) NOT NULL,
    AnnualSalary DECIMAL(18,2) NULL,
    RiceAllowance DECIMAL(18,2) NULL,
    TransportationAllowance DECIMAL(18,2) NULL,
    CommunicationAllowance DECIMAL(18,2) NULL,
    ClothingAllowance DECIMAL(18,2) NULL,
    SigningBonus DECIMAL(18,2) NULL,
    Remarks NVARCHAR(1000) NULL,
    CONSTRAINT FK_OfferComp_Offer
        FOREIGN KEY(JobOfferId) REFERENCES JobOffer(JobOfferId)
);

CREATE TABLE BenefitMaster (
    BenefitId INT IDENTITY(1,1) PRIMARY KEY,
    BenefitName VARCHAR(100) NOT NULL,
    IsActive BIT NOT NULL DEFAULT(1)
);

CREATE TABLE JobOfferBenefit (
    JobOfferBenefitId BIGINT IDENTITY(1,1) PRIMARY KEY,
    JobOfferId BIGINT NOT NULL,
    BenefitId INT NOT NULL,
    BenefitValue NVARCHAR(500) NULL,
    CONSTRAINT FK_OfferBenefit_Offer
        FOREIGN KEY(JobOfferId) REFERENCES JobOffer(JobOfferId),
    CONSTRAINT FK_OfferBenefit_Master
        FOREIGN KEY(BenefitId) REFERENCES BenefitMaster(BenefitId)
);

CREATE TABLE SalaryGrade (
    SalaryGradeId INT IDENTITY(1,1) PRIMARY KEY,
    GradeCode VARCHAR(20) NOT NULL UNIQUE,
    GradeName VARCHAR(100) NOT NULL,
    Description VARCHAR(500) NULL,
    IsActive BIT NOT NULL DEFAULT(1),
    CreatedBy VARCHAR(100) NOT NULL,
    CreatedDate DATETIME2 NOT NULL DEFAULT(GETDATE())
);

CREATE TABLE SalaryMatrix (
    SalaryMatrixId BIGINT IDENTITY(1,1) PRIMARY KEY,
    MatrixCode VARCHAR(30) NOT NULL UNIQUE,
    CompanyId INT NOT NULL,
    LegalEntityId INT NOT NULL,
    EffectiveDate DATE NOT NULL,
    ExpirationDate DATE NULL,
    VersionNo INT NOT NULL,
    Status VARCHAR(20) NOT NULL,
    Remarks NVARCHAR(1000) NULL,
    CreatedBy VARCHAR(100) NOT NULL,
    CreatedDate DATETIME2 NOT NULL DEFAULT(GETDATE())
);

CREATE TABLE SalaryMatrixDetail (
    SalaryMatrixDetailId BIGINT IDENTITY(1,1) PRIMARY KEY,
    SalaryMatrixId BIGINT NOT NULL,
    SalaryGradeId INT NOT NULL,
    MinimumSalary DECIMAL(18,2) NOT NULL,
    MidpointSalary DECIMAL(18,2) NOT NULL,
    MaximumSalary DECIMAL(18,2) NOT NULL,
    CurrencyCode VARCHAR(10) NOT NULL DEFAULT('PHP'),
    CONSTRAINT FK_SalaryMatrixDetail_Header
        FOREIGN KEY (SalaryMatrixId) REFERENCES SalaryMatrix(SalaryMatrixId),
    CONSTRAINT FK_SalaryMatrixDetail_Grade
        FOREIGN KEY (SalaryGradeId) REFERENCES SalaryGrade(SalaryGradeId)
);

CREATE TABLE PositionSalaryGrade (
    PositionSalaryGradeId BIGINT IDENTITY(1,1) PRIMARY KEY,
    PositionId INT NOT NULL,
    SalaryGradeId INT NOT NULL,
    EffectiveDate DATE NOT NULL,
    EndDate DATE NULL,
    IsActive BIT NOT NULL DEFAULT(1),
    CONSTRAINT FK_PositionSalaryGrade_Grade
        FOREIGN KEY (SalaryGradeId) REFERENCES SalaryGrade(SalaryGradeId)
);
