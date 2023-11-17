use StudentManageDB
go

-- 声明学号变量
declare @stuid int,@stuname varchar(20)

-- 查询李明的信息
set @stuname='李铭'
select StudentId,StudentName,Gender,StudentIdNo from Students
where StudentName=@stuname

-- 查询李铭的学号
select @stuid=StudentId from Students where StudentName=@stuname

-- 查询与李铭学号相邻的学员
select StudentId,StudentName,Gender,StudentIdNo from Students
where StudentId=(@stuid+1) or StudentId=(@stuid-1)

print'服务器名称：'+@@servername
print @@error

declare @birthday datetime,@day int,@age int
select @birthday=Birthday from Students where StudentId=100002
--计算天数
set @day=DATEDIFF(DAYOFYEAR, @birthday,GETDATE())
--计算年龄
set @age=FLOOR(@day/365)
--输出信息
print'100002学员年龄：'+CONVERT(VARCHAR(20),@age)
--直接查询
select FLOOR(DATEDIFF(DAYOFYEAR, @birthday,GETDATE())/365) as 年龄 from Students where StudentId=100002


