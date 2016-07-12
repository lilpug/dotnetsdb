using System;

namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*        Select Creation functions         */
        /*##########################################*/

        //This function builds the fields for the select statement
        protected void add_new_fields(query the_query, string table_name, string select_field, string select_start_field = null, string select_end_field = null)
        {
            select_single_validation(table_name);

            //Checks if there is any select fields
            if (select_field != null)
            {
                //Checks if the sizes are not the same and if not returns a false
                if (
                    (select_start_field != null && select_end_field != null) &&
                    ((!string.IsNullOrWhiteSpace(select_start_field) && string.IsNullOrWhiteSpace(select_end_field)) || (string.IsNullOrWhiteSpace(select_start_field) && !string.IsNullOrWhiteSpace(select_end_field)))
                    )
                {
                    throw new Exception("Select Error: When using a start or end parameter you must supply both");
                }

                //Do not change the main table name if its already set
                if (string.IsNullOrWhiteSpace(the_query.select_table) && !string.IsNullOrWhiteSpace(table_name))
                {
                    //Sets the table name
                    the_query.select_table = table_name;
                }

                string temp_build = "";

                //Builds the string
                if (select_start_field != null && select_end_field != null)
                {
                    temp_build += string.Format("{0} {1}.{2} {3}", select_start_field, table_name, select_field, select_end_field);
                }
                else
                {
                    temp_build += string.Format("{0}.{1}", table_name, select_field);
                }

                the_query.select_fields.Add(temp_build);
            }
            else
            {
                throw new Exception("Select Error: The fields parameter is empty.");
            }
        }

        protected void add_new_fields(query the_query, string table_name, string[] select_fields, string[] select_start_fields = null, string[] select_end_fields = null)
        {
            select_single_validation(table_name);

            //Checks if there is any select fields
            if (select_fields != null)
            {
                //Checks if the sizes are not the same and if not returns a false
                if (
                    (select_start_fields != null && select_end_fields != null) &&
                    (select_fields.Length != select_start_fields.Length || select_fields.Length != select_end_fields.Length)
                    )
                {
                    throw new Exception("Select Error: When using multiple start or end parameters you must supply an equal amount of both");
                }

                //Do not change the main table name if its already set
                if (string.IsNullOrWhiteSpace(the_query.select_table) && !string.IsNullOrWhiteSpace(table_name))
                {
                    //Sets the table name
                    the_query.select_table = table_name;
                }

                string temp_build = "";
                for (int i = 0; i < select_fields.Length; i++)
                {
                    //Determines if there should be a comma
                    string seperator = "";
                    if (i != 0)
                    {
                        seperator = ", ";
                    }

                    //Builds the string
                    if (select_start_fields != null && select_end_fields != null)
                    {
                        temp_build += string.Format("{0}{1} {2}.{3} {4}", seperator, select_start_fields[i], table_name, select_fields[i], select_end_fields[i]);
                    }
                    else
                    {
                        temp_build += string.Format("{0}{1}.{2}", seperator, table_name, select_fields[i]);
                    }
                }
                the_query.select_fields.Add(temp_build);
            }
            else
            {
                throw new Exception("Select Error: The fields parameter is empty.");
            }
        }

        /*##########################################*/
        /*       Select Validation functions        */
        /*##########################################*/

        protected void select_single_validation(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new Exception("Select Error: The table name supplied is empty.");
            }
        }

        protected void select_not_exist_validation(query theQuery)
        {
            if (!theQuery.orderList.Contains("select"))
            {
                throw new Exception("Select Error: you cannot add select fields without defining a main select statement first.");
            }
        }

        protected void select_exist_validation(query theQuery)
        {
            if (theQuery.orderList.Contains("select"))
            {
                throw new Exception("Select Error: a main select statement has already been defined, for additional fields use add_select_fields.");
            }
        }

        /*##########################################*/
        /*           Main Front function            */
        /*##########################################*/

        /// <summary>
        /// This function determines whether the select statement should be be distinct
        /// </summary>
        /// <param name="distinct"></param>
        public void is_distinct(bool distinct)
        {
            query theQuery = get_query();
            theQuery.is_dinstinct = distinct;
        }

        /// <summary>
        /// This functions adds an additional select field to a select statement
        /// </summary>
        /// <param name="table_name"></param>
        /// <param name="select_fields"></param>
        /// <param name="select_start_fields"></param>
        /// <param name="select_end_fields"></param>
        public void add_select_fields(string table_name, string select_field, string select_start_field = null, string select_end_field = null)
        {
            //Obtains the current query object
            query theQuery = get_query();

            select_not_exist_validation(theQuery);

            //Builds the select_fields sql
            add_new_fields(theQuery, table_name, select_field, select_start_field, select_end_field);
        }

        /// <summary>
        /// This functions adds additional select fields to a select statement
        /// </summary>
        /// <param name="table_name"></param>
        /// <param name="select_fields"></param>
        /// <param name="select_start_fields"></param>
        /// <param name="select_end_fields"></param>
        public void add_select_fields(string table_name, string[] select_fields, string[] select_start_fields = null, string[] select_end_fields = null)
        {
            //Obtains the current query object
            query theQuery = get_query();

            select_not_exist_validation(theQuery);

            //Builds the select_fields sql
            add_new_fields(theQuery, table_name, select_fields, select_start_fields, select_end_fields);
        }

        /// <summary>
        /// This functions adds a select statement
        /// </summary>
        /// <param name="table_name"></param>
        /// <param name="select_fields"></param>
        /// <param name="select_start_fields"></param>
        /// <param name="select_end_fields"></param>
        public void add_select(string table_name, string select_field, string select_start_field = null, string select_end_field = null)
        {
            query theQuery = get_query();

            select_exist_validation(theQuery);

            //Builds the select_fields sql
            add_new_fields(theQuery, table_name, select_field, select_start_field, select_end_field);

            //Adds the command
            theQuery.orderList.Add("select");
        }

        /// <summary>
        /// This functions adds a select statement
        /// </summary>
        /// <param name="table_name"></param>
        /// <param name="select_fields"></param>
        /// <param name="select_start_fields"></param>
        /// <param name="select_end_fields"></param>
        public void add_select(string table_name, string[] select_fields, string[] select_start_fields = null, string[] select_end_fields = null)
        {
            query theQuery = get_query();

            select_exist_validation(theQuery);

            //Builds the select_fields sql
            add_new_fields(theQuery, table_name, select_fields, select_start_fields, select_end_fields);

            //Adds the command
            theQuery.orderList.Add("select");
        }
    }
}