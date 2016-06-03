﻿using MySql.Data.MySqlClient;
using System;

namespace DotNetSDB
{
    public partial class MysqlCore
    {
        /*##########################################*/
        /*           Main Front function            */
        /*##########################################*/

        /// <summary>
        /// This function returns the sql query which will be built on run
        /// </summary>
        /// <returns></returns>
        public string sql_real_output()
        {
            try
            {
                //Compiles the sql in debug mode
                compiling(true);

                //gets the query ready and wraps the query in the deadlock solution
                MySqlCommand myCommand = new MySqlCommand(compiled_build, myConnection);

                //Checks for sanitisation
                sanitisation_create(ref myCommand);

                //Gets the query
                string query = myCommand.CommandText;

                //Loops over the parameters and outputs their real values
                foreach (MySqlParameter p in myCommand.Parameters)
                {
                    //query = query.Replace(p.ParameterName, ParameterValueForSQL(p));
                    query = ReplaceFirst(query, p.ParameterName, ParameterValueForSQL(p));
                }

                return query;
            }
            catch { }
            return null;
        }

        private string ReplaceFirst(string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }

        private string already_compiled_sql_output(ref MySqlCommand myCommand)
        {
            try
            {
                //Gets the query
                string query = myCommand.CommandText;

                //Loops over the parameters and outputs their real values
                foreach (MySqlParameter p in myCommand.Parameters)
                {
                    query = query.Replace(p.ParameterName, ParameterValueForSQL(p));
                }

                return query;
            }
            catch { }
            return null;
        }

        //This returns the parameters value in the specific format
        private String ParameterValueForSQL(MySqlParameter sp)
        {
            String retval = "";

            switch (sp.MySqlDbType)
            {
                case MySqlDbType.VarChar:
                case MySqlDbType.VarString:
                case MySqlDbType.Text:
                case MySqlDbType.MediumText:
                case MySqlDbType.LongText:
                case MySqlDbType.TinyText:
                case MySqlDbType.Time:
                case MySqlDbType.DateTime:
                case MySqlDbType.Date:
                    retval = "'" + sp.Value.ToString().Replace("'", "''") + "'";
                    break;

                case MySqlDbType.Bit:
                    retval = (ToBooleanOrDefault(sp.Value) != false) ? "1" : "0";
                    break;

                default://Ints etc all here
                    retval = sp.Value.ToString().Replace("'", "''");
                    break;
            }

            return retval;
        }

        //This deals with boolean and bit values
        private Boolean ToBooleanOrDefault(Object o)
        {
            Boolean ReturnVal = false;
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
                            ReturnVal = Boolean.Parse(o.ToString());
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