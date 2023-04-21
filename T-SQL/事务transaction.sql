use StudentManager
go
update CardAccount set CurrentMoney=CurrentMoney-1000 where StudentId=100001
update CardAccount set CurrentMoney=CurrentMoney+1000 where StudentId=100002
-----------------------------------------------------------------------------------------------------------
declare @errorSum int --��������������ۼ�����ִ���еĴ���
set @errorSum=0 --��ʼ��Ϊ0������û�д���
begin transaction
	begin
		--ת��
		update CardAccount set CurrentMoney=CurrentMoney-1000 where StudentId=100001
		set @errorSum=@errorSum+@@ERROR --�ۼ��Ƿ��д���
		--ת��
		update CardAccount set CurrentMoney=CurrentMoney+1000 where StudentId=100002
		set @errorSum=@errorSum+@@ERROR
		if(@errorSum>0)
			rollback transaction
		else
			commit transaction
	end
go
select * from CardAccount
-----------------------------------------------------------------------------------------------------------
use StudentManager
go
if exists(select * from Sysobjects where name='usp_TransferAccounts')
drop procedure usp_TransferAccounts
go
create procedure usp_TransferAccounts
@inputAccount int,--ת���˻�
@outputAccount int,--ת���˻�
@transferMoney int --���׽��
as
	declare @errorSum int --��������������ۼ�����ִ���еĴ���
	set @errorSum=0 --��ʼ��Ϊ0������û�д���
begin transaction
	begin
		--ת��
		update CardAccount set CurrentMoney=CurrentMoney-@transferMoney where StudentId=@outputAccount
		set @errorSum=@errorSum+@@ERROR
		--ת��
		update CardAccount set CurrentMoney=CurrentMoney+@transferMoney where StudentId=@inputAccount
		set @errorSum=@errorSum+@@ERROR
		if(@errorSum>0)
			rollback transaction
		else
			commit transaction
	end
go
-- ����ʧ�ܵ�ת��
exec usp_TransferAccounts 100002,100001,1000
select Students.StudentId,StudentName,CurrentMoney from Students
inner join CardAccount on Students.StudentId=CardAccount.StudentId
-- ���Գɹ���ת��
exec usp_TransferAccounts 100002,100001,800
select Students.StudentId,StudentName,CurrentMoney from Students
inner join CardAccount on Students.StudentId=CardAccount.StudentId






