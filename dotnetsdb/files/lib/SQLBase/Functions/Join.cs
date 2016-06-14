using System;
using System.Linq;

namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*         Join Compiling functions         */
        /*##########################################*/

        protected void join_build_compiling(query theQuery, string table_name, string join_type, string other_table_name, string join_field, string other_field)
        {
            //Does validation
            join_single_validation(join_type, table_name, other_table_name, join_field, other_field);

            //This is done to ensure if we have an alias we use it otherwise we use the generic
            string[] mainTables = table_name.Split(' ');
            string alias = (mainTables.Count() > 1) ? mainTables[1] : mainTables[0];

            string temp_build = string.Format("{0} {1} ON {2}.{3} = {4}.{5}", join_type, table_name, alias, join_field, other_table_name, other_field);

            //adds it to the join statement lists
            theQuery.join_fields.Add(temp_build);
        }

        protected void join_build_compiling(query theQuery, string table_name, string join_type, string other_table_name, string[] join_fields, string[] other_fields)
        {
            //Does validation
            join_multiple_validation(join_type, table_name, other_table_name, join_fields, other_fields);

            //This is done to ensure if we have an alias we use it otherwise we use the generic
            string[] mainTables = table_name.Split(' ');
            string alias = (mainTables.Count() > 1) ? mainTables[1] : mainTables[0];

            string temp_build = "";
            for (int i = 0; i < join_fields.Length; i++)
            {
                //Determines if there should be a an and for double join statements
                string seperator = "";
                if (i != 0)
                {
                    seperator = "AND ";
                }

                temp_build += string.Format("{0}{1} {2} ON {3}.{4} = {5}.{6}", seperator, join_type, table_name, alias, join_fields[i], other_table_name, other_fields[i]);
            }

            //adds it to the join statement lists
            theQuery.join_fields.Add(temp_build);
        }

        protected void join_additional_parameters_compiling(query theQuery, string table_name, string other_table_name, string join_field, string other_field)
        {
            //Does validation
            join_single_validation("exclude", table_name, other_table_name, join_field, other_field);

            //This is done to ensure if we have an alias we use it otherwise we use the generic
            string[] mainTables = table_name.Split(' ');
            string alias = (mainTables.Count() > 1) ? mainTables[1] : mainTables[0];

            string temp_build = string.Format("{0}{1}.{2} = {3}.{4}", " AND ", alias, join_field, other_table_name, other_field);

            //Adds it to the current join object
            theQuery.join_fields[theQuery.join_fields.Count - 1] = theQuery.join_fields[theQuery.join_fields.Count - 1] + temp_build;
        }

        protected void join_additional_parameters_compiling(query theQuery, string table_name, string other_table_name, string[] join_fields, string[] other_fields)
        {
            //Does validation
            join_multiple_validation("exclude", table_name, other_table_name, join_fields, other_fields);

            //This is done to ensure if we have an alias we use it otherwise we use the generic
            string[] mainTables = table_name.Split(' ');
            string alias = (mainTables.Count() > 1) ? mainTables[1] : mainTables[0];

            string temp_build = "";
            for (int i = 0; i < join_fields.Length; i++)
            {
                temp_build += string.Format("{0}{1}.{2} = {3}.{4}", " AND ", alias, join_fields[i], other_table_name, other_fields[i]);
            }

            //Adds it to the current join object
            theQuery.join_fields[theQuery.join_fields.Count - 1] = theQuery.join_fields[theQuery.join_fields.Count - 1] + temp_build;
        }

        /*##########################################*/
        /*        Join Validation functions         */
        /*##########################################*/

        protected void join_single_validation(string joinType, string joinTableName, string currentTableName, string joinTableFields, string currentTableFields)
        {
            if (string.IsNullOrWhiteSpace(joinType))
            {
                throw new Exception("Join Error: No join type has been supplied i.e. 'inner join'.");
            }
            else if (string.IsNullOrWhiteSpace(joinTableName))
            {
                throw new Exception("Join Error: No join table has been supplied.");
            }
            else if (string.IsNullOrWhiteSpace(currentTableName))
            {
                throw new Exception("Join Error: No current table has been supplied.");
            }
            else if (string.IsNullOrWhiteSpace(joinTableFields))
            {
                throw new Exception("Join Error: No join field has been supplied.");
            }
            else if (string.IsNullOrWhiteSpace(currentTableFields))
            {
                throw new Exception("Join Error: No current table field has been supplied.");
            }
        }

        protected void join_multiple_validation(string joinType, string joinTableName, string currentTableName, string[] joinTableFields, string[] currentTableFields)
        {
            if (string.IsNullOrWhiteSpace(joinType))
            {
                throw new Exception("Join Error: No join type has been supplied i.e. 'inner join'.");
            }
            else if (string.IsNullOrWhiteSpace(joinTableName))
            {
                throw new Exception("Join Error: No join table has been supplied.");
            }
            else if (string.IsNullOrWhiteSpace(currentTableName))
            {
                throw new Exception("Join Error: No current table has been supplied.");
            }
            else if (joinTableFields.Count() < 1)
            {
                throw new Exception("Join Error: No join fields have been supplied.");
            }
            else if (currentTableFields.Count() < 1)
            {
                throw new Exception("Join Error: No current table fields have been supplied.");
            }
            else if (joinTableFields.Count() != currentTableFields.Count())
            {
                throw new Exception("Join Error: The join fields and the current table fields do not have the same index amount.");
            }
        }

        protected void join_exist_validation(query theQuery)
        {
            if (theQuery.join_fields == null || theQuery.join_fields.Count <= 0)
            {
                throw new Exception("Join Error: There is no join statement to add additional parameters.");
            }
        }

        /*##########################################*/
        /*           Main Front functions           */
        /*##########################################*/

        /// <summary>
        /// <para>This function adds a join into the query.</para>
        /// <para>Note: Please remember to use 'JOIN' within the joinType i.e. 'INNER JOIN'</para>
        /// </summary>
        /// <param name="joinType">Please enter the join type here i.e. 'INNER JOIN'</param>
        /// <param name="joinTableName">Please enter the new table you wish to start your join on</param>
        /// <param name="currentTableName">Please enter your current table you are linking with</param>
        /// <param name="joinTableField">Please enter the field you are linking on the new table</param>
        /// <param name="currentTableField">Please enter the field you are linking on in the current table</param>
        public void add_join(string joinType, string joinTableName, string currentTableName, string joinTableField, string currentTableField)
        {
            //Gets the current query object
            query theQuery = get_query();

            //Builds the sql from the parameters
            join_build_compiling(theQuery, joinTableName, joinType, currentTableName, joinTableField, currentTableField);

            //Adds the join command
            theQuery.orderList.Add("join");
        }

        /// <summary>
        /// <para>This function adds a join into the query.</para>
        /// <para>Note: Please remember to use 'JOIN' within the joinType i.e. 'INNER JOIN'</para>
        /// </summary>
        /// <param name="joinType">Please enter the join type here i.e. 'INNER JOIN'</param>
        /// <param name="joinTableName">Please enter the new table you wish to start your join on</param>
        /// <param name="currentTableName">Please enter your current table you are linking with</param>
        /// <param name="joinTableFields">Please enter the fields you are linking on the new table</param>
        /// <param name="currentTableFields">Please enter the fields you are linking on in the current table</param>
        public void add_join(string joinType, string joinTableName, string currentTableName, string[] joinTableFields, string[] currentTableFields)
        {
            //Gets the current query object
            query theQuery = get_query();

            //Builds the sql from the parameters
            join_build_compiling(theQuery, joinTableName, joinType, currentTableName, joinTableFields, currentTableFields);

            //Adds the join command
            theQuery.orderList.Add("join");
        }

        /// <summary>
        /// <para>This function adds additional parameters to the previous join.</para>
        /// </summary>
        /// <param name="joinTableName">Please enter the new table you wish to start your join on</param>
        /// <param name="currentTableName">Please enter your current table you are linking with</param>
        /// <param name="joinTableFields">Please enter the fields you are linking on the new table</param>
        /// <param name="currentTableFields">Please enter the fields you are linking on in the current table</param>
        public void add_join_parameters(string joinTableName, string currentTableName, string[] joinTableFields, string[] currentTableFields)
        {
            //Gets the current query object
            query theQuery = get_query();

            join_exist_validation(theQuery);

            //Builds the additional sql from the parameters
            join_additional_parameters_compiling(theQuery, joinTableName, currentTableName, joinTableFields, currentTableFields);
        }

        /// <summary>
        /// <para>This function adds additional parameters to the previous join.</para>
        /// </summary>
        /// <param name="joinTableName">Please enter the new table you wish to start your join on</param>
        /// <param name="currentTableName">Please enter your current table you are linking with</param>
        /// <param name="joinTableField">Please enter the field you are linking on the new table</param>
        /// <param name="currentTableField">Please enter the field you are linking on in the current table</param>
        public void add_join_parameters(string joinTableName, string currentTableName, string joinTableField, string currentTableField)
        {
            //Gets the current query object
            query theQuery = get_query();

            join_exist_validation(theQuery);

            //Builds the additional sql from the parameters
            join_additional_parameters_compiling(theQuery, joinTableName, currentTableName, joinTableField, currentTableField);
        }
    }
}