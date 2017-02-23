using MySql.Data.MySqlClient;//Used for mysqlsqlcommand etc
using System;

namespace DotNetSDB
{
    public partial class MySLQCore
    {
        /*##########################################*/
        /*     Sanitisation Compiling functions     */
        /*##########################################*/

        protected virtual void SanitiseItems(string definition, ref MySqlCommand command, params object[] items)
        {
            //Note: We do this here and in a foreach so if we get any errors we can build the error definition section in the exception.
            int counter = -1;

            try
            {
                foreach (object data in items)
                {
                    counter++;

                    var newDefinition = string.Format("{0}{1}", definition, counter.ToString());

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
                throw new Exception(string.Format("There seems to be an issue while sanitising definition: {0}{1}Basic Error Information: {2}{1}Error Details: {3}", definition + counter.ToString(), Environment.NewLine, e.Message, e.ToString()));
            }
        }

        //This function processes all the different sanitisations that have been declared
        protected void SanitisationProcess(ref MySqlCommand command)
        {
            //The definition is broken into the following numbers:-
            //Operator definition, query number, operator number, value number

            //This loops through all the querys checking the values sanitize correctly
            for(int qc=0; qc<theQueries.Count; qc++)
            {
                Query current = theQueries[qc];

                if (current.whereRealValues.Count != 0)
                {
                    for(int i=0; i<current.whereRealValues.Count; i++)
                    {
                        SanitiseItems(string.Format("{0}_{1}_{2}_", whereDefinition, qc.ToString(), i.ToString()), ref command, current.whereRealValues[i]);
                    }
                }

                if (current.updateRealValues.Count != 0)
                {
                    for(int i=0; i<current.updateRealValues.Count; i++)
                    {
                        SanitiseItems(string.Format("{0}_{1}_{2}_", updateDefinition, qc.ToString(), i.ToString()), ref command, current.updateRealValues[i]);                        
                    }
                }

                if (current.insertRealValues.Count != 0)
                {   
                    for(int i=0; i< current.insertRealValues.Count; i++)
                    {   
                        SanitiseItems(string.Format("{0}_{1}_{2}_", insertDefinition, qc.ToString(), i.ToString()), ref command, current.insertRealValues[i]);
                        
                    }
                }

                if (current.customRealValues.Count != 0)
                {
                    for(int i=0; i< current.customRealValues.Count; i++)
                    {
                        SanitiseItems(string.Format("{0}_{1}_{2}_", customDefinition, qc.ToString(), i.ToString()), ref command, current.customRealValues[i]);
                    }
                }

                //This section does the extra functions via Mysql Core

                //Gets the index of the current query we are on
                int index = theQueries.IndexOf(current);
                if (theQueries2[index].exist_real_table_value != null && theQueries2[index].exist_real_table_value.Length > 0)
                {
                    SanitiseItems(string.Format("{0}_{1}_0_", existDefinition, qc.ToString()), ref command, theQueries2[index].exist_real_table_value);
                }

                if (theQueries2[index].get_fields_real_table_value != null && theQueries2[index].get_fields_real_table_value.Length > 0)
                {
                    SanitiseItems(string.Format("{0}_{1}_0_", fieldsDefinition, qc.ToString()), ref command, theQueries2[index].get_fields_real_table_value);
                }

                //Fires the extra hook run
                ExtraSanitisationProcessing(current, qc);
            }
        }

        //This can be used as a hook in function for new features which are inherited down the line and need to be sanatised
        protected virtual void ExtraSanitisationProcessing(Query current, int queryCounter)
        {
        }
    }
}