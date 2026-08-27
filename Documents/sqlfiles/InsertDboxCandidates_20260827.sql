-- Generated from SampleCandidates.xlsx, sheet [INSERT].
SET NOCOUNT ON;

--truncate table [DboxCandidates];

--update DboxCandidates set StatusId=1 where ResponseId > 0

INSERT INTO [dbo].[DboxCandidates] (
    [ResponseId],
    [DboxId],
    [CSGId],
    [CompanyId],
    [DivisionId],
    [DepartmentId],
    [DboxRefNum],
    [CandidateName],
    [Company],
    [Division],
    [Department],
    [CostCenter],
    [JobLevel],
    [JobPosition],
    [EmailAddress],
    [ContactNumber]
)
VALUES
    (1, NULL, 110, 10, 12, NULL, N'CA001', N'Mary Grace Piattos', N'UL Skin Sciences Inc.', N'CONSUMER PORTFOLIO STRATEGY AND DEVELOPMENT', N'RESEARCH & DEVELOPMENT - CONSUMER', N'CC-6101', N'S5', N'R&D', N'TestEmail01.com', N'12345678'),
    (2, NULL, 5, 1, 4, NULL, N'CA002', N'Anna Cornick', N'Unilab Inc.', N'UGI - LEGAL, EXT AFFAIRS, PUBLIC RELATIONS', N'EXTERNAL AFFAIRS', N'CC-6102', N'S2', N'External Affairs Specialist', N'TestEmail02.com', N'23456789'),
    (0, NULL, 109, 10, 13, NULL, N'CA003', N'Cris P. Chippy', N'UL Skin Sciences Inc.', N'REVENUE AND COMMERCIAL OPERATIONS', N'PRODUCT MANAGEMENT', N'CC-6103', N'AM', N'ASST. PRODUCT MANAGER', N'TestEmail01.com', N'34567900'),
    (4, NULL, 3, 1, 2, NULL, N'CA004', N'Lorna Nova', N'Unilab Inc.', N'SALES AND CUSTOMER DEVELOPMENT', N'NKA CT SSD/TGP/RPI', N'CC-6104', N'S4', N'CUSTOMER DEVELOPMENT SPECIALIST', N'TestEmail02.com', N'45679011'),
    (0, NULL, 12, 1, 1, NULL, N'CA005', N'Boy Boy Chiz Curls', N'Unilab Inc.', N'FINANCE', N'CG&AA PROPERTY COS. & ASSET MGT', N'CC-6105', N'5', N'CONTROLLERSHIP ANALYST', N'TestEmail01.com', N'56790122'),
    (0, NULL, 4, 1, 2, NULL, N'CA006', N'Marga Mr. Chips', N'Unilab Inc.', N'SALES AND CUSTOMER DEVELOPMENT', N'CONSUMER BEAUTY TRADE MARKETING', N'CC-6106', N'S3', N'Trade Marketing Specialist', N'TestEmail02.com', N'67901233'),
    (7, NULL, 4, 1, 1, NULL, N'CA007', N'Jenny Jack ‘n Jill', N'Unilab Inc.', N'FINANCE', N'BUSINESS UNIT FIN - PHARMA/MAINSTREAM', N'CC-6107', N'S3', N'Business Unit Finance Specialist', N'TestEmail01.com', N'79012344'),
    (8, NULL, 11, 1, 1, NULL, N'CA008', N'Carlo Clover Chips', N'Unilab Inc.', N'FINANCE', N'CONTROLLERSHIP PHARMA/MANUFACTURING', N'CC-6108', N'6', N'Controllership Specialist', N'TestEmail02.com', N'90123455'),
    (9, NULL, 1, 1, 2, NULL, N'CA009', N'Bea Boy Bawang', N'Unilab Inc.', N'SALES AND CUSTOMER DEVELOPMENT', N'TRADE EXECUTIONS (GMA/LUZON)', N'CC-6109', N'AM', N'Trade Execution Specialist', N'TestEmail01.com', N'101234566'),
    (10, NULL, 1, 1, 2, NULL, N'CA010', N'Sandy Safari', N'Unilab Inc.', N'SALES AND CUSTOMER DEVELOPMENT', N'TRADE EXECUTIONS (VISMIN)', N'CC-6110', N'AM', N'Trade Execution Specialist / Key Accounts', N'TestEmail02.com', N'112345677'),
    (11, NULL, 109, 10, 14, NULL, N'CA011', N'Patty Piattos', N'UL Skin Sciences Inc.', N'REVENUE AND COMMERCIAL OPERATIONS', N'E-COMMERCE', N'CC-6111', N'AM', N'ECommerce Officer', N'TestEmail01.com', N'123456788'),
    (12, NULL, 110, 10, 12, NULL, N'CA012', N'Rico Roller Coaster', N'UL Skin Sciences Inc.', N'CONSUMER PORTFOLIO STRATEGY AND DEVELOPMENT', N'RESEARCH & DEVELOPMENT - CONSUMER', N'CC-6112', N'S5', N'R&D', N'TestEmail02.com', N'134567899'),
    (13, NULL, 5, 1, 4, NULL, N'CA013', N'Nina Nagaraya', N'Unilab Inc.', N'UGI - LEGAL, EXT AFFAIRS, PUBLIC RELATIONS', N'EXTERNAL AFFAIRS', N'CC-6113', N'S2', N'External Affairs Specialist', N'TestEmail01.com', N'145679010'),
    (14, NULL, 109, 10, 13, NULL, N'CA014', N'Marco Mang Juan', N'UL Skin Sciences Inc.', N'REVENUE AND COMMERCIAL OPERATIONS', N'PRODUCT MANAGEMENT', N'CC-6114', N'AM', N'ASST. PRODUCT MANAGER', N'TestEmail02.com', N'156790121'),
    (1, NULL, 3, 1, 2, NULL, N'CA015', N'Joy Chippy', N'Unilab Inc.', N'SALES AND CUSTOMER DEVELOPMENT', N'NKA CT SSD/TGP/RPI', N'CC-6115', N'S4', N'CUSTOMER DEVELOPMENT SPECIALIST', N'TestEmail01.com', N'167901232'),
    (2, NULL, 12, 1, 1, NULL, N'CA016', N'Grace Green Peas', N'Unilab Inc.', N'FINANCE', N'CG&AA PROPERTY COS. & ASSET MGT', N'CC-6116', N'5', N'CONTROLLERSHIP ANALYST', N'TestEmail02.com', N'179012343'),
    (3, NULL, 4, 1, 3, NULL, N'CA017', N'Tina Tostillas', N'Unilab Inc.', N'HROD', N'HR', N'CC-6117', N'S3', N'Trade Marketing Specialist', N'TestEmail01.com', N'190123454'),
    (4, NULL, 4, 1, 1, NULL, N'CA018', N'Joey Oishi', N'Unilab Inc.', N'FINANCE', N'BUSINESS UNIT FIN - PHARMA/MAINSTREAM', N'CC-6118', N'S3', N'Business Unit Finance Specialist', N'TestEmail02.com', N'201234565'),
    (5, NULL, 11, 1, 3, NULL, N'CA019', N'Candy Calbee', N'Unilab Inc.', N'HROD', N'HR', N'CC-6119', N'6', N'Controllership Specialist', N'TestEmail01.com', N'212345676'),
    (6, NULL, 1, 1, 2, NULL, N'CA020', N'Archie Choco Mucho', N'Unilab Inc.', N'SALES AND CUSTOMER DEVELOPMENT', N'TRADE EXECUTIONS (GMA/LUZON)', N'CC-6120', N'AM', N'Trade Execution Specialist', N'TestEmail02.com', N'223456787');

GO
