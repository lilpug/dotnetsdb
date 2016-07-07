﻿//Convert .Net Type to SqlDbType or DbType and vise versa
//This class can be useful when you make conversion between types .The class supports conversion between .Net Type , SqlDbType and DbType .
//the original idea was from: //https://gist.github.com/abrahamjp/858392
//although i have modified type selectors to be more up to date i.e. datetime2
using System;
using System.Collections;
using System.Data;

namespace DotNetSDB
{
    /// <summary>
    /// Convert a base data type to another base data type
    /// </summary>
    public partial class SqlServerTypeConvertor
    {
        public class DbTypeMapEntry
        {
            public Type Type;
            public DbType DbType;
            public SqlDbType SqlDbType;

            public DbTypeMapEntry(Type type, DbType dbType, SqlDbType sqlDbType)
            {
                Type = type;
                DbType = dbType;
                SqlDbType = sqlDbType;
            }
        };

        public ArrayList typeList = new ArrayList();

        public SqlServerTypeConvertor()
        {
            init();
        }
    }
}