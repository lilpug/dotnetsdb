using System;
using System.Data.SqlClient; //used for sqlcommand etc

namespace DotNetSDB
{
    public partial class SqlServerCore
    {
        /*##########################################*/
        /*     Sanitisation Compiling functions     */
        /*##########################################*/

        protected virtual void SanitiseItems(string definition, ref SqlCommand command, params object[] items)
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
                        using (SqlServerTypeConvertor convertor = new SqlServerTypeConvertor())
                        {
                            command.Parameters[newDefinition].SqlDbType = convertor.ToSqlDbType(data.GetType());
                        }
                    }                    
                }
            }
            catch(Exception e)
            {
                throw new Exception($"There seems to be an issue while sanitising definition: {definition + counter.ToString()}{Environment.NewLine}Basic Error Information: {e.Message}{Environment.NewLine}Error Details: {e.ToString()}");
            }
        }

        //This function processes all the different sanitisations that have been declared
        protected void SanitisationProcess(ref SqlCommand command)
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

                    if (current.whereRealValues.Count != 0)
                    {
                        for (int i = 0; i < current.whereRealValues.Count; i++)
                        {
                            SanitiseItems($"{whereDefinition}_{realQueryCount}_{i}_", ref command, current.whereRealValues[i]);
                        }
                    }

                    if (current.updateRealValues.Count != 0)
                    {
                        for (int i = 0; i < current.updateRealValues.Count; i++)
                        {
                            SanitiseItems($"{updateDefinition}_{realQueryCount}_{i}_", ref command, current.updateRealValues[i]);
                        }
                    }

                    if (current.insertRealValues.Count != 0)
                    {
                        for (int i = 0; i < current.insertRealValues.Count; i++)
                        {
                            SanitiseItems($"{insertDefinition}_{realQueryCount}_{i}_", ref command, current.insertRealValues[i]);

                        }
                    }

                    if (current.customRealValues.Count != 0)
                    {
                        for (int i = 0; i < current.customRealValues.Count; i++)
                        {
                            SanitiseItems($"{customDefinition}_{realQueryCount}_{i}_", ref command, current.customRealValues[i]);
                        }
                    }

                    //This section does the extra functions via Mysql Core

                    //Gets the index of the current query we are on
                    int index = theQueries.IndexOf(current);
                    if (theQueries2[index].existRealTableValue != null && theQueries2[index].existRealTableValue.Length > 0)
                    {
                        SanitiseItems($"{existDefinition}_{realQueryCount}_0_", ref command, theQueries2[index].existRealTableValue);
                    }

                    if (theQueries2[index].getFieldsRealTableValue != null && theQueries2[index].getFieldsRealTableValue.Length > 0)
                    {
                        SanitiseItems($"{fieldsDefinition}_{realQueryCount}_0_", ref command, theQueries2[index].getFieldsRealTableValue);
                    }

                    //Fires the extra hook run
                    ExtraSanitisationProcessing(current, realQueryCount);
                }

            }
        }

        //This can be used as a hook in function for new features which are inherited down the line and need to be sanatised
        protected virtual void ExtraSanitisationProcessing(Query current, int queryCounter)
        {
        }
    }
}