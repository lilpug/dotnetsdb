//the original idea was from: //https://gist.github.com/abrahamjp/858392
using System;
using System.Data;

namespace DotNetSDB
{
    /// <summary>
    /// Convert a base data type to another base data type
    /// </summary>
    public partial class SqlServerTypeConvertor
    {
        public virtual void typeBool()
        {
            typeList.Add(new DbTypeMapEntry(typeof(bool), DbType.Boolean, SqlDbType.Bit));
        }

        public virtual void typeByte()
        {
            typeList.Add(new DbTypeMapEntry(typeof(byte), DbType.Binary, SqlDbType.TinyInt));
        }

        public virtual void typeByteArray()
        {
            typeList.Add(new DbTypeMapEntry(typeof(byte[]), DbType.Binary, SqlDbType.VarBinary));
        }

        public virtual void typeDateTime()
        {
            typeList.Add(new DbTypeMapEntry(typeof(DateTime), DbType.DateTime, SqlDbType.DateTime));
        }

        public virtual void typeDecimal()
        {
            typeList.Add(new DbTypeMapEntry(typeof(Decimal), DbType.Decimal, SqlDbType.Decimal));
        }

        public virtual void typeDouble()
        {
            typeList.Add(new DbTypeMapEntry(typeof(double), DbType.Double, SqlDbType.Float));
        }

        public virtual void typeGUID()
        {
            typeList.Add(new DbTypeMapEntry(typeof(Guid), DbType.Guid, SqlDbType.UniqueIdentifier));
        }

        public virtual void typeInt16()
        {
            typeList.Add(new DbTypeMapEntry(typeof(Int16), DbType.Int16, SqlDbType.SmallInt));
        }

        public virtual void typeInt32()
        {
            typeList.Add(new DbTypeMapEntry(typeof(Int32), DbType.Int32, SqlDbType.Int));
        }

        public virtual void typeInt64()
        {
            typeList.Add(new DbTypeMapEntry(typeof(Int64), DbType.Int64, SqlDbType.BigInt));
        }

        public virtual void typeLong()
        {
            typeList.Add(new DbTypeMapEntry(typeof(long), DbType.Int64, SqlDbType.BigInt));
        }

        public virtual void typeObject()
        {
            typeList.Add(new DbTypeMapEntry(typeof(object), DbType.Object, SqlDbType.Variant));
        }

        public virtual void typeString()
        {
            typeList.Add(new DbTypeMapEntry(typeof(string), DbType.String, SqlDbType.VarChar));
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
            typeLong();
            typeObject();
            typeString();

            //This one is run for any extra hooks we want to add in later versions
            extraType();
        }
    }
}