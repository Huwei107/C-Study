using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using UseFactory.Report;

namespace UseFactory.Factory
{
    class Factory
    {
        //【1】定义接口变量
        static IReport objReport = null;
        //【2】读取配置文件
        static string reportType = ConfigurationManager.AppSettings["ReportType"].ToString();
        //【3】根据用户要求创建接口对象
        public static IReport ChooseReportType()
        {
            switch (reportType)
            {
                case "ExcelReport":
                    objReport = new ExcelReport();
                    break;
                case "WordReport":
                    objReport = new WordReport();
                    break;
                default:
                    break;
            }
            return objReport;
        }
    }
}
