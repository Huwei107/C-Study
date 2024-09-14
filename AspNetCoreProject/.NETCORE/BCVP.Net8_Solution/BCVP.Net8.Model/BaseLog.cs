using SqlSugar;
using System;

namespace BCVP.Net8.Model
{
    public abstract class BaseLog : RootEntityTkey<long>
    {
        /// <summary>
        /// SplitField:分表特性，按照日期作为分表依据
        /// </summary>
        [SplitField]
        public DateTime? DateTime { get; set; }

        [SugarColumn(IsNullable = true)]
        public string Level { get; set; }

        [SugarColumn(IsNullable = true, ColumnDataType = "longtext,text,clob")]
        public string Message { get; set; }

        [SugarColumn(IsNullable = true, ColumnDataType = "longtext,text,clob")]
        public string MessageTemplate { get; set; }

        [SugarColumn(IsNullable = true, ColumnDataType = "longtext,text,clob")]
        public string Properties { get; set; }
    }
}