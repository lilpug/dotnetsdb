using System;
using System.Data.SqlClient; //used for sqlcommand etc

namespace DotNetSDB
{
    public partial class SqlServer2008
    {
        /*##########################################*/
        /*     Sanatisation Compiling functions     */
        /*##########################################*/

        protected override void SanitiseItems(string definition, ref SqlCommand command, params object[] items)
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
                        using (SqlServer2008TypeConvertor convertor = new SqlServer2008TypeConvertor())
                        {
                            command.Parameters[newDefinition].SqlDbType = convertor.ToSqlDbType(data.GetType());
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