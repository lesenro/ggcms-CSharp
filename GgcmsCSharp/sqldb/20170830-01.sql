/*
�޸��������ӱ�LinkTypeΪ�ַ���
*/
ALTER TABLE [dbo].[GgcmsFriendLinks] ALTER COLUMN [LinkType] nvarchar(100) 
GO

/*
�����������ӷ����ֵ�
*/
INSERT INTO [dbo].[GgcmsDictionaries] ([Title],[Value],[DictType],[OrderID],[SysFlag],[describe]) VALUES('������������','link_type',1,5,1,'������������')
GO

--�޸��ֵ�����DictTypeΪ�ַ���
ALTER TABLE [dbo].[GgcmsDictionaries] ALTER COLUMN [DictType] nvarchar(100) 
GO
