using System;
using System.Linq;

namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*         Join Compiling functions         */
        /*##########################################*/

        protected void JoinCompile(Query theQuery, string tableName, string joinType, string joinTable, string field, string joinField)
        {
            //Does validation
            JoinSingleValidation(joinType, tableName, joinTable, field, joinField);

            //This is done to ensure if we have an alias we use it otherwise we use the generic
            string[] mainTables = tableName.Split(' ');
            string alias = (mainTables.Count() > 1) ? mainTables[1] : mainTables[0];

            string temp_build = $"{joinType} {tableName} ON {alias}.{field} = {joinTable}.{joinField}";

            //adds it to the join statement lists
            theQuery.joinFields.Add(temp_build);
        }

        protected void JoinCompile(Query theQuery, string tableName, string joinType, string joinTable, string[] fields, string[] joinFields)
        {
            //Does validation
            JoinMultipleValidation(joinType, tableName, joinTable, fields, joinFields);

            //This is done to ensure if we have an alias we use it otherwise we use the generic
            string[] mainTables = tableName.Split(' ');
            string alias = (mainTables.Count() > 1) ? mainTables[1] : mainTables[0];

            string temp_build = "";
            for (int i = 0; i < fields.Length; i++)
            {   
                //Determines when to start using the and on the join parameters
                if (i == 0)//First params so init the join
                {                    
                    temp_build += $"{joinType} {tableName} ON {alias}.{fields[i]} = {joinTable}.{joinFields[i]}";                    
                }
                else//Add the additional params
                {
                    temp_build += $" AND {alias}.{fields[i]} = {joinTable}.{joinFields[i]}";
                }                
            }

            //adds it to the join statement lists
            theQuery.joinFields.Add(temp_build);
        }

        protected void JoinAdditionalCompile(Query theQuery, string tableName, string joinTable, string field, string joinField)
        {
            //Does validation
            JoinSingleValidation("exclude", tableName, joinTable, field, joinField);

            //This is done to ensure if we have an alias we use it otherwise we use the generic
            string[] mainTables = tableName.Split(' ');
            string alias = (mainTables.Count() > 1) ? mainTables[1] : mainTables[0];

            string temp_build = $" AND {alias}.{field} = {joinTable}.{joinField}";

            //Adds it to the current join object
            theQuery.joinFields[theQuery.joinFields.Count - 1] = theQuery.joinFields[theQuery.joinFields.Count - 1] + temp_build;
        }

        protected void JoinAdditionalCompile(Query theQuery, string tableName, string joinTable, string[] fields, string[] joinFields)
        {
            //Does validation
            JoinMultipleValidation("exclude", tableName, joinTable, fields, joinFields);

            //This is done to ensure if we have an alias we use it otherwise we use the generic
            string[] mainTables = tableName.Split(' ');
            string alias = (mainTables.Count() > 1) ? mainTables[1] : mainTables[0];

            string temp_build = "";
            for (int i = 0; i < fields.Length; i++)
            {
                temp_build += $" AND {alias}.{fields[i]} = {joinTable}.{joinFields[i]}";
            }

            //Adds it to the current join object
            theQuery.joinFields[theQuery.joinFields.Count - 1] = theQuery.joinFields[theQuery.joinFields.Count - 1] + temp_build;
        }

        /*##########################################*/
        /*        Join Validation functions         */
        /*##########################################*/

        protected void JoinSingleValidation(string joinType, string joinTableName, string currentTableName, string joinTableFields, string currentTableFields)
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

        protected void JoinMultipleValidation(string joinType, string joinTableName, string currentTableName, string[] joinTableFields, string[] currentTableFields)
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

        protected void JoinExistValidation(Query theQuery)
        {
            if (theQuery.joinFields == null || theQuery.joinFields.Count <= 0)
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
            Query theQuery = GetQuery();

            //Builds the sql from the parameters
            JoinCompile(theQuery, joinTableName, joinType, currentTableName, joinTableField, currentTableField);

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
            Query theQuery = GetQuery();

            //Builds the sql from the parameters
            JoinCompile(theQuery, joinTableName, joinType, currentTableName, joinTableFields, currentTableFields);

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
            Query theQuery = GetQuery();

            JoinExistValidation(theQuery);

            //Builds the additional sql from the parameters
            JoinAdditionalCompile(theQuery, joinTableName, currentTableName, joinTableFields, currentTableFields);
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
            Query theQuery = GetQuery();

            JoinExistValidation(theQuery);

            //Builds the additional sql from the parameters
            JoinAdditionalCompile(theQuery, joinTableName, currentTableName, joinTableField, currentTableField);
        }
    }
}