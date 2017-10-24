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

        /// <summary>
        /// This is the core function that sets up the SQL command object ready for execution
        /// </summary>
        /// <returns></returns>
        protected virtual MySqlCommand CoreCommandSetup()
        {
            MySqlCommand myCommand = null;

            //Checks if we are running a stored procedure or normal query
            if (Procedure != null)
            {
                //Checks if its already been compiled i.e. debug sql output
                if (compiledSql.Length == 0)
                {
                    //Compiles the querys into one massive query string
                    compiling();
                }

                //Throws an exception if any SQL query exists as we also have stored procedure flags
                if (compiledSql.Length > 0)
                {
                    throw new Exception("Database Execution Error: You cannot run both a stored procedure and an SQL query at the same time.");
                }

                //gets the insert query ready
                myCommand = new MySqlCommand(Procedure.Name, myConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };
            }
            //Normal query
            else
            {
                //Checks if its already been compiled i.e. debug sql output
                if (compiledSql.Length == 0)
                {
                    //Compiles the querys into one massive query string
                    compiling();
                }

                //gets the query ready and wraps the query in the deadlock solution
                myCommand = new MySqlCommand(compiledSql.ToString(), myConnection);
            }

            //Adds the connection timeout
            myCommand.CommandTimeout = connectionTime;

            //Checks for sanitisation
            SanitisationProcess(ref myCommand);

            return myCommand;
        }

        /// <summary>
        /// This is the core Deadlock retry functionality
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="myConnection"></param>
        /// <param name="myCommand"></param>
        /// <param name="counter"></param>
        /// <param name="exception"></param>
        /// <param name="runFunction"></param>
        /// <returns></returns>
        protected virtual T DeadLockRetry<T>(MySqlConnection myConnection, MySqlCommand myCommand, int counter, MySqlException exception, Func<MySqlConnection, int, T> runFunction)
        {
            //Deadlock issue: re-attempt based on the number of tries we can do
            if (exception.Number == 1205 && counter < maxDeadlockTry)
            {
                Thread.Sleep(1000);
                return runFunction(myConnection, counter + 1);
            }
            else
            {
                //Checks if a logger has been attached
                if (loggerDetails != null)
                {
                    //Logs the data
                    using (OutputManagement debugger = new OutputManagement(loggerDetails))
                    {
                        if (exception.Number == 1205)
                        {
                            debugger.AddToLog($"{exception.Message}{Environment.NewLine}DEADLOCK ERROR{Environment.NewLine}Query: '{GetCompiledSqlFromCommand(myCommand)}'");
                        }
                        else
                        {
                            debugger.AddToLog($"{exception.Message}{Environment.NewLine}Query: '{GetCompiledSqlFromCommand(myCommand)}'");
                        }
                    }
                }
                throw exception;
            }
        }

        //Normal

        protected virtual bool CoreProcessing(MySqlConnection myConnection, int counter = 0)
        {
            //Sets up the command object
            MySqlCommand myCommand = CoreCommandSetup();

            try
            {
                //Executes the query *if it does not throw an exception then it was done so successfully*
                var value = myCommand.ExecuteNonQuery();

                return true;
            }
            catch (MySqlException e)
            {
                //Runs the deadlock retry function
                return DeadLockRetry<bool>(myConnection, myCommand, counter, e, CoreProcessing);
            }
        }

        //SqlDataReader
        //Note: This is more quicker and effective on memory as it does not store all the results in memory only a single row before iteration

        protected virtual MySqlDataReader CoreProcessingReaderReturn(MySqlConnection myConnection, int counter = 0)
        {
            //Sets up the command object
            MySqlCommand myCommand = CoreCommandSetup();

            try
            {
                //Executes the command and returns the reader it into the reader
                return myCommand.ExecuteReader();
            }
            catch (MySqlException e)
            {
                //Runs the deadlock retry function
                return DeadLockRetry(myConnection, myCommand, counter, e, CoreProcessingReaderReturn);
            }
        }

        //SqlDataAdapter
        //Note: This stores all the result set directly in memory

        protected virtual MySqlDataAdapter CoreProcessingAdapterReturn(MySqlConnection myConnection, int counter = 0)
        {
            //Sets up the command object
            MySqlCommand myCommand = CoreCommandSetup();

            try
            {
                //Executes the command and puts it into the reader
                MySqlDataAdapter myAdapter = new MySqlDataAdapter(myCommand);

                return myAdapter;
            }
            catch (MySqlException e)
            {
                //Runs the deadlock retry function
                return DeadLockRetry(myConnection, myCommand, counter, e, CoreProcessingAdapterReturn);
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
                    //Clears the queries and stored procedure ready for the next
                    disposeAll();
                    compiledSql.Clear();
                    Procedure = null;
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
                    //Clears the queries and stored procedure ready for the next
                    disposeAll();
                    compiledSql.Clear();
                    Procedure = null;
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
                    //Clears the queries and stored procedure ready for the next
                    disposeAll();
                    compiledSql.Clear();
                    Procedure = null;
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
                    //Clears the queries and stored procedure ready for the next
                    disposeAll();
                    compiledSql.Clear();
                    Procedure = null;
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
                    //Clears the queries and stored procedure ready for the next
                    disposeAll();
                    compiledSql.Clear();
                    Procedure = null;
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
                    //Clears the queries and stored procedure ready for the next
                    disposeAll();
                    compiledSql.Clear();
                    Procedure = null;
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
                    //Clears the queries and stored procedure ready for the next
                    disposeAll();
                    compiledSql.Clear();
                    Procedure = null;
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
    }
}