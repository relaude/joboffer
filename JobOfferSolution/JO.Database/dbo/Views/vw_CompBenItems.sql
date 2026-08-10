CREATE VIEW [dbo].[vw_CompBenItems]
AS
SELECT
    [CBI].[Id],
    [CBI].[CatId],
    [CBC].[CatName],
    [CBI].[ItmName],
    [CBI].[ItmDesc],
    [CBI].[Amount]
FROM [dbo].[CompBenItems] AS [CBI]
LEFT JOIN [dbo].[CompBenItmCat] AS [CBC]
    ON [CBC].[Id] = [CBI].[CatId];