using System;
using System.Data;

namespace DotNetSDB
{
    /// <summary>
    /// The class that deals with database type conversions for SQL Server 2014
    /// </summary>
    public partial class SqlServer2014TypeConvertor : SqlServerTypeConvertor
    {
        /// <summary>
        /// Adds the datetime type conversions
        /// </summary>
        public override void TypeDateTime()
        {
            TypeList.Add(new DbTypeMapEntry(typeof(DateTime), DbType.DateTime2, SqlDbType.DateTime2));
        }
    }
}