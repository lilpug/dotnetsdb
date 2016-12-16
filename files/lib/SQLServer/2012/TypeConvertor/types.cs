using System;
using System.Data;

namespace DotNetSDB
{
    public partial class SqlServer2012TypeConvertor : SqlServerTypeConvertor
    {
        public override void typeDateTime()
        {
            typeList.Add(new DbTypeMapEntry(typeof(DateTime), DbType.DateTime2, SqlDbType.DateTime2));
        }
    }
}