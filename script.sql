USE CourseProject;

	UPDATE [CourseProject].[dbo].[Users]
	SET [IsAdmin] = 1
	WHERE [Username] = 'derelict';

DELETE  FROM ProductsFromBasket;
INSERT [CourseProject].[dbo].[Categories] VALUES
	('�����'),
	('����� � �������'),
	('����������������'),
	('���������');

GO