using System;
using System.Collections.Generic;

namespace DotNetSDB
{
    public partial class MySQLCore
    {
        /*##########################################*/
        /*          SP Validation functions         */
        /*##########################################*/

        /// <summary>
        /// This function runs validation checks against the stored procedure before setting it up
        /// </summary>
        /// <param name="name"></param>
        protected void ProcedureValidation(string name)
        {
            if(Procedure != null)
            {
                throw new Exception("Stored Procedure Error: A stored procedure has already been loaded, you can only run a stored procedure one at a time.");
            }
            else if (string.IsNullOrWhiteSpace(name))
            {
                throw new Exception("Stored Procedure Error: The procedure name supplied is empty.");
            }
        }

        /*##########################################*/
        /*              SP Add function             */
        /*##########################################*/
        
        /// <summary>
        /// This function adds a stored procedure and its parameters to be run
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parameters"></param>
        public virtual void add_stored_procedure(string name, Dictionary<string, object> parameters = null)
        {
            //Checks the validation is ok before continuing, will throw an exception if not
            ProcedureValidation(name);

            //Creates the stored procedure object ready for execution
            Procedure = new StoredProcedure()
            {
                Name = name,
                Parameters = parameters
            };              
        }
    }
}