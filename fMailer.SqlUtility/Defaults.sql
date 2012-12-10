USE [fMailer]

INSERT INTO[Settings]
           ([Signature]
           ,[EmailAddressFrom]
           ,[Username]
           ,[Password]
           ,[Pop3Address]
           ,[Pop3Prot]
           ,[Pop3UseSsl]
           ,[SmtpAddress]
           ,[SmtpSslPort]
           ,[SmtpTlsPort]
           ,[SmtpUseAuth]
           ,[SmtpUseSsl])
     VALUES
           ('Simple signature, just for test!'
           ,'sly.feafarot@gmail.com'
           ,'sly.feafarot@gmail.com'
           ,''
           ,'pop.gmail.com'
           ,995
           ,1
           ,'smtp.gmail.com'
           ,465
           ,587
           ,1
           ,1)
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
VALUES (1, 1), (1, 2);