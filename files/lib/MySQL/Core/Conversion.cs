using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.Specialized; //Used for HybridDictionary
using System.Data;//Used for DataTable
using System.Dynamic;//Used for mysqlsqlcommand etc
using System.Text;

namespace DotNetSDB
{
    public partial class MySQLCore
    {
        /*##########################################*/
        /*        Data Conversion functions         */
        /*##########################################*/

        //This functions takes the resulting data and converts it into a json format string
        //Note: http://jsonformatter.curiousconcept.com/
        protected virtual string ResultToJson(ref MySqlDataReader myReader)
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
                    sb.Append($"\"{myReader.GetName(i)}\":");
                    sb.Append($"\"{myReader[i]}\"");

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
            sb.Append($"],\"columns\": {{ \"count\": {myReader.FieldCount}, \"names\": [");
            for (int i = 0; i < myReader.FieldCount; i++)
            {
                if (i == myReader.FieldCount - 1)
                {
                    sb.Append($"\"{myReader.GetName(i)}\"");
                }
                else
                {
                    sb.Append($"\"{myReader.GetName(i)}\",");
                }
            }

            sb.Append($"]}}, \"result_count\": {rowCount} }}");

            return sb.ToString();
        }

        //This function takes the first result value and returns it as a string
        protected virtual string ResultToString(ref MySqlDataReader myReader)
        {
            //Puts the first output into a string
            string value = null;
            if (myReader != null && myReader.Read())
            {
                value = myReader.GetValue(0).ToString();
            }
            return value;
        }

        //This functions takes the resulting data and converts it into a dataTable format
        protected DataTable ResultToDataTable(ref MySqlDataReader myReader)
        {
            DataTable main_store = new DataTable();
            //Loads the data into a datatable format
            main_store.Load(myReader);
            return main_store;
        }

        //This functions takes the resulting data and converts it into a dataset format
        protected DataSet ResultToDataSet(ref MySqlDataAdapter myAdapter, bool enforceConstraints)
        {
            DataSet ds = new DataSet();

            //Sets the constraints setup
            ds.EnforceConstraints = enforceConstraints;

            //Loads the data into a datatable format
            myAdapter.Fill(ds);

            return ds;
        }

        //This function takes the resulting data and puts every first row value into an array
        protected string[] ResultToStringArray(ref MySqlDataReader myReader)
        {
            List<string> ls = new List<string>();

            while (myReader.Read())
            {
                ls.Add(myReader.GetValue(0).ToString());
            }

            return ls.ToArray();
        }

        //This functions takes the resulting data and converts it into a list of dynamic class objects
        protected List<dynamic> ResultToDynamic(ref MySqlDataReader myReader)
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
    }
}