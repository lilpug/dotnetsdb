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
        /// <summary>
        /// Adds the boolean type conversions
        /// </summary>
        public virtual void TypeBool()
        {
            TypeList.Add(new DbTypeMapEntry(typeof(bool), DbType.Boolean, SqlDbType.Bit));
        }

        /// <summary>
        /// Adds the byte type conversions
        /// </summary>
        public virtual void TypeByte()
        {
            TypeList.Add(new DbTypeMapEntry(typeof(byte), DbType.Binary, SqlDbType.TinyInt));
        }

        /// <summary>
        /// Adds the byte array type conversions
        /// </summary>
        public virtual void TypeByteArray()
        {
            TypeList.Add(new DbTypeMapEntry(typeof(byte[]), DbType.Binary, SqlDbType.VarBinary));
        }

        /// <summary>
        /// Adds the datetime type conversions
        /// </summary>
        public virtual void TypeDateTime()
        {
            TypeList.Add(new DbTypeMapEntry(typeof(DateTime), DbType.DateTime, SqlDbType.DateTime));
        }

        /// <summary>
        /// Adds the decimal type conversions
        /// </summary>
        public virtual void TypeDecimal()
        {
            TypeList.Add(new DbTypeMapEntry(typeof(Decimal), DbType.Decimal, SqlDbType.Decimal));
        }

        /// <summary>
        /// Adds the double type conversions
        /// </summary>
        public virtual void TypeDouble()
        {
            TypeList.Add(new DbTypeMapEntry(typeof(double), DbType.Double, SqlDbType.Float));
        }

        /// <summary>
        /// Adds the guid type conversions
        /// </summary>
        public virtual void TypeGUID()
        {
            TypeList.Add(new DbTypeMapEntry(typeof(Guid), DbType.Guid, SqlDbType.UniqueIdentifier));
        }

        /// <summary>
        /// Adds the int16 type conversions
        /// </summary>
        public virtual void TypeInt16()
        {
            TypeList.Add(new DbTypeMapEntry(typeof(Int16), DbType.Int16, SqlDbType.SmallInt));
        }

        /// <summary>
        /// Adds the int32 type conversions
        /// </summary>
        public virtual void TypeInt32()
        {
            TypeList.Add(new DbTypeMapEntry(typeof(Int32), DbType.Int32, SqlDbType.Int));
        }

        /// <summary>
        /// Adds the int64 type conversions
        /// </summary>
        public virtual void TypeInt64()
        {
            TypeList.Add(new DbTypeMapEntry(typeof(Int64), DbType.Int64, SqlDbType.BigInt));
        }

        /// <summary>
        /// Adds the long type conversions
        /// </summary>
        public virtual void TypeLong()
        {
            TypeList.Add(new DbTypeMapEntry(typeof(long), DbType.Int64, SqlDbType.BigInt));
        }

        /// <summary>
        /// Adds the object type conversions
        /// </summary>
        public virtual void TypeObject()
        {
            TypeList.Add(new DbTypeMapEntry(typeof(object), DbType.Object, SqlDbType.Variant));
        }

        /// <summary>
        /// Adds the string type conversions
        /// </summary>
        public virtual void TypeString()
        {
            TypeList.Add(new DbTypeMapEntry(typeof(string), DbType.String, SqlDbType.VarChar));
        }

        /// <summary>
        /// Adds the datatable type conversions
        /// </summary>
        public virtual void TypeTable()
        {
            TypeList.Add(new DbTypeMapEntry(typeof(DataTable), DbType.Object, SqlDbType.Structured));
        }

        /// <summary>
        /// Adds any additional types which might be put in through inheritance
        /// </summary>
        public virtual void ExtraType()
        {
        }

        /// <summary>
        /// This function is the loading function for the types
        /// </summary>
        private void LoadTypes()
        {
            TypeBool();
            TypeByte();
            TypeByteArray();
            TypeDateTime();
            TypeDecimal();
            TypeDouble();
            TypeGUID();
            TypeInt16();
            TypeInt32();
            TypeInt64();
            TypeLong();
            TypeObject();
            TypeString();
            TypeTable();

            //This one is run for any extra hooks we want to add in later versions
            ExtraType();
        }
    }
}