SELECT 
    s.name AS SchemaName,
    dp.name AS OwnerName
FROM sys.schemas s
JOIN sys.database_principals dp
    ON s.principal_id = dp.principal_id
WHERE dp.name = 'xjoboffer_user';

ALTER AUTHORIZATION ON SCHEMA::HangFire TO dbo;