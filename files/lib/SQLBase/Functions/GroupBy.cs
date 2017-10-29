using System;
using System.Text;

namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*       GroupBy Compiling functions        */
        /*##########################################*/

        /// <summary>
        /// This function deals with creating the group by SQL
        /// </summary>
        /// <param name="theQuery"></param>
        /// <param name="tableName"></param>
        /// <param name="field"></param>
        protected void GroupByCompile(Query theQuery, string tableName, string field)
        {
            //validation check
            GroupBySingleValidation(tableName, field);
            
            theQuery.GroupbyFields.Add($"{tableName}.{field}");
        }

        /// <summary>
        /// This function deals with creating the group by SQL
        /// </summary>
        /// <param name="theQuery"></param>
        /// <param name="tableName"></param>
        /// <param name="fields"></param>
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

            theQuery.GroupbyFields.Add(sb.ToString());
        }

        /*##########################################*/
        /*      GroupBy Validation functions        */
        /*##########################################*/

        /// <summary>
        /// This function validates the group by variables
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="field"></param>
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

        /// <summary>
        /// This function validates the group by variables
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fields"></param>
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

        /// <summary>
        /// This function validates that the group by base statement has been added before trying to add additional fields
        /// </summary>
        /// <param name="theQuery"></param>
        protected void GroupByNotExistValidation(Query theQuery)
        {
            if (!theQuery.OrderList.Contains("groupby"))
            {
                throw new Exception("Group Error: you cannot add group by fields without defining a main group by statement first.");
            }
        }

        /// <summary>
        /// This function validates that the group by query has not already been run as a new one is about to be added
        /// </summary>
        /// <param name="theQuery"></param>
        protected void GroupByExistValidation(Query theQuery)
        {
            if (theQuery.OrderList.Contains("groupby"))
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

            theQuery.OrderList.Add("groupby");
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

            theQuery.OrderList.Add("groupby");
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