using System;
using System.Data.SqlClient; //used for sqlcommand etc

namespace DotNetSDB
{
    public partial class SQLServer2008
    {
        /*##########################################*/
        /*     Sanitisation Compiling functions     */
        /*##########################################*/

        /// <summary>
        /// This function sanitise and parameter binds the query values using the 2008 type lookup class
        /// </summary>
        /// <param name="definition"></param>
        /// <param name="command"></param>
        /// <param name="items"></param>
        protected override void SanitiseItems(string definition, ref SqlCommand command, params object[] items)
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
                        using (DatabaseTypeConvertor convertor = new DatabaseTypeConvertor("sqlserver2008"))
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
    }
}