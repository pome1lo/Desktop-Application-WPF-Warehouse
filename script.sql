USE CourseProject;

	UPDATE [CourseProject].[dbo].[Users]
	SET [IsAdmin] = 1
	WHERE [Username] = 'admin';
	
DELETE  FROM ProductsFromBasket;
DELETE  FROM ORDERS;

DROP TABLE ProductsFromBasket;
DROP TABLE ORDERS;

SELECT * FROM Categories;
SELECT * FROM Products;
SELECT * FROM Users;
SELECT * FROM ORDERS;
SELECT * FROM ProductsFromBasket;

INSERT [CourseProject].[dbo].[Categories] VALUES
	('Фасад'),
	('Блоки и кирпичи'),
	('Металлопродукция'),
	('Электрика');

GO