USE cocktails;

ALTER TABLE Recipe
DROP CONSTRAINT FK_LibraryRecipe;

ALTER TABLE Component
DROP CONSTRAINT FK_ComponentTypeComponent;

ALTER TABLE Ingredient
DROP CONSTRAINT FK_RecipeIngredient
	,FK_ComponentIngredient
	,FK_UnitIngredient;

DROP TABLE Recipe;

DROP TABLE Ingredient;

DROP TABLE Component;

DROP TABLE ComponentType;

DROP TABLE Unit;

DROP TABLE Library;
