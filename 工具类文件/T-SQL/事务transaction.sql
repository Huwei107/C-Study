use StudentManager
go
update CardAccount set CurrentMoney=CurrentMoney-1000 where StudentId=100001
update CardAccount set CurrentMoney=CurrentMoney+1000 where StudentId=100002
-----------------------------------------------------------------------------------------------------------
declare @errorSum int --定义变量，用于累计事务执行中的错误
set @errorSum=0 --初始化为0，代表没有错误
begin transaction
	begin
		--转出
		update CardAccount set CurrentMoney=CurrentMoney-1000 where StudentId=100001
		set @errorSum=@errorSum+@@ERROR --累计是否有错误
		--转入
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
@inputAccount int,--转入账户
@outputAccount int,--转出账户
@transferMoney int --交易金额
as
	declare @errorSum int --定义变量，用于累计事务执行中的错误
	set @errorSum=0 --初始化为0，代表没有错误
begin transaction
	begin
		--转出
		update CardAccount set CurrentMoney=CurrentMoney-@transferMoney where StudentId=@outputAccount
		set @errorSum=@errorSum+@@ERROR
		--转入
		update CardAccount set CurrentMoney=CurrentMoney+@transferMoney where StudentId=@inputAccount
		set @errorSum=@errorSum+@@ERROR
		if(@errorSum>0)
			rollback transaction
		else
			commit transaction
	end
go
-- 测试失败的转账
exec usp_TransferAccounts 100002,100001,1000
select Students.StudentId,StudentName,CurrentMoney from Students
inner join CardAccount on Students.StudentId=CardAccount.StudentId
-- 测试成功的转账
exec usp_TransferAccounts 100002,100001,800
select Students.StudentId,StudentName,CurrentMoney from Students
inner join CardAccount on Students.StudentId=CardAccount.StudentId






