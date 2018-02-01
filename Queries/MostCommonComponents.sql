USE cocktails;

-- Get number of recipes in a specific library
SELECT COUNT(r. Id)
FROM Recipe r
JOIN Library l ON l.Id = r.LibraryId
WHERE l.Name = 'Death & Co';

-- Get most common recipe components by Library
SELECT c.Name, COUNT(i.Id) AS Recipes
FROM Component c
JOIN Ingredient i ON i.ComponentId = c.Id
JOIN Recipe r ON r.Id = i.RecipeId
JOIN Library l ON l.Id = r.LibraryId
WHERE l.Name = 'Death & Co'
GROUP BY c.Name
ORDER BY Recipes DESC, c.Name;
