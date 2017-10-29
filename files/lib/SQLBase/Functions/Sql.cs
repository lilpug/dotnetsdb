using System;
using System.Collections.Generic;

namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*           Main Front function            */
        /*##########################################*/
        
        /// <summary>
        /// This function allows manual parameter binding and returns a string of the definition to input into the query
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public string add_pure_sql_bind(object values)
        {
            Query current = GetQuery();
            object[] holding = AddData(values);
            if (holding.Length > 1)
            {
                throw new Exception("add_pure_sql_bind Error: The data your trying to bind is an array not singular, please use the 'add_pure_sql_bind_array' function for this.");
            }
            current.CustomRealValues.Add(holding);
            return $"{customDefinition}_{theQueries.Count}_{current.CustomRealValues.Count - 1}_0";
        }

        /// <summary>
        /// This function allows an array of manual parameter binding and returns a string array of the definition to input into the query
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public string[] add_pure_sql_bind_array(object values)
        {
            Query current = GetQuery();
            object[] holding = AddData(values);
            current.CustomRealValues.Add(holding);

            if (holding.Length > 1)
            {
                List<string> defs = new List<string>();
                for (int i = 0; i < holding.Length; i++)
                {
                    defs.Add($"{customDefinition}_{theQueries.Count}_{current.CustomRealValues.Count - 1}_{i}");
                }
                return defs.ToArray();
            }
            else
            {
                return new string[] { $"{customDefinition}_{theQueries.Count}_{current.CustomRealValues.Count - 1}_0" };
            }
        }

        /// <summary>
        /// This function adds an sql query to the current query object.
        /// </summary>
        /// <param name="sqlQuery"></param>
        public void add_pure_sql(string sqlQuery)
        {
            Query theQuery = GetQuery();

            theQuery.PureSql.Add(sqlQuery);
            theQuery.OrderList.Add("pure_sql");
        }

        /// <summary>
        /// <para>This function adds a sql statement to the start of the current query object being processed.</para>
        /// <para>Note: This function can only be used once per new query, using it multiple times will wipe the previous data until a new query object 'start_new_query()'.</para>
        /// </summary>
        /// <param name="sqlQuery"></param>
        public void add_start_sql_wrapper(string sqlQuery)
        {
            Query theQuery = GetQuery();
            theQuery.SqlStartWrapper = sqlQuery;
        }

        /// <summary>
        /// <para>This function adds a sql statement to the end of the current query object being processed.</para>
        /// <para>Note: This function can only be used once per new query, using it multiple times will wipe the previous data until a new query object 'start_new_query()'.</para>
        /// </summary>
        /// <param name="sqlQuery"></param>
        public void add_end_sql_wrapper(string sqlQuery)
        {
            Query theQuery = GetQuery();
            theQuery.SqlEndWrapper = sqlQuery;
        }
    }
}