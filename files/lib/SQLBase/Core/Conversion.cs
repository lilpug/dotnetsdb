using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace DotNetSDB
{
    public abstract partial class SQLBase : IDisposable
    {
        /*##########################################*/
        /*        Data Conversion functions         */
        /*##########################################*/

        /// <summary>
        /// This function returns a raw string from a datatable using the specified parameters
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        protected string DataTableToRawString(DataTable dt, string delimiter = ",")
        {
            //Initialises the string builder
            StringBuilder sb = new StringBuilder();

            //Adds a blank new line at the start
            sb.AppendLine("");

            //Gets all the datatables headers
            IEnumerable<string> columnNames = dt.Columns.Cast<DataColumn>().
                                              Select(column => column.ColumnName);
            sb.AppendLine(string.Join(delimiter, columnNames));

            //Gets all the datatables row data
            foreach (DataRow row in dt.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field =>
                  string.Concat("\"", field.ToString().Replace("\"", "\"\""), "\""));
                sb.AppendLine(string.Join(delimiter, fields));
            }

            //Adds a blank new line at the end
            sb.AppendLine("");

            return sb.ToString();
        }

        /// <summary>
        /// This function creates a csv file from a datatable using the specified parameters
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="fullFilePath"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        protected bool DataTableToCSV(DataTable dt, string fullFilePath, string delimiter = ",")
        {
            //Checks to ensure the parent directory exists
            if (Directory.Exists(Path.GetDirectoryName(fullFilePath)))
            {
                //Adds the csv extension if one has not been supplied in the file path
                string extension = Path.GetExtension(fullFilePath);
                if (string.IsNullOrWhiteSpace(extension))
                {
                    extension = ".csv";
                }

                //Gets the raw datatable output
                string rawOutput = DataTableToRawString(dt, delimiter);

                //Creates the file
                return CreateFile($@"{Path.GetDirectoryName(fullFilePath)}\{Path.GetFileNameWithoutExtension(fullFilePath)}{extension}", rawOutput);
            }

            return false;
        }

        /// <summary>
        /// This function appends to an existing csv file from a datatable using the specified parameters
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="fullFilePath"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        protected bool DataTableAppendCSV(DataTable dt, string fullFilePath, string delimiter = ",")
        {
            //Adds the csv extension if one has not been supplied in the file path
            string extension = Path.GetExtension(fullFilePath);
            if (string.IsNullOrWhiteSpace(extension))
            {
                extension = ".csv";
            }

            //Checks to ensure the file already exists before we try appending to it
            if (File.Exists($@"{Path.GetDirectoryName(fullFilePath)}\{Path.GetFileNameWithoutExtension(fullFilePath)}{extension}"))
            {
                //Gets the raw datatable output
                string rawOutput = DataTableToRawString(dt, delimiter);

                //Creates the file
                return AppendFile($@"{Path.GetDirectoryName(fullFilePath)}\{Path.GetFileNameWithoutExtension(fullFilePath)}{extension}", rawOutput);
            }

            return false;
        }

    }
}