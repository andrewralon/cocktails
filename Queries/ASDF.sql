USE cocktails;

-- Get Components: Type, Name
SELECT c.Name, ct.Name
FROM ComponentType ct
JOIN Component c ON c.ComponentTypeId = ct.Id
WHERE ct.Id = c.ComponentTypeId;

-- Get Recipe: Name/Instructions, Ingredients, Component Names
SELECT r.Name, i.Quantity, u.Name, c.Name, r.Instructions
FROM Recipe r
JOIN Ingredient i ON i.RecipeId = r.Id
JOIN Component c ON c.Id = i.ComponentId
JOIN Unit u ON u.Id = i.UnitId
WHERE r.Id = i.RecipeId
AND c.Id = i.ComponentId
AND u.Id = i.UnitId
ORDER BY i.Number;

-- Get ingredient Info: Recipe, Component
SELECT i.Id, r.Name, c.Name, i.Quantity, i.UnitId, i.Number
  FROM Ingredient i
  JOIN Recipe r ON r.Id = RecipeId
  JOIN Component c ON c.Id = ComponentId
  WHERE i.Id = '8de9a849-b065-4dc3-8089-7d56ecfa8048'


SELECT * FROM Recipe;
SELECT * FROM Component;
