
CREATE VIEW [dbo].[vw_CompBenPlans]
AS
SELECT
    [CBP].[Id],
    [CBP].[PlanName],

    [CBP].[SGId],
    [SG].[GradeName] AS [SalaryGrade],

    [CBP].[TypeId],
    [ET].[TypeName] AS [EmpStatus],

    [CBP].[AreaId],
    [AR].[AreaName] AS [WorkArea],

    [CBP].[SchedId],
    [SC].[SchedName] AS [WorkSchedule],

    [CBP].[ShiftId],
    [SH].[ShiftName] AS [ShiftCode],

    [CBP].[ClassId],
    [CL].[ClassName] AS [JobClass],

    [CBP].[FreqId],
    [FR].[FreqName] AS [Frequency],

    [CBP].[MRIId],
    [MRI].[MRIName] AS [MRI],

    [CBP].[Motorized],
    [CBP].[AllowTrans],
    [CBP].[AllowSpec],
    [CBP].[Incentive],
    [CBP].[Annual],
    [CBP].[Swipe],

    [CBP].[CreatedBy],
    [CBP].[CreatedAt],
    [CBP].[ModifiedBy],
    [CBP].[ModifiedAt]
FROM [dbo].[CompBenPlans] AS [CBP]
LEFT JOIN [dbo].[SalaryGrades] AS [SG]
    ON [SG].[Id] = [CBP].[SGId]
LEFT JOIN [dbo].[CompBenEmpType] AS [ET]
    ON [ET].[Id] = [CBP].[TypeId]
LEFT JOIN [dbo].[CompBenArea] AS [AR]
    ON [AR].[Id] = [CBP].[AreaId]
LEFT JOIN [dbo].[CompBenSched] AS [SC]
    ON [SC].[Id] = [CBP].[SchedId]
LEFT JOIN [dbo].[CompBenShift] AS [SH]
    ON [SH].[Id] = [CBP].[ShiftId]
LEFT JOIN [dbo].[CompBenClass] AS [CL]
    ON [CL].[Id] = [CBP].[ClassId]
LEFT JOIN [dbo].[CompBenFreq] AS [FR]
    ON [FR].[Id] = [CBP].[FreqId]
LEFT JOIN [dbo].[CompBenMRI] AS [MRI]
    ON [MRI].[Id] = [CBP].[MRIId];