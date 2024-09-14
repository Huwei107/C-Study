
using SqlSugar;

namespace BCVP.Net8.Model;

[Tenant("log")]
[SplitTable(SplitType.Month)] //按月分表 （自带分表支持 年、季、月、周、日）
[SugarTable($@"{nameof(AuditSqlLog)}_{{year}}{{month}}{{day}}")]
//[SugarTable($@"AuditSqlLog_20231201", "sql审计日志")]
public class AuditSqlLog : BaseLog
{

}