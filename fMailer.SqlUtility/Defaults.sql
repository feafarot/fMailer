USE [fMailer]

INSERT INTO[Settings]
           ([Signature]
           ,[EmailAddressFrom]
           ,[Username]
           ,[Password]
           ,[Pop3Address]
           ,[Pop3IsGmail]
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
           ,1
           ,995
           ,1
           ,'smtp.gmail.com'
           ,587
           ,587
           ,1
           ,1)
INSERT INTO [User] ([Id], [Login], [Password])
VALUES (1, 'admin', 'root' );
GO

INSERT INTO [ContactsGroup] ([Name], [UserId])
VALUES ('Administrators', 1);
GO

INSERT INTO [Contact] ([FirstName], [LastName], [MiddleName], [Email], [UserId])
VALUES ('Roman', 'Konkin', 'Vladimirovich', 'sly.feafarot@gmail.com', 1);
GO

INSERT INTO [ContactsToGroups] ([ContactId], [ContactsGroupId])
VALUES (1, 1);
GO