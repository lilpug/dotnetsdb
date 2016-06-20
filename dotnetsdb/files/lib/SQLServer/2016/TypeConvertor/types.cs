using System;
using System.Data;

namespace DotNetSDB
{
    public partial class SqlServer2016TypeConvertor : SqlServerTypeConvertor
    {
        public override void typeDateTime()
        {
            _DbTypeList.Add(new DbTypeMapEntry(typeof(DateTime), DbType.DateTime2, SqlDbType.DateTime2));
        }
    }
}