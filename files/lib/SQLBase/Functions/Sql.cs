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
            current.customRealValues.Add(holding);
            return string.Format("{0}_{1}_{2}_0", customDefinition, theQueries.Count.ToString(), (current.customRealValues.Count - 1).ToString());
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
            current.customRealValues.Add(holding);

            if (holding.Length > 1)
            {
                List<string> defs = new List<string>();
                for (int i = 0; i < holding.Length; i++)
                {
                    defs.Add(string.Format("{0}_{1}_{2}_{3}", customDefinition, theQueries.Count.ToString(), (current.customRealValues.Count - 1).ToString(), i.ToString()));
                }
                return defs.ToArray();
            }
            else
            {
                return new string[] { string.Format("{0}_{1}_{2}_0", customDefinition, theQueries.Count.ToString(), (current.customRealValues.Count - 1).ToString()) };
            }
        }

        /// <summary>
        /// This function allows an array of manual parameter binding and returns a concatenated string of the definitions to input into the query
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public string add_pure_sql_bind_array_to_string(object values)
        {
            var bindings = add_pure_sql_bind_array(values);
            return string.Join(",", bindings);
        }

        /// <summary>
        /// This function adds an sql query to the current query object.
        /// </summary>
        /// <param name="sqlQuery"></param>
        public void add_pure_sql(string sqlQuery)
        {
            Query theQuery = GetQuery();

            theQuery.pureSql.Add(sqlQuery);
            theQuery.orderList.Add("pure_sql");
        }

        /// <summary>
        /// <para>This function adds a sql statement to the start of the current query object being processed.</para>
        /// <para>Note: This function can only be used once per new query, using it multiple times will wipe the previous data until a new query object 'start_new_query()'.</para>
        /// </summary>
        /// <param name="sqlQuery"></param>
        public void add_start_sql_wrapper(string sqlQuery)
        {
            Query theQuery = GetQuery();
            theQuery.sqlStartWrapper = sqlQuery;
        }

        /// <summary>
        /// <para>This function adds a sql statement to the end of the current query object being processed.</para>
        /// <para>Note: This function can only be used once per new query, using it multiple times will wipe the previous data until a new query object 'start_new_query()'.</para>
        /// </summary>
        /// <param name="sqlQuery"></param>
        public void add_end_sql_wrapper(string sqlQuery)
        {
            Query theQuery = GetQuery();
            theQuery.sqlEndWrapper = sqlQuery;
        }
    }
}