using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Text;

namespace DotNetSDB
{
    public partial class SqlServerCore
    {
        /*##########################################*/
        /*        Data Conversion functions         */
        /*##########################################*/

        //This functions takes the resulting data and converts it into a json format string
        //Note: http://jsonformatter.curiousconcept.com/
        protected virtual string result_conversion_json(ref SqlDataReader myReader)
        {
            StringBuilder sb = new StringBuilder();
            if (myReader == null || myReader.FieldCount == 0)
            {
                sb.Append("null");
                return null;
            }

            int rowCount = 0;

            sb.Append(@"{""rows"":[");

            while (myReader.Read())
            {
                sb.Append("{");

                for (int i = 0; i < myReader.FieldCount; i++)
                {
                    sb.Append("\"" + myReader.GetName(i) + "\":");
                    sb.Append("\"" + myReader[i] + "\"");

                    sb.Append(i == myReader.FieldCount - 1 ? "" : ",");
                }

                sb.Append("},");

                rowCount++;
            }

            if (rowCount > 0)
            {
                int index = sb.ToString().LastIndexOf(",");
                sb.Remove(index, 1);
            }

            //Adds the column information section:
            sb.Append("],\"columns\": { \"count\": " + myReader.FieldCount + ", \"names\": [");
            for (int i = 0; i < myReader.FieldCount; i++)
            {
                if (i == myReader.FieldCount - 1)
                {
                    sb.Append("\"" + myReader.GetName(i) + "\"");
                }
                else
                {
                    sb.Append("\"" + myReader.GetName(i) + "\",");
                }
            }
            sb.Append("]}, \"result_count\": " + rowCount + "}");

            return sb.ToString();
        }

        //This functions takes the resulting data and converts it into a dataTable format
        protected DataTable result_conversion_datatable(ref SqlDataReader myReader)
        {
            DataTable main_store = new DataTable();
            //Loads the data into a datatable format
            main_store.Load(myReader);
            return main_store;
        }

        //This functions takes the resulting data and converts it into a dataset format
        protected DataSet result_conversion_dataset(ref SqlDataAdapter myAdapter, bool enforceConstraints)
        {
            DataSet ds = new DataSet();

            //Sets the constraints setup
            ds.EnforceConstraints = enforceConstraints;

            //Loads the data into a datatable format
            myAdapter.Fill(ds);

            return ds;
        }

        //This function takes the resulting data and puts every first row value into an array
        protected string[] result_conversion_array(ref SqlDataReader myReader)
        {
            List<string> ls = new List<string>();

            while (myReader.Read())
            {
                ls.Add(myReader.GetValue(0).ToString());
            }

            return ls.ToArray();
        }

        //This functions takes the resulting data and converts it into a list of dynamic class objects
        protected List<dynamic> result_conversion_dynamic(ref SqlDataReader myReader)
        {
            //Builds the temporary storage object
            List<dynamic> temp = new List<dynamic>();

            //Loops over the reader row by row
            while (myReader.Read())
            {
                //Creates the expandoObject for this row
                dynamic main = new ExpandoObject();

                //This uses the main expandoObject as a reference to add new properties to it without knowing the name at design time
                var expandoObject = main as IDictionary<string, object>;

                //Loops over the columns and values for this particular row
                for (int i = 0; i < myReader.FieldCount; i++)
                {
                    expandoObject.Add(myReader.GetName(i), myReader[i]);
                }

                //Return the original dynamic object now we have the propeties added
                temp.Add(main);
            }

            //Returns the new dynamic list
            return temp;
        }

        //This function allows datatable results to be grouped together via an ID
        //Note: i.e. multiple of the same userid's for each keyword grouped into 1 userid for all the keywords
        public DataTable result_merge(DataTable theHolder, string idColumn, object mergeColumn, bool removeDuplicates = false)
        {
            //Sets up the error output just in case something goes wrong
            DataTable error = new DataTable();
            error.Columns.Add("errro");

            try
            {
                /*General Note: We add the columns to the second datatable rather then cloning the one passed because,
                    while cloning the order of the fields can change, this will cause confliction problems when a merge value is being put
                    possibly in the id field when it should be in the merge field. To solve this we just added the columns manually so that
                    the order which we process them is always the same and does not dynamically change. */

                //Checks whether there is only a single mergeColumn
                if (mergeColumn.GetType() == typeof(string))
                {
                    //Duplicates all the columns of the datatable
                    DataTable second = new DataTable();

                    //Adds the columns
                    second.Columns.Add(idColumn, typeof(string));
                    second.Columns.Add((string)mergeColumn, typeof(string[]));

                    //Variables for holding the results while processing
                    string temp = "";
                    List<string> store = new List<string>();

                    //Loops through all the rows in the datatable
                    for (int i = 0; i < theHolder.Rows.Count; i++)
                    {
                        if (temp == "")//If its the first run simply add the id to the temp and add the column to the list
                        {
                            //Puts the id into the temp storage and adds the first value to the list
                            temp = theHolder.Rows[i][idColumn].ToString();
                            store.Add(theHolder.Rows[i][mergeColumn.ToString()].ToString());
                        }
                        else if (temp == theHolder.Rows[i][idColumn].ToString())//if still the same ID simply add to the list again
                        {
                            //Checks if there is any duplicate values in the list and if the removeDuplicate flag is true
                            if (removeDuplicates && store.IndexOf(theHolder.Rows[i][mergeColumn.ToString()].ToString()) != -1)
                            {//Do nothing and do not add
                            }
                            else
                            {
                                store.Add(theHolder.Rows[i][mergeColumn.ToString()].ToString());
                            }
                        }
                        else//If the ID has changed then output the row with the built list and start a new one ready for accumulating the list again
                        {
                            //Orders the content in the list
                            store.Sort();

                            //Adds the list as a string array to the datatable
                            second.Rows.Add(temp, store.ToArray());

                            //Clears the list
                            store.Clear();

                            //Puts the id into the temp storage and adds the first value to the list
                            temp = theHolder.Rows[i][idColumn].ToString();
                            store.Add(theHolder.Rows[i][mergeColumn.ToString()].ToString());
                        }
                    }

                    //If there is any left over that have not been added to the datatable it adds it
                    if (store.Count != 0)
                    {
                        store.Sort();
                        second.Rows.Add(temp, store.ToArray());
                    }

                    //Returns the new datatable
                    return second;
                }
                else if (mergeColumn.GetType() == typeof(string[]))//Checks if there is multiple columnns to merge
                {
                    //Duplicates all the columns of the datatable
                    DataTable second = new DataTable();

                    //Adds the columns
                    second.Columns.Add(idColumn, typeof(string));

                    //Variables for holding the results while processing
                    string temp = "";
                    HybridDictionary store = new HybridDictionary();

                    //Loops through the columns and adds lists ready for them
                    foreach (string name in (string[])mergeColumn)
                    {
                        //Creates a new list for the column in the hybrid
                        store.Add(name, new List<string>());

                        //Adds the columns
                        second.Columns.Add(name, typeof(string[]));
                    }

                    //Loops through all the rows in the datatable
                    for (int i = 0; i < theHolder.Rows.Count; i++)
                    {
                        if (temp == "")//If its the first run simply add the id to the temp and add the column to the list
                        {
                            //Puts the id into the temp storage and adds the first column values to the list
                            temp = theHolder.Rows[i][idColumn].ToString();
                            foreach (string name in (string[])mergeColumn)
                            {
                                List<string> temp_list = (List<string>)store[name];
                                temp_list.Add(theHolder.Rows[i][name].ToString());
                                store[name] = temp_list;
                            }
                        }
                        else if (temp == theHolder.Rows[i][idColumn].ToString())//if still the same ID simply add to the list again
                        {
                            foreach (string name in (string[])mergeColumn)
                            {
                                List<string> temp_list = (List<string>)store[name];

                                //Checks if there is any duplicate values in the list and if the removeDuplicate flag is true
                                if (removeDuplicates && temp_list.IndexOf(theHolder.Rows[i][name].ToString()) != -1)
                                {//Do nothing and do not add
                                }
                                else
                                {
                                    temp_list.Add(theHolder.Rows[i][name].ToString());
                                    store[name] = temp_list;
                                }
                            }
                        }
                        else//If the ID has changed then output the row with the built list and start a new one ready for accumulating the list again
                        {
                            //Creates a new row with all the merged lists as their columns
                            DataRow rw = second.NewRow();
                            rw.SetField(idColumn, temp);
                            foreach (string name in (string[])mergeColumn)
                            {
                                List<string> temp_list = (List<string>)store[name];
                                //Orders the content in the list
                                temp_list.Sort();
                                rw.SetField(name, temp_list.ToArray());

                                //Wipes the list now its been added
                                store[name] = new List<string>();
                            }
                            second.Rows.Add(rw);

                            //Puts the id into the temp storage and adds the first column values to the list
                            temp = theHolder.Rows[i][idColumn].ToString();
                            foreach (string name in (string[])mergeColumn)
                            {
                                List<string> temp_list = (List<string>)store[name];
                                temp_list.Add(theHolder.Rows[i][name].ToString());
                                store[name] = temp_list;
                            }
                        }
                    }

                    //Adds the final one
                    if (store.Count != 0)
                    {
                        //Creates a new row with all the merged lists as their columns
                        DataRow rw = second.NewRow();
                        rw.SetField(idColumn, temp);
                        foreach (string name in (string[])mergeColumn)
                        {
                            List<string> temp_list = (List<string>)store[name];
                            //Orders the content in the list
                            temp_list.Sort();
                            rw.SetField(name, temp_list.ToArray());
                        }

                        second.Rows.Add(rw);
                    }

                    return second;
                }
            }
            catch (Exception)
            {
                return error;
            }

            return error;
        }
    }
}