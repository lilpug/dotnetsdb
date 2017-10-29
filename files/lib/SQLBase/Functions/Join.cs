using System;
using System.Linq;
using System.Text;

namespace DotNetSDB
{
    public abstract partial class SQLBase
    {
        /*##########################################*/
        /*         Join Compiling functions         */
        /*##########################################*/

        /// <summary>
        /// This function deals with creating the join SQL
        /// </summary>
        /// <param name="theQuery"></param>
        /// <param name="tableName"></param>
        /// <param name="joinType"></param>
        /// <param name="joinTable"></param>
        /// <param name="field"></param>
        /// <param name="joinField"></param>
        protected void JoinCompile(Query theQuery, string tableName, string joinType, string joinTable, string field, string joinField)
        {
            //Does validation
            JoinSingleValidation(joinType, tableName, joinTable, field, joinField);

            //This is done to ensure if we have an alias we use it otherwise we use the generic
            string[] mainTables = tableName.Split(' ');
            string alias = (mainTables.Count() > 1) ? mainTables[1] : mainTables[0];

            string tempBuild = $"{joinType} {tableName} ON {alias}.{field} = {joinTable}.{joinField}";

            //adds it to the join statement lists
            theQuery.JoinFields.Add(new StringBuilder(tempBuild));
        }

        /// <summary>
        /// This function deals with creating the join SQL
        /// </summary>
        /// <param name="theQuery"></param>
        /// <param name="tableName"></param>
        /// <param name="joinType"></param>
        /// <param name="joinTable"></param>
        /// <param name="fields"></param>
        /// <param name="joinFields"></param>
        protected void JoinCompile(Query theQuery, string tableName, string joinType, string joinTable, string[] fields, string[] joinFields)
        {
            //Does validation
            JoinMultipleValidation(joinType, tableName, joinTable, fields, joinFields);

            //This is done to ensure if we have an alias we use it otherwise we use the generic
            string[] mainTables = tableName.Split(' ');
            string alias = (mainTables.Count() > 1) ? mainTables[1] : mainTables[0];

            StringBuilder sb = new StringBuilder();
            
            for (int i = 0; i < fields.Length; i++)
            {   
                //Determines when to start using the and on the join parameters
                if (i == 0)//First params so init the join
                {                    
                    sb.Append($"{joinType} {tableName} ON {alias}.{fields[i]} = {joinTable}.{joinFields[i]}");                    
                }
                else//Add the additional params
                {
                    sb.Append($" AND {alias}.{fields[i]} = {joinTable}.{joinFields[i]}");
                }                
            }

            //adds it to the join statement lists
            theQuery.JoinFields.Add(new StringBuilder(sb.ToString()));
        }

        /// <summary>
        /// This function deals with creating additional join SQL
        /// </summary>
        /// <param name="theQuery"></param>
        /// <param name="tableName"></param>
        /// <param name="joinTable"></param>
        /// <param name="field"></param>
        /// <param name="joinField"></param>
        protected void JoinAdditionalCompile(Query theQuery, string tableName, string joinTable, string field, string joinField)
        {
            //Does validation
            JoinSingleValidation("exclude", tableName, joinTable, field, joinField);

            //This is done to ensure if we have an alias we use it otherwise we use the generic
            string[] mainTables = tableName.Split(' ');
            string alias = (mainTables.Count() > 1) ? mainTables[1] : mainTables[0];

            string tempBuild = $" AND {alias}.{field} = {joinTable}.{joinField}";

            //Adds it to the current join object
            theQuery.JoinFields[theQuery.JoinFields.Count - 1].Append(tempBuild);
        }

        /// <summary>
        /// This function deals with creating additional join SQL
        /// </summary>
        /// <param name="theQuery"></param>
        /// <param name="tableName"></param>
        /// <param name="joinTable"></param>
        /// <param name="fields"></param>
        /// <param name="joinFields"></param>
        protected void JoinAdditionalCompile(Query theQuery, string tableName, string joinTable, string[] fields, string[] joinFields)
        {
            //Does validation
            JoinMultipleValidation("exclude", tableName, joinTable, fields, joinFields);

            //This is done to ensure if we have an alias we use it otherwise we use the generic
            string[] mainTables = tableName.Split(' ');
            string alias = (mainTables.Count() > 1) ? mainTables[1] : mainTables[0];

            StringBuilder sb = new StringBuilder();
            
            for (int i = 0; i < fields.Length; i++)
            {
                sb.Append($" AND {alias}.{fields[i]} = {joinTable}.{joinFields[i]}");
            }

            //Adds it to the current join object
            theQuery.JoinFields[theQuery.JoinFields.Count - 1].Append(sb.ToString());
        }

        /*##########################################*/
        /*        Join Validation functions         */
        /*##########################################*/

        /// <summary>
        /// This function validates the join variables
        /// </summary>
        /// <param name="joinType"></param>
        /// <param name="joinTableName"></param>
        /// <param name="currentTableName"></param>
        /// <param name="joinTableFields"></param>
        /// <param name="currentTableFields"></param>
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

        /// <summary>
        /// This function validates the join variables
        /// </summary>
        /// <param name="joinType"></param>
        /// <param name="joinTableName"></param>
        /// <param name="currentTableName"></param>
        /// <param name="joinTableFields"></param>
        /// <param name="currentTableFields"></param>
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

        /// <summary>
        /// This function validates that the join base statement has been added before trying to add additional joins
        /// </summary>
        /// <param name="theQuery"></param>
        protected void JoinExistValidation(Query theQuery)
        {
            if (theQuery.JoinFields == null || theQuery.JoinFields.Count <= 0)
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
            theQuery.OrderList.Add("join");
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
            theQuery.OrderList.Add("join");
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