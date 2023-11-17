use StudentManager
go
if exists(select * from sysobjects where name='usp_ScoreQuery')
drop procedure usp_ScoreQuery
go
create procedure usp_ScoreQuery --创建无参存储过程
as
    --查询考试信息
    select Students.StudentId,StudentName,ClassName,
              ScoreSum=(CSharp+SQLServerDB) from Students
    inner join StudentClass on StudentClass.ClassId=Students.ClassId
    inner join ScoreList on Students.StudentId=ScoreList.StudentId
    order by ScoreSum DESC
    --统计分析考试信息
    select StudentClass.ClassId,C#Avg=avg(CSharp),DBAvg=avg(SQLServerDB)  into #scoreTemp
    from StudentClass 
    inner join Students on StudentClass.ClassId=Students.ClassId
    inner join ScoreList on ScoreList.StudentId=Students.StudentId
    group by StudentClass.ClassId order by ClassId
	--
    select ClassName,C#Avg,DBAvg from #scoreTemp
    inner join StudentClass on StudentClass.ClassId=#scoreTemp.ClassId
go
exec usp_ScoreQuery  --调用存储过程
-----------------------------------------------------------------------------------------------------------
if exists(select * from Sysobjects where name = 'usp_ScoreQuery2')
drop procedure usp_ScoreQuery2
go
--创建带参数的存储过程
create procedure usp_ScoreQuery2
@CSharp int=60,
@DB int=60
as
	select Students.StudentId,StudentName,C#=CSharp,DB=SQLServerDB from Students
	inner join ScoreList on Students.StudentId = ScoreList.StudentId
	where CSharp < @CSharp or SQLServerDB < @DB
go
--调用带参数的存储过程
exec usp_ScoreQuery2 60,65 --按照参数顺序赋值
exec usp_ScoreQuery2 @DB=65,@CSharp=60 --参数顺序可以调换
exec usp_ScoreQuery2 65
exec usp_ScoreQuery2 default,65
-----------------------------------------------------------------------------------------------------------
if exists(select * from Sysobjects where name = 'usp_ScoreQuery3')
drop procedure usp_ScoreQuery3
go
--创建带参数的存储过程
create procedure usp_ScoreQuery3
@AbsentCount int output, --缺考的总人数
@FailedCount int output, --不及格的总人数
@CSharp int=60,
@DB int=60
as
	select Students.StudentId,StudentName,C#=CSharp,DB=SQLServerDB from Students
	inner join ScoreList on Students.StudentId = ScoreList.StudentId
	where CSharp < @CSharp or SQLServerDB < @DB
	--查询统计结果
	select @AbsentCount=count(*) from Students
	where StudentId not in (select StudentId from ScoreList) --查询缺考的总人数
	select @FailedCount=count(*) from ScoreList
	where CSharp < @CSharp or SQLServerDB < @DB --查询不及格的总人数
go
--调用带输出参数的存储过程
declare @AbsentCount int,@FailedCount int --首先定义输出参数
exec usp_ScoreQuery3 @AbsentCount output,@FailedCount output
select 缺考的总人数=@AbsentCount,不及格的总人数=@FailedCount
	










	






