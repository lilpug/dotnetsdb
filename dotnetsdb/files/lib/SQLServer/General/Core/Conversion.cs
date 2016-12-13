﻿using System.Collections.Generic;
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

        protected virtual string result_conversion_string(ref SqlDataReader myReader)
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
        protected virtual DataTable result_conversion_datatable(ref SqlDataReader myReader)
        {
            DataTable main_store = new DataTable();
            //Loads the data into a datatable format
            main_store.Load(myReader);
            return main_store;
        }

        //This functions takes the resulting data and converts it into a dataset format
        protected virtual DataSet result_conversion_dataset(ref SqlDataAdapter myAdapter, bool enforceConstraints)
        {
            DataSet ds = new DataSet();

            //Sets the constraints setup
            ds.EnforceConstraints = enforceConstraints;

            //Loads the data into a datatable format
            myAdapter.Fill(ds);

            return ds;
        }

        //This function takes the resulting data and puts every first row value into an array
        protected virtual string[] result_conversion_string_array(ref SqlDataReader myReader)
        {
            List<string> ls = new List<string>();

            while (myReader.Read())
            {
                ls.Add(myReader.GetValue(0).ToString());
            }

            return ls.ToArray();
        }

        //This functions takes the resulting data and converts it into a list of dynamic class objects
        protected virtual List<dynamic> result_conversion_dynamic(ref SqlDataReader myReader)
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