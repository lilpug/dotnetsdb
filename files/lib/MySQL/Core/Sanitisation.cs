using MySql.Data.MySqlClient;//Used for mysqlsqlcommand etc
using System;

namespace DotNetSDB
{
    public partial class MySQLCore
    {
        /*##########################################*/
        /*     Sanitisation Compiling functions     */
        /*##########################################*/

        /// <summary>
        /// This function sanitise and parameter binds the query values using the MySQL Core type lookup class
        /// </summary>
        /// <param name="definition"></param>
        /// <param name="command"></param>
        /// <param name="items"></param>
        protected virtual void SanitiseItems(string definition, ref MySqlCommand command, params object[] items)
        {
            //Note: We do this here and in a foreach so if we get any errors we can build the error definition section in the exception.
            int counter = -1;

            try
            {
                foreach (object data in items)
                {
                    counter++;

                    //Use the definition literally if its a stored procedure
                    string newDefinition = null;
                    if (Procedure != null)
                    {
                        newDefinition = definition;
                    }
                    else
                    {
                        newDefinition = $"{definition}{counter}";
                    }

                    command.Parameters.AddWithValue(newDefinition, ((data == null) ? DBNull.Value : data));
                    if (data != null)
                    {
                        using (MySqlTypeConvertor convertor = new MySqlTypeConvertor())
                        {
                            command.Parameters[newDefinition].MySqlDbType = convertor.ToSqlDbType(data.GetType());
                        }
                    }                    
                }
            }
            catch(Exception e)
            {
                throw new Exception($"There seems to be an issue while sanitising definition: {definition + counter.ToString()}{Environment.NewLine}Basic Error Information: {e.Message}{Environment.NewLine}Error Details: {e.ToString()}");
            }
        }

        /// <summary>
        /// This function processes all the different sanitisations that have been declared
        /// </summary>
        /// <param name="command"></param>
        protected void SanitisationProcess(ref MySqlCommand command)
        {
            //This checks if we are sanitising a stored procedure or a query
            if (Procedure != null)
            {
                //Runs the stored procedure process

                //Checks if any parameters have been supplied with the command to call the stored procedure
                if (Procedure.Parameters != null && Procedure.Parameters.Count > 0)
                {
                    //Loops over all the keys in the stored procedure
                    foreach (string name in Procedure.Parameters.Keys)
                    {
                        SanitiseItems(name, ref command, Procedure.Parameters[name]);
                    }
                }
            }
            else
            {
                //Runs the query process

                //The definition is broken into the following numbers:-
                //Operator definition, query number, operator number, value number

                //This loops through all the querys checking the values sanitize correctly
                for (int qc = 0; qc < theQueries.Count; qc++)
                {
                    Query current = theQueries[qc];
                    int realQueryCount = qc + 1;

                    if (current.WhereRealValues.Count != 0)
                    {
                        for (int i = 0; i < current.WhereRealValues.Count; i++)
                        {
                            SanitiseItems($"{whereDefinition}_{realQueryCount}_{i}_", ref command, current.WhereRealValues[i]);
                        }
                    }

                    if (current.UpdateRealValues.Count != 0)
                    {
                        for (int i = 0; i < current.UpdateRealValues.Count; i++)
                        {
                            SanitiseItems($"{updateDefinition}_{realQueryCount}_{i}_", ref command, current.UpdateRealValues[i]);
                        }
                    }

                    if (current.InsertRealValues.Count != 0)
                    {
                        for (int i = 0; i < current.InsertRealValues.Count; i++)
                        {
                            SanitiseItems($"{insertDefinition}_{realQueryCount}_{i}_", ref command, current.InsertRealValues[i]);

                        }
                    }

                    if (current.CustomRealValues.Count != 0)
                    {
                        for (int i = 0; i < current.CustomRealValues.Count; i++)
                        {
                            SanitiseItems($"{customDefinition}_{realQueryCount}_{i}_", ref command, current.CustomRealValues[i]);
                        }
                    }

                    //Fires the extra hook run
                    ExtraSanitisationProcessing(current, ref command, realQueryCount);
                }
            }
        }

        /// <summary>
        /// This can be used as a hook in function for new features which are inherited down the line and need to be sanatised
        /// </summary>
        /// <param name="current"></param>
        /// <param name="command"></param>
        /// <param name="queryCounter"></param>
        protected virtual void ExtraSanitisationProcessing(Query current, ref MySqlCommand command, int queryCounter)
        {
            QueryExtension theQuery = (QueryExtension)current;

            if (theQuery.ExistRealTableValue != null && theQuery.ExistRealTableValue.Length > 0)
            {
                SanitiseItems($"{existDefinition}_{queryCounter}_0_", ref command, theQuery.ExistRealTableValue);
            }

            if (theQuery.GetFieldsRealTableValue != null && theQuery.GetFieldsRealTableValue.Length > 0)
            {
                SanitiseItems($"{fieldsDefinition}_{queryCounter}_0_", ref command, theQuery.GetFieldsRealTableValue);
            }
        }
    }
}