using System;
using System.Collections.Generic;

namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*           Main Front function            */
        /*##########################################*/

        //This function allows manual binding to occur on pure sql

        /// <summary>
        /// This function allows manual parameter binding and returns a string of the definition to input into the query
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public string add_pure_sql_bind(object values)
        {
            query current = this.get_query();
            object[] holding = add_data(values);
            if (holding.Length > 1)
            {
                throw new Exception("CustomParameterBind Error: The data your trying to bind is an array not singular, please use the 'CustomParameterBindArrayType' function for this.");
            }
            current.custom_real_values.Add(holding);
            return custom_definition + "_" + theQueries.Count.ToString() + "_" + (current.custom_real_values.Count - 1).ToString() + "_" + "0";
        }

        //This function allows manual binding to occur with array types on pure sql

        /// <summary>
        /// This function allows manual parameter binding and returns a string[] of the definition to input into the query
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public string[] add_pure_sql_bind_array(object values)
        {
            query current = this.get_query();
            object[] holding = add_data(values);
            current.custom_real_values.Add(holding);

            if (holding.Length > 1)
            {
                List<string> defs = new List<string>();
                for (int i = 0; i < holding.Length; i++)
                {
                    defs.Add(custom_definition + "_" + theQueries.Count.ToString() + "_" + (current.custom_real_values.Count - 1).ToString() + "_" + i.ToString());
                }
                return defs.ToArray();
            }
            else
            {
                return new string[] { custom_definition + "_" + theQueries.Count.ToString() + "_" + (current.custom_real_values.Count - 1).ToString() + "_" + "0" };
            }
        }

        /// <summary>
        /// This function adds an sql query to the current query object.
        /// </summary>
        /// <param name="sqlQuery"></param>
        public void add_pure_sql(string sqlQuery)
        {
            query theQuery = get_query();

            theQuery.pure_sql.Add(sqlQuery);
            theQuery.orderList.Add("pure_sql");
        }

        /// <summary>
        /// <para>This function adds a sql statement to the start of the current query object being processed.</para>
        /// <para>Note: This function can only be used once per new query, using it multiple times will wipe the previous data until a new query object 'start_new_query()'.</para>
        /// </summary>
        /// <param name="sqlQuery"></param>
        public void add_start_sql_wrapper(string sqlQuery)
        {
            query theQuery = get_query();
            theQuery.sql_start_wrapper = sqlQuery;
        }

        /// <summary>
        /// <para>This function adds a sql statement to the end of the current query object being processed.</para>
        /// <para>Note: This function can only be used once per new query, using it multiple times will wipe the previous data until a new query object 'start_new_query()'.</para>
        /// </summary>
        /// <param name="sqlQuery"></param>
        public void add_end_sql_wrapper(string sqlQuery)
        {
            query theQuery = get_query();
            theQuery.sql_end_wrapper = sqlQuery;
        }
    }
}