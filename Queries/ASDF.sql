USE cocktails;

-- Get Components: Type, Name
SELECT c.Name, ct.Type
FROM ComponentType AS ct, Component AS c
WHERE ct.Id = c.ComponentTypeId;

-- Get Recipe: Name/Instructions, Ingredients, Component Names
SELECT r.Name, i.Quantity, u.Name, c.Name, r.Instructions
FROM Recipe AS r, Ingredient AS i, Component AS c, Unit AS u
WHERE r.Id = i.RecipeId
AND c.Id = i.ComponentId
AND u.Id = i.UnitId
ORDER BY i.Number;


SELECT * FROM Recipe;
SELECT * FROM Component;
