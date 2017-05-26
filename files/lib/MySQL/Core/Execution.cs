using DotNetSDB.output;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;//Used for DataTable
using System.Threading;

namespace DotNetSDB
{
    public partial class MySQLCore
    {
        /*##########################################*/
        /*        Executing Query functions         */
        /*##########################################*/

        //These functions are the core sql execution functions
        //Note: the reason the using statement is not within the core processing as when returning data from the reader or adapter the connection needs to be alive still

        //Normal

        protected virtual void CoreProcessing(MySqlConnection myConnection, int counter = 0)
        {
            //Checks if its already been compiled i.e. debug sql output
            if (string.IsNullOrWhiteSpace(compiledSql))
            {
                //Compiles the querys into one massive query string
                compiling();
            }

            //gets the query ready and wraps the query in the deadlock solution
            MySqlCommand myCommand = new MySqlCommand(compiledSql, myConnection);

            myCommand.CommandTimeout = connectionTime;

            //Checks for sanitisation
            SanitisationProcess(ref myCommand);

            try
            {
                //Executes the query *if it does not throw an exception then it was done so successfully*
                var value = myCommand.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                //Deadlock issue: re-attempt based on the number of tries we can do
                if (e.Number == 1205 && counter < maxDeadlockTry)
                {
                    Thread.Sleep(1000);
                    CoreProcessing(myConnection, counter + 1);
                }
                else
                {
                    //Checks if a logger has been attached
                    if (loggerDetails != null)
                    {
                        //Logs the data
                        using (OutputManagement debugger = new OutputManagement(loggerDetails))
                        {
                            if (e.Number == 1205)
                            {
                                debugger.AddToLog($"{e.Message}\r\nDEADLOCK ERROR\r\nQuery: '{GetCompiledSqlFromCommand(ref myCommand)}'");
                            }
                            else
                            {
                                debugger.AddToLog($"{e.Message}\r\nQuery: '{GetCompiledSqlFromCommand(ref myCommand)}'");
                            }
                        }
                    }
                    throw e;
                }
            }
        }

        protected virtual void CoreProcedureProcessing(string procedureName, MySqlConnection myConnection, int counter = 0, Dictionary<string, object> theParameters = null)
        {
            //Gets the output reader ready
            MySqlCommand myCommand = new MySqlCommand();

            //gets the insert query ready
            myCommand = new MySqlCommand(procedureName, myConnection);

            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandTimeout = connectionTime;

            //Checks its not null
            if (theParameters != null && theParameters.Keys.Count > 0)
            {
                //Adds all the parameters
                foreach (string parameter in theParameters.Keys)
                {
                    myCommand.Parameters.Add(new MySqlParameter(parameter, theParameters[parameter]));
                }
            }

            try
            {
                //Executes the query *if it does not throw an exception then it was done so successfully*
                myCommand.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                //Deadlock issue: re-attempt based on the number of tries we can do
                if (e.Number == 1205 && counter < maxDeadlockTry)
                {
                    Thread.Sleep(1000);
                    CoreProcedureProcessing(procedureName, myConnection, counter + 1);
                }
                else
                {
                    //Checks if a logger has been attached
                    if (loggerDetails != null)
                    {
                        //Logs the data
                        using (OutputManagement debugger = new OutputManagement(loggerDetails))
                        {
                            if (e.Number == 1205)
                            {
                                debugger.AddToLog($"{e.Message}\r\nDEADLOCK ERROR\r\nStored Procedure: '{procedureName}'");
                            }
                            else
                            {
                                debugger.AddToLog($"{e.Message}\r\nStored Procedure: '{procedureName}'");
                            }
                        }
                    }
                    throw e;
                }
            }
        }

        //SqlDataReader
        //Note: This is more quicker and effective on memory as it does not store all the results in memory only a single row before iteration

        protected virtual MySqlDataReader CoreProcessingReaderReturn(MySqlConnection myConnection, int counter = 0)
        {
            //Checks if its already been compiled i.e. debug sql output
            if (string.IsNullOrWhiteSpace(compiledSql))
            {
                //Compiles the querys into one massive query string
                compiling();
            }

            MySqlDataReader myReader = null;

            //gets the query ready and wraps the query in the deadlock solution
            MySqlCommand myCommand = new MySqlCommand(compiledSql, myConnection);

            myCommand.CommandTimeout = connectionTime;

            //Checks for sanitisation
            SanitisationProcess(ref myCommand);

            try
            {
                //Executes the command and puts it into the reader
                myReader = myCommand.ExecuteReader();
                
                return myReader;
            }
            catch (MySqlException e)
            {
                //Deadlock issue: re-attempt based on the number of tries we can do
                if (e.Number == 1205 && counter < maxDeadlockTry)
                {
                    Thread.Sleep(1000);
                    //If after some delay the recursions succeeds then return its datareader chain to the original instance
                    return CoreProcessingReaderReturn(myConnection, counter + 1);
                }
                else
                {
                    //Checks if a logger has been attached
                    if (loggerDetails != null)
                    {
                        //Logs the data
                        using (OutputManagement debugger = new OutputManagement(loggerDetails))
                        {
                            if (e.Number == 1205)
                            {
                                debugger.AddToLog($"{e.Message}\r\nDEADLOCK ERROR\r\nQuery: '{GetCompiledSqlFromCommand(ref myCommand)}'");
                            }
                            else
                            {
                                debugger.AddToLog($"{e.Message}\r\nQuery: '{GetCompiledSqlFromCommand(ref myCommand)}'");
                            }
                        }
                    }
                    throw e;
                }
            }
        }

        protected virtual MySqlDataReader CoreProcedureProcessingReaderReturn(string procedureName, MySqlConnection myConnection, int counter = 0, Dictionary<string, object> theParameters = null)
        {
            //Gets the output reader ready
            MySqlDataReader myReader = null;
            MySqlCommand myCommand = new MySqlCommand();

            //gets the insert query ready
            myCommand = new MySqlCommand(procedureName, myConnection);

            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandTimeout = connectionTime;

            //Checks its not null
            if (theParameters != null && theParameters.Keys.Count > 0)
            {
                //Adds all the parameters
                foreach (string parameter in theParameters.Keys)
                {
                    myCommand.Parameters.Add(new MySqlParameter(parameter, theParameters[parameter]));
                }
            }

            try
            {
                //Executes the command and puts it into the reader
                myReader = myCommand.ExecuteReader();                

                return myReader;
            }
            catch (MySqlException e)
            {
                //Deadlock issue: re-attempt based on the number of tries we can do
                if (e.Number == 1205 && counter < maxDeadlockTry)
                {
                    Thread.Sleep(1000);
                    return CoreProcedureProcessingReaderReturn(procedureName, myConnection, counter + 1);
                }
                else
                {
                    //Checks if a logger has been attached
                    if (loggerDetails != null)
                    {
                        //Logs the data
                        using (OutputManagement debugger = new OutputManagement(loggerDetails))
                        {
                            if (e.Number == 1205)
                            {
                                debugger.AddToLog($"{e.Message}\r\nDEADLOCK ERROR\r\nStored Procedure: '{procedureName}'");
                            }
                            else
                            {
                                debugger.AddToLog($"{e.Message}\r\nStored Procedure: '{procedureName}'");
                            }
                        }
                    }
                    throw e;
                }
            }
        }

        //SqlDataAdapter
        //Note: This stores all the result set directly in memory

        protected virtual MySqlDataAdapter CoreProcessingAdapterReturn(MySqlConnection myConnection, int counter = 0)
        {
            //Checks if its already been compiled i.e. debug sql output
            if (string.IsNullOrWhiteSpace(compiledSql))
            {
                //Compiles the querys into one massive query string
                compiling();
            }

            //gets the query ready and wraps the query in the deadlock solution
            MySqlCommand myCommand = new MySqlCommand(compiledSql, myConnection);

            myCommand.CommandTimeout = connectionTime;

            //Checks for sanitisation
            SanitisationProcess(ref myCommand);

            try
            {
                //Executes the command and puts it into the reader
                MySqlDataAdapter myAdapter = new MySqlDataAdapter(myCommand);                

                return myAdapter;
            }
            catch (MySqlException e)
            {
                //Deadlock issue: re-attempt based on the number of tries we can do
                if (e.Number == 1205 && counter < maxDeadlockTry)
                {
                    Thread.Sleep(1000);
                    //If after some delay the recursions succeeds then return its datareader chain to the original instance
                    return CoreProcessingAdapterReturn(myConnection, counter + 1);
                }
                else
                {
                    //Checks if a logger has been attached
                    if (loggerDetails != null)
                    {
                        //Logs the data
                        using (OutputManagement debugger = new OutputManagement(loggerDetails))
                        {
                            if (e.Number == 1205)
                            {
                                debugger.AddToLog($"{e.Message}\r\nDEADLOCK ERROR\r\nQuery: '{GetCompiledSqlFromCommand(ref myCommand)}'");
                            }
                            else
                            {
                                debugger.AddToLog($"{e.Message}\r\nQuery: '{GetCompiledSqlFromCommand(ref myCommand)}'");
                            }
                        }
                    }
                    throw e;
                }
            }
        }

        /*##########################################*/
        /*        Normal Sql Query functions        */
        /*##########################################*/

        /// <summary>
        /// <para>This executes the sql which has been added.</para>
        /// <para>Note: This does not return any data, it only executes the sql.</para>
        /// </summary>
        public virtual void run()
        {
            using (myConnection = new MySqlConnection(connection))
            {
                //Opens the connection
                myConnection.Open();

                try
                {
                    //Runs the core processing
                    CoreProcessing(myConnection);
                }
                catch (Exception e)
                {
                    //Checks if a logger has been attached
                    if (loggerDetails != null)
                    {
                        //Logs the data
                        using (OutputManagement debugger = new OutputManagement(loggerDetails))
                        {
                            debugger.AddToLog($"\r\nrun: '{e.Message}'");
                        }
                    }
                    throw e;
                }
                finally
                {
                    //Clears the queries ready for the next
                    disposeAll();
                    compiledSql = "";
                }
            }
        }

        //Single value return functions

        /// <summary>
        /// <para>This executes the sql which has been added and returns the first value as string.</para>
        /// </summary>
        public virtual string run_return_string()
        {
            using (myConnection = new MySqlConnection(connection))
            {
                //Opens the connection
                myConnection.Open();

                //Runs the core processing and returns the data reader from it
                MySqlDataReader myReader = CoreProcessingReaderReturn(myConnection);

                try
                {
                    //gets the first value as a string output
                    string value = ResultToString(ref myReader);

                    //Closes the connections that are open
                    myReader.Close();

                    return value;
                }
                catch (Exception e)
                {
                    //Checks if a logger has been attached
                    if (loggerDetails != null)
                    {
                        //Logs the data
                        using (OutputManagement debugger = new OutputManagement(loggerDetails))
                        {
                            debugger.AddToLog($"\r\nrun_return_string: '{e.Message}'");
                        }
                    }
                    throw e;
                }
                finally
                {
                    //Clears the queries ready for the next
                    disposeAll();
                    compiledSql = "";
                }
            }
        }

        /// <summary>
        /// <para>This executes the sql which has been added and returns a string array from the first value of the results.</para>
        /// </summary>
        public virtual string[] run_return_string_array()
        {
            using (myConnection = new MySqlConnection(connection))
            {
                //Opens the connection
                myConnection.Open();

                //Runs the core processing and returns the data reader from it
                MySqlDataReader myReader = CoreProcessingReaderReturn(myConnection);

                try
                {
                    string[] temp = ResultToStringArray(ref myReader);

                    //Closes the connections that are open
                    myReader.Close();

                    return temp;
                }
                catch (Exception e)
                {
                    //Checks if a logger has been attached
                    if (loggerDetails != null)
                    {
                        //Logs the data
                        using (OutputManagement debugger = new OutputManagement(loggerDetails))
                        {
                            debugger.AddToLog($"\r\nrun_return_array: '{e.Message}'");
                        }
                    }
                    throw e;
                }
                finally
                {
                    //Clears the queries ready for the next
                    disposeAll();
                    compiledSql = "";
                }
            }
        }

        //End of the single value return functions

        /// <summary>
        /// <para>This executes the sql which has been added and returns the a json formatted string with the results.</para>
        /// </summary>
        public virtual string run_return_json()
        {
            using (myConnection = new MySqlConnection(connection))
            {
                //Opens the connection
                myConnection.Open();

                //Runs the core processing and returns the data reader from it
                MySqlDataReader myReader = CoreProcessingReaderReturn(myConnection);

                try
                {
                    string theResults = ResultToJson(ref myReader);

                    //Closes the connections that are open
                    myReader.Close();

                    return theResults;
                }
                catch (Exception e)
                {
                    //Checks if a logger has been attached
                    if (loggerDetails != null)
                    {
                        //Logs the data
                        using (OutputManagement debugger = new OutputManagement(loggerDetails))
                        {
                            debugger.AddToLog($"\r\nrun_return_json: '{e.Message}'");
                        }
                    }
                    throw e;
                }
                finally
                {
                    //Clears the queries ready for the next
                    disposeAll();
                    compiledSql = "";
                }
            }
        }

        /// <summary>
        /// <para>This executes the sql which has been added and returns the results in a datatable.</para>
        /// </summary>
        public virtual DataTable run_return_datatable()
        {
            using (myConnection = new MySqlConnection(connection))
            {
                //Opens the connection
                myConnection.Open();

                //Runs the core processing and returns the data reader from it
                MySqlDataReader myReader = CoreProcessingReaderReturn(myConnection);

                try
                {
                    //This section Processes the results into datatable format
                    DataTable theResults = ResultToDataTable(ref myReader);

                    //Closes the connections that are open
                    myReader.Close();

                    return theResults;
                }
                catch (Exception e)
                {
                    //Checks if a logger has been attached
                    if (loggerDetails != null)
                    {
                        //Logs the data
                        using (OutputManagement debugger = new OutputManagement(loggerDetails))
                        {
                            debugger.AddToLog($"\r\nrun_return_datatable: '{e.Message}'");
                        }
                    }
                    throw e;
                }
                finally
                {
                    //Clears the queries ready for the next
                    disposeAll();
                    compiledSql = "";
                }
            }
        }

        /// <summary>
        /// <para>This executes the sql which has been added and returns the results in a dataset.</para>
        /// </summary>
        public virtual DataSet run_return_dataset(bool enforceConstraints = true)
        {
            using (myConnection = new MySqlConnection(connection))
            {
                //Opens the connection
                myConnection.Open();

                //Runs the core processing and returns the data reader from it
                MySqlDataAdapter myAdapter = CoreProcessingAdapterReturn(myConnection);

                try
                {
                    //This section Processes the results into datatable format
                    DataSet theResults = ResultToDataSet(ref myAdapter, enforceConstraints);

                    //Closes the connections that are open
                    myAdapter.Dispose();

                    return theResults;
                }
                catch (Exception e)
                {
                    //Checks if a logger has been attached
                    if (loggerDetails != null)
                    {
                        //Logs the data
                        using (OutputManagement debugger = new OutputManagement(loggerDetails))
                        {
                            debugger.AddToLog($"\r\nrun_return_datatable: '{e.Message}'");
                        }
                    }
                    throw e;
                }
                finally
                {
                    //Clears the queries ready for the next
                    disposeAll();
                    compiledSql = "";
                }
            }
        }

        /// <summary>
        /// <para>This executes the sql which has been added and returns a list of dynamic objects that are structured with the results.</para>
        /// </summary>
        public virtual List<dynamic> run_return_dynamic()
        {
            using (myConnection = new MySqlConnection(connection))
            {
                //Opens the connection
                myConnection.Open();

                //Runs the core processing and returns the data reader from it
                MySqlDataReader myReader = CoreProcessingReaderReturn(myConnection);

                try
                {
                    //This section Processes the results into datatable format
                    List<dynamic> theResults = ResultToDynamic(ref myReader);

                    //Closes the connections that are open
                    myReader.Close();

                    return theResults;
                }
                catch (Exception e)
                {
                    //Checks if a logger has been attached
                    if (loggerDetails != null)
                    {
                        //Logs the data
                        using (OutputManagement debugger = new OutputManagement(loggerDetails))
                        {
                            debugger.AddToLog($"\r\nrun_return_dynamic: '{e.Message}'");
                        }
                    }
                    throw e;
                }
                finally
                {
                    //Clears the queries ready for the next
                    disposeAll();
                    compiledSql = "";
                }
            }
        }

        /// <summary>
        /// <para>This executes the sql which has been added and creates a csv file using the file path and delimiter parameters specified.</para>
        /// </summary>
        /// <param name="fullFilePath">the full file location including the filename</param>
        /// <param name="delimiter">the delimiter used for the csv</param>
        /// <returns></returns>
        public virtual bool run_to_csv(string fullFilePath, string delimiter = ",")
        {
            //Does the standard datatable query
            DataTable queryData = run_return_datatable();
            try
            {
                return DataTableToCSV(queryData, fullFilePath, delimiter);
            }
            catch (Exception e)
            {
                //Checks if a logger has been attached
                if (loggerDetails != null)
                {
                    //Logs the data
                    using (OutputManagement debugger = new OutputManagement(loggerDetails))
                    {
                        debugger.AddToLog($"\r\nrun_to_csv: '{e.Message}'");
                    }
                }
                throw e;
            }
        }

        /// <summary>
        /// <para>This executes the sql which has been added and appends the results to a csv file using the file path and delimiter parameters specified.</para>
        /// </summary>
        /// <param name="fullFilePath">the full file location including the filename</param>
        /// <param name="delimiter">the delimiter used for the csv</param>
        /// <returns></returns>
        public virtual bool run_append_to_csv(string fullFilePath, string delimiter = ",")
        {
            //Does the standard datatable query
            DataTable queryData = run_return_datatable();
            try
            {
                return DataTableAppendCSV(queryData, fullFilePath, delimiter);
            }
            catch (Exception e)
            {
                //Checks if a logger has been attached
                if (loggerDetails != null)
                {
                    //Logs the data
                    using (OutputManagement debugger = new OutputManagement(loggerDetails))
                    {
                        debugger.AddToLog($"\r\nrun_append_to_csv: '{e.Message}'");
                    }
                }
                throw e;
            }
        }

        /// <summary>
        /// <para>This executes the sql which has been added and returns the raw string format using the delimiter parameter specified.</para>
        /// </summary>        
        /// <param name="delimiter">the delimiter</param>
        /// <returns></returns>
        public virtual string run_raw_output(string delimiter = ",")
        {
            //Does the standard datatable query
            DataTable queryData = run_return_datatable();
            try
            {
                return DataTableToRawString(queryData, delimiter);
            }
            catch (Exception e)
            {
                //Checks if a logger has been attached
                if (loggerDetails != null)
                {
                    //Logs the data
                    using (OutputManagement debugger = new OutputManagement(loggerDetails))
                    {
                        debugger.AddToLog($"\r\nrun_raw_output: '{e.Message}'");
                    }
                }
                throw e;
            }
        }

        /*##########################################*/
        /*        Procedure Sql Query functions     */
        /*##########################################*/

        /// <summary>
        /// This executes the procedure passed and returns the results in a datatable.
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="theParameters"></param>
        /// <returns></returns>
        public virtual DataTable run_procedure_return_datatable(string procedureName, Dictionary<string, object> theParameters = null)
        {
            using (myConnection = new MySqlConnection(connection))
            {
                //Opens the connection
                myConnection.Open();

                //Runs the core processing for a procedure and returns the data reader from it
                MySqlDataReader myReader = CoreProcedureProcessingReaderReturn(procedureName, myConnection, 0, theParameters);

                try
                {
                    DataTable theResults = ResultToDataTable(ref myReader);

                    //Closes the connections that are open
                    myReader.Close();

                    //This section Processes the results into datatable format
                    return theResults;
                }
                catch (Exception e)
                {
                    //Checks if a logger has been attached
                    if (loggerDetails != null)
                    {
                        //Logs the data
                        using (OutputManagement debugger = new OutputManagement(loggerDetails))
                        {
                            debugger.AddToLog($"\r\nrun_procedure_return_datatable: '{e.Message}'");
                        }
                    }
                    throw e;
                }
                finally
                {
                    //Clears the queries ready for the next
                    disposeAll();
                    compiledSql = "";
                }
            }
        }

        /// <summary>
        /// <para>This executes the procedure passed.</para>
        /// <para>Note: This does not return any data, it only executes the procedure.</para>
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="theParameters"></param>
        public virtual void run_procedure(string procedureName, Dictionary<string, object> theParameters = null)
        {
            using (myConnection = new MySqlConnection(connection))
            {
                //Opens the connection
                myConnection.Open();

                try
                {
                    //Runs the core processing for a procedure
                    CoreProcedureProcessing(procedureName, myConnection, 0, theParameters);
                }
                catch (Exception e)
                {
                    //Checks if a logger has been attached
                    if (loggerDetails != null)
                    {
                        //Logs the data
                        using (OutputManagement debugger = new OutputManagement(loggerDetails))
                        {
                            debugger.AddToLog($"\r\nrun_procedure: '{e.Message}'");
                        }
                    }
                    throw e;
                }
                finally
                {
                    //Clears the queries ready for the next
                    disposeAll();
                    compiledSql = "";
                }
            }
        }
    }
}