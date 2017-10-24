using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace DotNetSDB
{
    public partial class SqlServerCore
    {
        /*##########################################*/
        /*           Main Front function            */
        /*##########################################*/

        /// <summary>
        /// This function returns the sql query which will be built on a run function
        /// </summary>
        /// <returns></returns>
        public string return_compiled_sql_string()
        {
            try
            {
                //Compiles the sql in debug mode
                compiling(true);

                //gets the query ready and wraps the query in the deadlock solution
                SqlCommand myCommand = new SqlCommand(compiledSql.ToString(), myConnection);

                //Checks for sanitisation
                SanitisationProcess(ref myCommand);

                //Gets the query
                string query = myCommand.CommandText;

                //Loops over the parameters and outputs their real values
                foreach (SqlParameter p in myCommand.Parameters)
                {   
                    query = ReplaceOccurrences(query, p.ParameterName, ParameterValueForSQL(p));
                }

                return query;
            }
            catch { }
            return null;
        }
        
        protected string ReplaceOccurrences(string text, string search, string replace)
        {
            return Regex.Replace(text, @"" + search + "+", replace);
        }

        protected string GetCompiledSqlFromCommand(SqlCommand myCommand)
        {
            try
            {
                //Gets the query
                string query = myCommand.CommandText;

                //Loops over the parameters and outputs their real values
                foreach (SqlParameter p in myCommand.Parameters)
                {
                    query = ReplaceOccurrences(query, p.ParameterName, ParameterValueForSQL(p));
                }

                return query;
            }
            catch { }
            return null;
        }

        //This returns the parameters value in the specific format
        protected string ParameterValueForSQL(SqlParameter sp)
        {
            string retval = "";

            switch (sp.SqlDbType)
            {
                case SqlDbType.Char:
                case SqlDbType.NChar:
                case SqlDbType.NText:
                case SqlDbType.NVarChar:
                case SqlDbType.Text:
                case SqlDbType.Time:
                case SqlDbType.VarChar:
                case SqlDbType.Xml:
                case SqlDbType.Date:
                case SqlDbType.DateTime:
                case SqlDbType.DateTime2:
                case SqlDbType.DateTimeOffset:
                    retval = "'" + sp.Value.ToString().Replace("'", "''") + "'";
                    break;

                case SqlDbType.Bit:
                    retval = (ToBooleanOrDefault(sp.Value) != false) ? "1" : "0";
                    break;

                default://Ints etc all here
                    retval = sp.Value.ToString().Replace("'", "''");
                    break;
            }

            return retval;
        }

        //This deals with boolean and bit values
        protected bool ToBooleanOrDefault(object o)
        {
            bool ReturnVal = false;
            try
            {
                if (o != null)
                {
                    switch (o.ToString().ToLower())
                    {
                        case "yes":
                        case "true":
                        case "ok":
                        case "y":
                        case "1":
                            ReturnVal = true;
                            break;

                        case "no":
                        case "false":
                        case "n":
                        case "0":
                            ReturnVal = false;
                            break;

                        default:
                            ReturnVal = bool.Parse(o.ToString());
                            break;
                    }
                }
            }
            catch
            {
            }
            return ReturnVal;
        }
    }
}