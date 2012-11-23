USE fMailer;

INSERT INTO [Settings] ([Signature])
VALUES ('Simple signature!');
INSERT INTO [User] ([Id], [Login], [Password], [Email])
VALUES (1, 'admin', 'root', 'sly.feafarot@gmail.com');

GO

INSERT INTO [ContactsGroup] ([Name], [UserId])
VALUES ('UA-Analyze', 1), 
       ('Framework', 1), 
       ('Winter School', 1), 
       ('KTURE main', 1);
       
INSERT INTO [Contact] ([FirstName], [LastName], [MiddleName], [Email], [UserId])
VALUES ('Roman', 'Konkin', 'Vladimirovich', 'sly.feafarot@gmail.com', 1),
	   ('Ivan', 'Vasiliy', 'Ivanov', 'ivianov@test.ml', 1);
	   
INSERT INTO [ContactsToGroups] ([ContactId], [ContactsGroupId])
VALUES (3, 1), (3, 2);