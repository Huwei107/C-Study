use StudentManager
go
if exists(select name from sysIndexes where name='IX_Student_StudentName')
drop index Students.IX_Student_StudentName
go
create nonclustered index IX_Student_StudentName
on Students(StudentName)
with fillfactor=30
go
------------------------------------------------------------------------------------------
-- 按照下列标准选择建立索引的列
-- 频繁搜索的列。
-- 经常用作查询选择的列。
-- 经常排序、分组的列。
-- 经常作联接的列(主键/外键)。
------------------------------------------------------------------------------------------
-- 使用索引的注意事项：
-- 1.查询时减少使用*返回全部列，不要返回不需要的列
-- 2.索引应该尽量小，在字节数小的列上建立索引
-- 3.WHERE子句中有多个条件表达式时，包含索引列的表达式应置于其他条件表达式之前
-- 4.根据业务数据发生频率，定期重新生成或重新组织索引，进行碎片整理
