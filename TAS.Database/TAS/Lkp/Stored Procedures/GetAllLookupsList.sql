

CREATE PROCEDURE [Lkp].[GetAllLookupsList]

AS
BEGIN
--0------------------------- LookupsGroups -----------------------------
SELECT '00000000-0000-0000-0000-000000000000' AS LookupGroupID, '-- Please Select --' AS LookupGroup
UNION ALL
SELECT LookupGroupID, LookupGroup FROM [Lkp].LookupsGroups WHERE IsDeleted = 0 ORDER BY LookupGroup
----------------------------------------------------------------------

--1-------------------------  (Mode Of Travel Code) -----------------------------
SELECT '00000000-0000-0000-0000-000000000000' AS LookupsID, '-- Mode Of Travel --' AS [Description]
UNION ALL
SELECT LookupsID, [Description] FROM [Lkp].Lookups l left join [Lkp].[LookupsGroups] lg on lg.LookupGroupID = l.LookupGroupID  WHERE lg.LookupGroup='Mode Of Travel' and l.IsDeleted = 0 ORDER BY [Description]
----------------------------------------------------------------------


--2-------------------------  (Trip Schema Code) -----------------------------
SELECT '00000000-0000-0000-0000-000000000000' AS LookupsID, '-- Please Select --' AS [Description]
UNION ALL
SELECT LookupsID, [Description] FROM [Lkp].Lookups l left join [Lkp].[LookupsGroups] lg on lg.LookupGroupID = l.LookupGroupID  WHERE lg.LookupGroup='Trip Schema' and l.IsDeleted = 0 ORDER BY [Description]
----------------------------------------------------------------------


--3-------------------------  (From Location) -----------------------------
SELECT '00000000-0000-0000-0000-000000000000' AS CityID, '-- Please Select --' AS CityDescription
UNION ALL
SELECT TOP 100 CityID, CityDescription FROM [Lookups].[Lkp].[City] WHERE [IsDeleted] = 0 ORDER BY CityDescription
----------------------------------------------------------------------

--4-------------------------  (To Location) -----------------------------
SELECT '00000000-0000-0000-0000-000000000000' AS CityID, '-- Please Select --' AS CityDescription
UNION ALL
SELECT TOP 100 CityID, CityDescription FROM [Lookups].[Lkp].[City] WHERE [IsDeleted] = 0 ORDER BY CityDescription
----------------------------------------------------------------------


--5-------------------------  (Status BY CODE TA) -----------------------------
SELECT '00000000-0000-0000-0000-000000000000' AS LookupsID,'' AS Code, '-- Please Select --' AS [Description],'' AS LongDescription
UNION ALL
SELECT LookupsID, Code, [Description], LongDescription FROM [Lkp].Lookups l left join [Lkp].[LookupsGroups] lg on lg.LookupGroupID = l.LookupGroupID  WHERE lg.LookupGroup='TA Status Code' and l.IsDeleted = 0 ORDER BY [Description]
----------------------------------------------------------------------

--6-------------------------  (Expenditure Code) -----------------------------
SELECT '00000000-0000-0000-0000-000000000000' AS LookupsID, '-- Please Select --' AS [Description]
UNION ALL
SELECT LookupsID, [Description] FROM [Lkp].Lookups l left join [Lkp].[LookupsGroups] lg on lg.LookupGroupID = l.LookupGroupID  WHERE lg.LookupGroup='Expenditure' and l.IsDeleted = 0 ORDER BY [Description]
----------------------------------------------------------------------

--7-------------------------  (Currencies) -----------------------------
SELECT '00000000-0000-0000-0000-000000000000' AS CurrencyID, '-- Currency --' AS CurrencyName
UNION ALL
SELECT CurrencyID, CurrencyName FROM [Lookups].[Lkp].[Currency] WHERE [IsDeleted] = 0 ORDER BY CurrencyName
----------------------------------------------------------------------
--8-------------------------  (Status BY CODE TEC) -----------------------------
SELECT '00000000-0000-0000-0000-000000000000' AS LookupsID,'' AS Code, '-- Please Select --' AS [Description]
UNION ALL
SELECT LookupsID, Code, [Description] FROM [Lkp].Lookups l left join [Lkp].[LookupsGroups] lg on lg.LookupGroupID = l.LookupGroupID  WHERE lg.LookupGroup='TEC Status Code' and l.IsDeleted = 0 ORDER BY [Description]
----------------------------------------------------------------------
--9-------------------------  (Pay Office) -----------------------------
SELECT '00000000-0000-0000-0000-000000000000' AS LookupsID,'' AS Code, '-- Paying Office (Location Code) --' AS [Description]
UNION ALL
SELECT LookupsID, Code, [Description] FROM [Lkp].Lookups l left join [Lkp].[LookupsGroups] lg on lg.LookupGroupID = l.LookupGroupID  WHERE lg.LookupGroup='Pay Office' and l.IsDeleted = 0 ORDER BY [Description]
----------------------------------------------------------------------


END
