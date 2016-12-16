//the original idea was from: //https://gist.github.com/abrahamjp/858392
using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace DotNetSDB
{
    /// <summary>
    /// Convert a base data type to another base data type
    /// </summary>
    public partial class MySqlTypeConvertor
    {
        public virtual void typeBool()
        {
            typeList.Add(new DbTypeMapEntry(typeof(bool), DbType.Boolean, MySqlDbType.Bit));
        }

        public virtual void typeByte()
        {
            typeList.Add(new DbTypeMapEntry(typeof(byte), DbType.Binary, MySqlDbType.Byte));
        }

        public virtual void typeByteArray()
        {
            typeList.Add(new DbTypeMapEntry(typeof(byte[]), DbType.Binary, MySqlDbType.VarBinary));
        }

        public virtual void typeDateTime()
        {
            typeList.Add(new DbTypeMapEntry(typeof(DateTime), DbType.DateTime, MySqlDbType.DateTime));
        }

        public virtual void typeDecimal()
        {
            typeList.Add(new DbTypeMapEntry(typeof(Decimal), DbType.Decimal, MySqlDbType.Decimal));
        }

        public virtual void typeDouble()
        {
            typeList.Add(new DbTypeMapEntry(typeof(double), DbType.Double, MySqlDbType.Float));
        }

        public virtual void typeGUID()
        {
            typeList.Add(new DbTypeMapEntry(typeof(Guid), DbType.Guid, MySqlDbType.Guid));
        }

        public virtual void typeInt16()
        {
            typeList.Add(new DbTypeMapEntry(typeof(Int16), DbType.Int16, MySqlDbType.Int16));
        }

        public virtual void typeInt32()
        {
            typeList.Add(new DbTypeMapEntry(typeof(Int32), DbType.Int32, MySqlDbType.Int32));
        }

        public virtual void typeInt64()
        {
            typeList.Add(new DbTypeMapEntry(typeof(Int64), DbType.Int64, MySqlDbType.Int64));
        }

        public virtual void typeObject()
        {
            typeList.Add(new DbTypeMapEntry(typeof(object), DbType.Object, MySqlDbType.Blob));
        }

        public virtual void typeString()
        {
            typeList.Add(new DbTypeMapEntry(typeof(string), DbType.String, MySqlDbType.VarChar));
        }

        public virtual void extraType()
        {
        }

        private void init()
        {
            typeBool();
            typeByte();
            typeByteArray();
            typeDateTime();
            typeDecimal();
            typeDouble();
            typeGUID();
            typeInt16();
            typeInt32();
            typeInt64();
            typeObject();
            typeString();

            //This one is run for any extra hooks we want to add in later versions
            extraType();
        }
    }
}