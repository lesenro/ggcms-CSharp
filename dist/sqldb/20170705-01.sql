/*
添加修改密码权限菜单
*/

INSERT INTO [dbo].[GgcmsPowers] ([PowerName], [OrderId], [ParentId], [PowerTag], [IconClass], [ShowInSidebar], [PowerType], [PowerParams], [Path]) VALUES ( N'修改密码', N'27', N'2', N'modifyPassword', null, N'0', N'0', null, N'modifyPassword')
GO