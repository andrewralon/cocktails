USE cocktails;

-- Get Components: Type, Name
SELECT c.Name, ct.Name
FROM ComponentType ct
JOIN Component c ON c.ComponentTypeId = ct.Id
WHERE ct.Id = c.ComponentTypeId;

-- Get Recipe: Name/Instructions, Ingredients, Component Names
SELECT r.Name, i.Quantity, u.Name AS Unit, c.Name AS Component, i.Id AS IngredientId, r.Instructions, l.Name AS Library
FROM Recipe r
JOIN Ingredient i ON i.RecipeId = r.Id
JOIN Component c ON c.Id = i.ComponentId
JOIN Unit u ON u.Id = i.UnitId
JOIN Library l ON l.Id = r.LibraryId
WHERE r.Id = i.RecipeId
AND c.Id = i.ComponentId
AND u.Id = i.UnitId
AND  r.Id IN ('e6a8d57d-741e-4312-9b3c-12cc79e08d41', '313000dc-25af-48b0-b95e-88387603c727')
ORDER BY l.Name, i.Number;

-- Get ingredient Info: Recipe, Component
SELECT i.Id, r.Name, c.Name, i.Quantity, i.UnitId, i.Number
  FROM Ingredient i
  JOIN Recipe r ON r.Id = RecipeId
  JOIN Component c ON c.Id = ComponentId
  WHERE i.Id = '8de9a849-b065-4dc3-8089-7d56ecfa8048'


SELECT * FROM Recipe;
SELECT * FROM Component;
