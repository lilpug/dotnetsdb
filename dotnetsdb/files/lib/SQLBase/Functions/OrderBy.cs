using System;

namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*       OrderBy Compiling functions        */
        /*##########################################*/

        //This function builds the fields for the orderBy
        protected void orderBy_build_compiling(query theQuery, string tableName, string field, string type = null)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new Exception("OrderBy Error: The table name supplied is empty.");
            }

            if (!string.IsNullOrWhiteSpace(field))
            {
                string temp_build = "";

                //Builds the string
                if (!string.IsNullOrWhiteSpace(type))
                {
                    temp_build += string.Format("{0}.{1} {2}", tableName, field, type);
                }
                else
                {
                    temp_build += string.Format("{0}.{1}", tableName, field);
                }

                //Compiles the final build of the order by sql
                theQuery.orderby_fields.Add(temp_build);
            }
            else
            {
                throw new Exception("OrderBy Error: The fields parameter is empty.");
            }
        }

        protected void orderBy_build_compiling(query theQuery, string tableName, string[] fields, string[] types = null)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new Exception("OrderBy Error: The table name supplied is empty.");
            }

            if (fields != null && fields.Length > 0)
            {
                if (types != null && fields.Length != types.Length)
                {
                    throw new Exception("OrderBy Error: When using an orderby and ordering types there must be an equal amount of both fields and types.");
                }

                string temp_build = "";
                for (int i = 0; i < fields.Length; i++)
                {
                    //Determines if there should be a comma
                    string seperator = "";
                    if (i != 0)
                    {
                        seperator = ", ";
                    }

                    //Builds the string
                    if (types != null)
                    {
                        temp_build += string.Format("{0}{1}.{2} {3}", seperator, tableName, fields[i], types[i]);
                    }
                    else
                    {
                        temp_build += string.Format("{0}{1}.{2}", seperator, tableName, fields[i]);
                    }
                }

                //Compiles the final build of the order by sql
                theQuery.orderby_fields.Add(temp_build);
            }
            else
            {
                throw new Exception("OrderBy Error: The fields parameter is empty.");
            }
        }

        /*##########################################*/
        /*           Main Front functions           */
        /*##########################################*/

        /// <summary>
        /// This function creates the main orderby statement.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="field"></param>
        /// <param name="type"></param>
        public void add_orderby(string tableName, string field, string type = null)
        {
            query theQuery = get_query();

            if (theQuery.orderList.Contains("orderby"))
            {
                throw new Exception("OrderBy Error: a main orderby statement has already been defined, for additional fields use add_orderby_fields.");
            }

            //Builds the order by sql into the query
            orderBy_build_compiling(theQuery, tableName, field, type);

            //Adds the command
            theQuery.orderList.Add("orderby");
        }

        /// <summary>
        /// This function creates the main orderby statement.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fields"></param>
        /// <param name="types"></param>
        public void add_orderby(string tableName, string[] fields, string[] types = null)
        {
            query theQuery = get_query();

            if (theQuery.orderList.Contains("orderby"))
            {
                throw new Exception("OrderBy Error: a main orderby statement has already been defined, for additional fields use add_orderby_fields.");
            }

            //Builds the order by sql into the query
            orderBy_build_compiling(theQuery, tableName, fields, types);

            //Adds the command
            theQuery.orderList.Add("orderby");
        }

        /// <summary>
        /// This function adds an additional field to the orderby statement
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="field"></param>
        /// <param name="type"></param>
        public void add_orderby_fields(string tableName, string field, string type)
        {
            query theQuery = get_query();

            if (!theQuery.orderList.Contains("orderby"))
            {
                throw new Exception("OrderBy Error: you cannot add additional fields and types without defining a main orderby statement first.");
            }

            //Builds the order by sql into the query
            orderBy_build_compiling(theQuery, tableName, field, type);
        }

        /// <summary>
        /// This function adds additional fields to the orderby statement
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fields"></param>
        /// <param name="types"></param>
        public void add_orderby_fields(string tableName, string[] fields, string[] types)
        {
            query theQuery = get_query();

            if (!theQuery.orderList.Contains("orderby"))
            {
                throw new Exception("OrderBy Error: you cannot add additional fields and types without defining a main orderby statement first.");
            }

            //Builds the order by sql into the query
            orderBy_build_compiling(theQuery, tableName, fields, types);
        }
    }
}