using System;

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

            string temp_build = string.Format("{0}.{1}", tableName, field);

            theQuery.groupbyFields.Add(temp_build);
        }

        protected void GroupByCompile(Query theQuery, string tableName, string[] fields)
        {
            //validation check
            GroupByMultipleValidation(tableName, fields);

            string temp_build = "";
            for (int i = 0; i < fields.Length; i++)
            {
                //Determines if there should be a comma
                string seperator = "";
                if (i != 0)
                {
                    seperator = ", ";
                }

                temp_build += string.Format("{0}{1}.{2}", seperator, tableName, fields[i]);
            }

            theQuery.groupbyFields.Add(temp_build);
        }

        /*##########################################*/
        /*      GroupBy Validation functions        */
        /*##########################################*/

        protected void GroupBySingleValidation(string tableName, string field)
        {
            if (!string.IsNullOrWhiteSpace(tableName))
            {
                throw new Exception("Group Error: no table has been passed.");
            }
            else if (!string.IsNullOrWhiteSpace(field))
            {
                throw new Exception("Group Error: no field has been passed.");
            }
        }

        protected void GroupByMultipleValidation(string tableName, string[] fields)
        {
            if (!string.IsNullOrWhiteSpace(tableName))
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