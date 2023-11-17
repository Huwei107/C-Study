use StudentManager
go
if exists(select * from sysobjects where name='usp_ScoreQuery')
drop procedure usp_ScoreQuery
go
create procedure usp_ScoreQuery --�����޲δ洢����
as
    --��ѯ������Ϣ
    select Students.StudentId,StudentName,ClassName,
              ScoreSum=(CSharp+SQLServerDB) from Students
    inner join StudentClass on StudentClass.ClassId=Students.ClassId
    inner join ScoreList on Students.StudentId=ScoreList.StudentId
    order by ScoreSum DESC
    --ͳ�Ʒ���������Ϣ
    select StudentClass.ClassId,C#Avg=avg(CSharp),DBAvg=avg(SQLServerDB)  into #scoreTemp
    from StudentClass 
    inner join Students on StudentClass.ClassId=Students.ClassId
    inner join ScoreList on ScoreList.StudentId=Students.StudentId
    group by StudentClass.ClassId order by ClassId
	--
    select ClassName,C#Avg,DBAvg from #scoreTemp
    inner join StudentClass on StudentClass.ClassId=#scoreTemp.ClassId
go
exec usp_ScoreQuery  --���ô洢����
-----------------------------------------------------------------------------------------------------------
if exists(select * from Sysobjects where name = 'usp_ScoreQuery2')
drop procedure usp_ScoreQuery2
go
--�����������Ĵ洢����
create procedure usp_ScoreQuery2
@CSharp int=60,
@DB int=60
as
	select Students.StudentId,StudentName,C#=CSharp,DB=SQLServerDB from Students
	inner join ScoreList on Students.StudentId = ScoreList.StudentId
	where CSharp < @CSharp or SQLServerDB < @DB
go
--���ô������Ĵ洢����
exec usp_ScoreQuery2 60,65 --���ղ���˳��ֵ
exec usp_ScoreQuery2 @DB=65,@CSharp=60 --����˳����Ե���
exec usp_ScoreQuery2 65
exec usp_ScoreQuery2 default,65
-----------------------------------------------------------------------------------------------------------
if exists(select * from Sysobjects where name = 'usp_ScoreQuery3')
drop procedure usp_ScoreQuery3
go
--�����������Ĵ洢����
create procedure usp_ScoreQuery3
@AbsentCount int output, --ȱ����������
@FailedCount int output, --�������������
@CSharp int=60,
@DB int=60
as
	select Students.StudentId,StudentName,C#=CSharp,DB=SQLServerDB from Students
	inner join ScoreList on Students.StudentId = ScoreList.StudentId
	where CSharp < @CSharp or SQLServerDB < @DB
	--��ѯͳ�ƽ��
	select @AbsentCount=count(*) from Students
	where StudentId not in (select StudentId from ScoreList) --��ѯȱ����������
	select @FailedCount=count(*) from ScoreList
	where CSharp < @CSharp or SQLServerDB < @DB --��ѯ�������������
go
--���ô���������Ĵ洢����
declare @AbsentCount int,@FailedCount int --���ȶ����������
exec usp_ScoreQuery3 @AbsentCount output,@FailedCount output
select ȱ����������=@AbsentCount,�������������=@FailedCount
	










	






