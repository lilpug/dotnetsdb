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
            try
            {
                int counter = 0;

                foreach (object data in items)
                {
                    command.Parameters.AddWithValue(definition + counter.ToString(), ((data == null) ? DBNull.Value : data));
                    if (data != null)
                    {
                        SqlServer2012Convertor convertor = new SqlServer2012Convertor();
                        command.Parameters[definition + counter.ToString()].SqlDbType = convertor.ToSqlDbType(data.GetType());
                    }
                    counter++;
                }
            }
            catch(Exception e)
            {
                throw new Exception(string.Format("There seems to be an issue while sanitising definition: {0}{1}Error: {2}", definition, Environment.NewLine, e.Message));
            }
        }
    }
}