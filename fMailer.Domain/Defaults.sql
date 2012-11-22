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