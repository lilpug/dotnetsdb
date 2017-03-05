﻿using System;

namespace DotNetSDB
{
    public partial class SqlServerCore
    {
        /*##########################################*/
        /*             Main Front function          */
        /*##########################################*/
        
        /// <summary>
        /// This function adds the insert statement without fields or values
        /// </summary>
        /// <param name="tableName"></param>
        public virtual void add_insert_return(string tableName)
        {
            //Runs the original insert
            add_insert(tableName);

            Query2 theQuery2 = GetQuery2();

            //Adds the new return feature
            theQuery2.insertReturn = true;
        }

        /// <summary>
        /// This functions adds the insert statement with a field only
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="field"></param>
        public virtual void add_insert_return(string tableName, string field)
        {
            //Runs the original insert
            add_insert(tableName, field);
            
            Query2 theQuery2 = GetQuery2();

            //Adds the new return feature
            theQuery2.insertReturn = true;
        }

        /// <summary>
        /// This functions adds the insert statement with fields only
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fields"></param>
        public virtual void add_insert_return(string tableName, string[] fields)
        {
            //Runs the original insert
            add_insert(tableName, fields);

            Query2 theQuery2 = GetQuery2();

            //Adds the new return feature
            theQuery2.insertReturn = true;
        }

        /// <summary>
        /// This function adds the insert statement with only values
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="values">single value or object[] only</param>
        public virtual void add_insert_return(string tableName, object values)
        {
            //Runs the original insert
            add_insert(tableName, values);

            Query2 theQuery2 = GetQuery2();

            //Adds the new return feature
            theQuery2.insertReturn = true;
        }

        /// <summary>
        /// This function adds the insert statement with a field and value
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="field"></param>
        /// <param name="value">single value only</param>
        public virtual void add_insert_return(string tableName, string field, object value)
        {
            //Runs the original insert
            add_insert(tableName, field, value);

            Query2 theQuery2 = GetQuery2();

            //Adds the new return feature
            theQuery2.insertReturn = true;
        }

        /// <summary>
        /// This function adds the insert statement with the fields and values
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fields"></param>
        /// <param name="values">object[] only</param>
        public virtual void add_insert_return(string tableName, string[] fields, object values)
        {
            //Runs the original insert
            add_insert(tableName, fields, values);

            Query2 theQuery2 = GetQuery2();

            //Adds the new return feature
            theQuery2.insertReturn = true;
        }
    }
}