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
            _DbTypeList.Add(new DbTypeMapEntry(typeof(bool), DbType.Boolean, SqlDbType.Bit));
        }

        public virtual void typeByte()
        {
            _DbTypeList.Add(new DbTypeMapEntry(typeof(byte), DbType.Binary, SqlDbType.TinyInt));
        }

        public virtual void typeByteArray()
        {
            _DbTypeList.Add(new DbTypeMapEntry(typeof(byte[]), DbType.Binary, SqlDbType.VarBinary));
        }

        public virtual void typeDateTime()
        {
            _DbTypeList.Add(new DbTypeMapEntry(typeof(DateTime), DbType.DateTime, SqlDbType.DateTime));
        }

        public virtual void typeDecimal()
        {
            _DbTypeList.Add(new DbTypeMapEntry(typeof(Decimal), DbType.Decimal, SqlDbType.Decimal));
        }

        public virtual void typeDouble()
        {
            _DbTypeList.Add(new DbTypeMapEntry(typeof(double), DbType.Double, SqlDbType.Float));
        }

        public virtual void typeGUID()
        {
            _DbTypeList.Add(new DbTypeMapEntry(typeof(Guid), DbType.Guid, SqlDbType.UniqueIdentifier));
        }

        public virtual void typeInt16()
        {
            _DbTypeList.Add(new DbTypeMapEntry(typeof(Int16), DbType.Int16, SqlDbType.SmallInt));
        }

        public virtual void typeInt32()
        {
            _DbTypeList.Add(new DbTypeMapEntry(typeof(Int32), DbType.Int32, SqlDbType.Int));
        }

        public virtual void typeInt64()
        {
            _DbTypeList.Add(new DbTypeMapEntry(typeof(Int64), DbType.Int64, SqlDbType.BigInt));
        }

        public virtual void typeLong()
        {
            _DbTypeList.Add(new DbTypeMapEntry(typeof(long), DbType.Int64, SqlDbType.BigInt));
        }

        public virtual void typeObject()
        {
            _DbTypeList.Add(new DbTypeMapEntry(typeof(object), DbType.Object, SqlDbType.Variant));
        }

        public virtual void typeString()
        {
            _DbTypeList.Add(new DbTypeMapEntry(typeof(string), DbType.String, SqlDbType.VarChar));
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