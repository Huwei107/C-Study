use StudentManageDB
go

-- ����ѧ�ű���
declare @stuid int,@stuname varchar(20)

-- ��ѯ��������Ϣ
set @stuname='����'
select StudentId,StudentName,Gender,StudentIdNo from Students
where StudentName=@stuname

-- ��ѯ������ѧ��
select @stuid=StudentId from Students where StudentName=@stuname

-- ��ѯ������ѧ�����ڵ�ѧԱ
select StudentId,StudentName,Gender,StudentIdNo from Students
where StudentId=(@stuid+1) or StudentId=(@stuid-1)

print'���������ƣ�'+@@servername
print @@error

declare @birthday datetime,@day int,@age int
select @birthday=Birthday from Students where StudentId=100002
--��������
set @day=DATEDIFF(DAYOFYEAR, @birthday,GETDATE())
--��������
set @age=FLOOR(@day/365)
--�����Ϣ
print'100002ѧԱ���䣺'+CONVERT(VARCHAR(20),@age)
--ֱ�Ӳ�ѯ
select FLOOR(DATEDIFF(DAYOFYEAR, @birthday,GETDATE())/365) as ���� from Students where StudentId=100002


