use StudentManager
go
--判断视图是否已存在
if exists(select * from sysobjects where name='View_StuScore')
drop view View_StuScore
go
--创建视图
create view View_StuScore
as
	select Students.StudentId,StudentName,ClassName,C#=CSharp,SQLDB=SQLServerDB,
		   ScoreSum=(CSharp+SQLServerDB),AvgScore=Round((CSharp+SQLServerDB) /2.0,2)
	from Students
	inner join ScoreList ON Students.StudentId = ScoreList.StudentId
	inner join StudentClass ON Students.ClassId = StudentClass.ClassId
	-- 定义视图中不能出现 order by子句，除非select列表中也有一个 top子句
	-- 不能出现into，不能引用临时表或表变量 
go
--查询视图
select * from View_StuScore

