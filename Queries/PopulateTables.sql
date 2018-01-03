USE cocktails;

--DELETE FROM Library;
INSERT INTO Library(Name) VALUES ('Death & Co');

--DELETE FROM ComponentType;
INSERT INTO ComponentType(Name) VALUES 
	('Absinthe'),
	('Agave'),
	('Bitter'),
	('Bourbon'),
	('Brandy'),
	('Cream'),
	('Fortified Wine'),
	('Fruit'),
	('Genever'),
	('Gin'),
	('Juice'),
	('Liqueur'),
	('Mix'),
	('Oddball'),
	('Rum'),
	('Scotch'),
	('Syrup'),
	('Topper'),
	('Vermouth'),
	('Vodka'),
	('Whiskey'),
	('Wine');

--DELETE FROM Units;
INSERT INTO Unit(Name) VALUES
	(NULL),
	('oz'),
	('ml'),
	('tsp'),
	('tbsp'),
	('dash'),
	('drop'),
	('bar spoon'),
	('pinch');

--DELETE FROM Component;
INSERT INTO Component(ComponentTypeId, Name) VALUES
	(15, 'Ron del Barrilito 3-Star Rum'),
	(11, 'Lime Juice'),
	(17, 'Acacia Honey Syrup'),
	(18, 'Dry Champagne');

--INSERT INTO Recipe(LibraryId, Name, Instructions) VALUES
--	('', 'Airmail', 'Shake all the ingredients (except the champagne) with ice, then strain into a flute. Top with champagne. No garnish');
