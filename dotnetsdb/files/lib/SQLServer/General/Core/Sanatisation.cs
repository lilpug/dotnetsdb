using System;
using System.Data.SqlClient; //used for sqlcommand etc

namespace DotNetSDB
{
    public partial class SqlServerCore
    {
        /*##########################################*/
        /*     Sanitisation Compiling functions     */
        /*##########################################*/

        protected virtual void Sanitisation(string definition, ref SqlCommand command, params object[] items)
        {
            int counter = 0;

            try
            {
                foreach (object data in items)
                {
                    command.Parameters.AddWithValue(definition + counter.ToString(), ((data == null) ? DBNull.Value : data));
                    if (data != null)
                    {
                        SqlServerTypeConvertor convertor = new SqlServerTypeConvertor();
                        command.Parameters[definition + counter.ToString()].SqlDbType = convertor.ToSqlDbType(data.GetType());                        
                    }
                    counter++;
                }
            }
            catch(Exception e)
            {
                throw new Exception(string.Format("There seems to be an issue while sanitising definition: {0}{1}Basic Error Information: {2}{1}Error Details: {3}", definition + counter.ToString(), Environment.NewLine, e.Message, e.ToString()));
            }
        }

        //This function processes all the different sanitisations that have been declared
        protected void sanitisation_create(ref SqlCommand command)
        {
            //The definition is broken into the following numbers:-
            //Operator definition, query number, operator number, value number

            int queryCounter = 1;

            //This loops through all the querys checking the values sanitize correctly
            foreach (query current in theQueries)
            {
                //The counter is used for linking the parameters with the correct definitions
                int operatorCounter = 0;

                if (current.where_real_values.Count != 0)
                {
                    operatorCounter = 0;

                    foreach (object[] values in current.where_real_values)
                    {
                        Sanitisation(where_definition + "_" + queryCounter.ToString() + "_" + operatorCounter.ToString() + "_", ref command, values);
                        operatorCounter++;
                    }
                }

                if (current.update_real_values.Count != 0)
                {
                    operatorCounter = 0;

                    foreach (object[] values in current.update_real_values)
                    {
                        Sanitisation(update_definition + "_" + queryCounter.ToString() + "_" + operatorCounter.ToString() + "_", ref command, values);
                        operatorCounter++;
                    }
                }

                if (current.insert_real_values.Count != 0)
                {
                    operatorCounter = 0;

                    foreach (object[] values in current.insert_real_values)
                    {
                        Sanitisation(insert_definition + "_" + queryCounter.ToString() + "_" + operatorCounter.ToString() + "_", ref command, values);
                        operatorCounter++;
                    }
                }

                if (current.custom_real_values.Count != 0)
                {
                    operatorCounter = 0;

                    foreach (object[] values in current.custom_real_values)
                    {
                        Sanitisation(custom_definition + "_" + queryCounter.ToString() + "_" + operatorCounter.ToString() + "_", ref command, values);
                        operatorCounter++;
                    }
                }

                //This section does the extra functions via Mysql Core

                //Gets the index of the current query we are on
                int index = theQueries.IndexOf(current);
                if (theQueries2[index].exist_real_table_value != "")
                {
                    Sanitisation(exist_definition + "_" + queryCounter.ToString() + "_" + "0" + "_", ref command, theQueries2[index].exist_real_table_value);
                }

                if (theQueries2[index].get_fields_real_table_value != "")
                {
                    Sanitisation(fields_definition + "_" + queryCounter.ToString() + "_" + "0" + "_", ref command, theQueries2[index].get_fields_real_table_value);
                }

                //Fires the extra hook run
                ExtraSanitisation(current, queryCounter);

                queryCounter++;
            }
        }

        //This can be used as a hook in function for new features which are inherited down the line and need to be sanatised
        protected virtual void ExtraSanitisation(query current, int queryCounter)
        {
        }
    }
}