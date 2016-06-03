namespace DotNetSDB
{
    public partial class SqlServer2008 : SqlServerCore
    {
        /*##########################################*/
        /*        Limit Compiling functions         */
        /*##########################################*/

        /* The general query idea:-
                        --If the hash table already exists for some reason, it removes it
		                IF OBJECT_ID('tempdb..#database2008limitwrapper') IS NOT NULL
	                        begin
		                        drop table #database2008limitwrapper
	                        end

		                --selects the limited results into the hash table
                        select * into #database2008limitwrapper from
		                (
				            --using an extra column of row_number to calculate the limit
				            SELECT students.* , ROW_NUMBER() over (order by (select 0)) as theLimitRow FROM students
                        ) as a
                        where a.theLimitRow >= 1 and a.theLimitRow <= 10

		                --drops the extra column used to calculate the correct rows
		                alter table #database2008limitwrapper
                        drop column theLimitRow

		                --displays the results without the extra column
                        select * from #database2008limitwrapper

		                --removes the hash table as we do not need it anymore
                        drop table #database2008limitwrapper
                        ;
            */

        /* Old function but keeping for awhile to ensure the new one does not break */
        //This function is the sql server limit wrapper that allows us to limit the rows
        /*protected string limit_create(query3 theQuery, string orderby, string compiling)
        {
            if (orderby != "")
            {
                compiling = compiling.Replace(orderby, " ");
                compiling = compiling.Replace("FROM", ", ROW_NUMBER() over (" + orderby + ") as theLimitRow FROM ");
            }
            else
            {
                compiling = compiling.Replace("FROM", ", ROW_NUMBER() over (order by (select 0)) as theLimitRow FROM ");
            }

            compiling = "with tableRowsForLimiting as ( " + compiling + " ) Select * from tableRowsForLimiting";
            compiling = compiling + " where theLimitRow >= " + theQuery.limit_count_one.ToString() + " and theLimitRow <= " + theQuery.limit_count_two.ToString();

            //If there is an order by this section pulls it apart and rebuilds it for the limit wrapper

            if (orderby != "")
            {
                //This removes the start part for processing
                orderby = orderby.ToLower().Replace("order by", " ").TrimEnd();

                //This loops over the string looking for another table name for order by
                while (orderby.IndexOf(".") != -1)
                {
                    //This obtains the table name before the.
                    int end = orderby.IndexOf(".");
                    int start = orderby.IndexOf(" ");

                    //This replaces the table name with the new table name and the delimiter
                    orderby = orderby.Replace(orderby.Substring(start, (end + 1) - start), " tableRowsForLimiting@@@");

                    //This section now looks to see if theres a next section, if there is it delimits the spaces on the current
                    //string we have just processed so we do not do it again
                    end = orderby.IndexOf(",");
                    start = orderby.IndexOf(" ");
                    if (start < end)
                    {
                        orderby = orderby.Replace(orderby.Substring(start, (end + 1) - start), orderby.Substring(start, (end) - start).Replace(" ", "___") + "{}{}");
                    }
                }

                //This puts the start of the order by back and replaces all the delimiters with the real characters again.
                orderby = " ORDER BY " + orderby.Replace("@@@", ".").Replace("___", " ").Replace("{}{}", ",");

                compiling = compiling + " " + orderby;
            }

            return compiling;
        }*/

        //This function is the sql server limit wrapper that allows us to limit the rows
        protected string limit_create(query3 theQuery, string orderby, string compiling)
        {
            if (orderby != "")
            {
                compiling = compiling.Replace(orderby, " ");
                compiling = compiling.Replace("FROM", ", ROW_NUMBER() over (" + orderby + ") as theLimitRow FROM ");
            }
            else
            {
                compiling = compiling.Replace("FROM", ", ROW_NUMBER() over (order by (select 0)) as theLimitRow FROM ");
            }

            string hashCheck = @"
                --If the hash table already exists for some reason, it removes it
		        IF OBJECT_ID('tempdb..#database2008limitwrapper') IS NOT NULL
	                begin
		                drop table #database2008limitwrapper
	                end
                ";
            compiling = hashCheck + " select * into #database2008limitwrapper from  ( " + compiling + " ) as a";
            compiling = compiling + " where a.theLimitRow >= " + theQuery.limit_count_one.ToString() + " and a.theLimitRow <= " + theQuery.limit_count_two.ToString();

            //If there is an order by this section pulls it apart and rebuilds it for the limit wrapper

            if (orderby != "")
            {
                //This removes the start part for processing
                orderby = orderby.ToLower().Replace("order by", " ").TrimEnd();

                //This loops over the string looking for another table name for order by
                while (orderby.IndexOf(".") != -1)
                {
                    //This obtains the table name before the.
                    int end = orderby.IndexOf(".");
                    int start = orderby.IndexOf(" ");

                    //This replaces the table name with the new table name and the delimiter
                    orderby = orderby.Replace(orderby.Substring(start, (end + 1) - start), " a@@@");

                    //This section now looks to see if theres a next section, if there is it delimits the spaces on the current
                    //string we have just processed so we do not do it again
                    end = orderby.IndexOf(",");
                    start = orderby.IndexOf(" ");
                    if (start < end)
                    {
                        orderby = orderby.Replace(orderby.Substring(start, (end + 1) - start), orderby.Substring(start, (end) - start).Replace(" ", "___") + "{}{}");
                    }
                }

                //This puts the start of the order by back and replaces all the delimiters with the real characters again.
                orderby = " ORDER BY " + orderby.Replace("@@@", ".").Replace("___", " ").Replace("{}{}", ",");

                compiling = compiling + " " + orderby;
            }

            string extraEndWrapper = @"
                --drops the extra column used to calculate the correct rows
                alter table #database2008limitwrapper
                drop column theLimitRow

                --displays the results without the extra column
                select * from #database2008limitwrapper

                --removes the hash table as we do not need it anymore
                drop table #database2008limitwrapper
                ";
            compiling = compiling + extraEndWrapper;

            return compiling;
        }

        /*##########################################*/
        /*           Main Front functions           */
        /*##########################################*/

        public void add_limit(int minValue, int maxValue)
        {
            query theMain = get_query();
            query3 theQuery = get_query3();

            theQuery.limit_count_one = minValue;
            theQuery.limit_count_two = maxValue;

            theMain.orderList.Add("limit");
        }
    }
}