using System;
using System.Text;

namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*       GroupBy Compiling functions        */
        /*##########################################*/

        protected void GroupByCompile(Query theQuery, string tableName, string field)
        {
            //validation check
            GroupBySingleValidation(tableName, field);
            
            theQuery.groupbyFields.Add($"{tableName}.{field}");
        }

        protected void GroupByCompile(Query theQuery, string tableName, string[] fields)
        {
            //validation check
            GroupByMultipleValidation(tableName, fields);

            StringBuilder sb = new StringBuilder();
            
            for (int i = 0; i < fields.Length; i++)
            {
                //Determines if there should be a comma
                if (i != 0)
                {
                    sb.Append(", ");
                }

                sb.Append($"{tableName}.{fields[i]}");
            }

            theQuery.groupbyFields.Add(sb.ToString());
        }

        /*##########################################*/
        /*      GroupBy Validation functions        */
        /*##########################################*/

        protected void GroupBySingleValidation(string tableName, string field)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new Exception("Group Error: no table has been passed.");
            }
            else if (string.IsNullOrWhiteSpace(field))
            {
                throw new Exception("Group Error: no field has been passed.");
            }
        }

        protected void GroupByMultipleValidation(string tableName, string[] fields)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new Exception("Group Error: no table has been passed.");
            }
            else if (fields == null || fields.Length <= 0)
            {
                throw new Exception("Group Error: no fields has been passed.");
            }
        }

        protected void GroupByNotExistValidation(Query theQuery)
        {
            if (!theQuery.orderList.Contains("groupby"))
            {
                throw new Exception("Group Error: you cannot add group by fields without defining a main grpup by statement first.");
            }
        }

        protected void GroupByExistValidation(Query theQuery)
        {
            if (theQuery.orderList.Contains("groupby"))
            {
                throw new Exception("Group Error: a main group by statement has already been defined, for additional fields use add_groupby_fields.");
            }
        }

        /*##########################################*/
        /*            Main Front functions          */
        /*##########################################*/

        /// <summary>
        /// This function adds the main group by statement
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="field"></param>
        public void add_groupby(string tableName, string field)
        {
            Query theQuery = GetQuery();

            //Validation
            GroupByExistValidation(theQuery);

            GroupByCompile(theQuery, tableName, field);

            theQuery.orderList.Add("groupby");
        }

        /// <summary>
        /// This function adds the main group by statement
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fields"></param>
        public void add_groupby(string tableName, string[] fields)
        {
            Query theQuery = GetQuery();

            //Validation
            GroupByExistValidation(theQuery);

            GroupByCompile(theQuery, tableName, fields);

            theQuery.orderList.Add("groupby");
        }

        /// <summary>
        /// This function adds additional fields to the group by statement
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="field"></param>
        public void add_groupby_fields(string tableName, string field)
        {
            Query theQuery = GetQuery();

            //Validation
            GroupByNotExistValidation(theQuery);

            GroupByCompile(theQuery, tableName, field);
        }

        /// <summary>
        /// This function adds additional fields to the group by statement
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fields"></param>
        public void add_groupby_fields(string tableName, string[] fields)
        {
            Query theQuery = GetQuery();

            //Validation
            GroupByNotExistValidation(theQuery);

            GroupByCompile(theQuery, tableName, fields);
        }
    }
}