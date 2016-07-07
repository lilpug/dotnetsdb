using System;
using System.Data.SqlClient; //used for sqlcommand etc

namespace DotNetSDB
{
    public partial class SqlServer2012
    {
        /*##########################################*/
        /*     Sanitisation Compiling functions     */
        /*##########################################*/

        protected override void Sanitisation(string definition, ref SqlCommand command, params object[] items)
        {
            int counter = -1;

            try
            {   
                foreach (object data in items)
                {
                    counter++;

                    command.Parameters.AddWithValue(definition + counter.ToString(), ((data == null) ? DBNull.Value : data));
                    if (data != null)
                    {
                        using (SqlServer2012TypeConvertor convertor = new SqlServer2012TypeConvertor())
                        {
                            command.Parameters[definition + counter.ToString()].SqlDbType = convertor.ToSqlDbType(data.GetType());
                        }
                    }                        
                }                
            }
            catch(Exception e)
            {   
                throw new Exception(string.Format("There seems to be an issue while sanitising definition: {0}{1}Basic Error Information: {2}{1}Error Details: {3}", definition + counter.ToString(), Environment.NewLine, e.Message, e.ToString()));
            }
        }
    }
}