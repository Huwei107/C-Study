use StudentManager
go
--�ж���ͼ�Ƿ��Ѵ���
if exists(select * from sysobjects where name='View_StuScore')
drop view View_StuScore
go
--������ͼ
create view View_StuScore
as
	select Students.StudentId,StudentName,ClassName,C#=CSharp,SQLDB=SQLServerDB,
		   ScoreSum=(CSharp+SQLServerDB),AvgScore=Round((CSharp+SQLServerDB) /2.0,2)
	from Students
	inner join ScoreList ON Students.StudentId = ScoreList.StudentId
	inner join StudentClass ON Students.ClassId = StudentClass.ClassId
	-- ������ͼ�в��ܳ��� order by�Ӿ䣬����select�б���Ҳ��һ�� top�Ӿ�
	-- ���ܳ���into������������ʱ������� 
go
--��ѯ��ͼ
select * from View_StuScore

