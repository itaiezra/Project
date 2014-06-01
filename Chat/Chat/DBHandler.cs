using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.Sql;

namespace Chat
{
    class DBHandler
    {
        public static string connectionString = @"Data Source=(LocalDB)\v11.0;AttachDbFilename=E:\My Files\Documents\School\PROJECT\Chat\Chat\Database.mdf;Integrated Security=True";
       // public static string connectionString = @"Data Source=(LocalDB)\v11.0;AttachDbFilename=E:\Project\Chat\Chat\Database.mdf;Integrated Security=True";
        public static SqlConnectionStringBuilder abc = new SqlConnectionStringBuilder(connectionString);
      //  public static SqlConnection conn = new SqlConnection(abc.ToString());

        public void addQuestion(string q)
        {
          //  SqlConnection conn = new SqlConnection(connectionString);
          //  conn.Open();


        }
        public void addAswer(string a)
        {
         //   SqlConnection conn = new SqlConnection(connectionString);
          //  conn.Open();
        }
        public string findQuestion(int q)
        {
            SqlConnection conn = new SqlConnection(abc.ToString());
            string question = "";
            using (conn)
            {

                SqlCommand command = new SqlCommand("SELECT * FROM dbo.Table_Questions WHERE Id =" + q, conn);
                conn.Open();
                SqlDataReader read = command.ExecuteReader();
                if (!read.HasRows)
                    question = "no rows";
                else
                    while (read.Read())
                    {
                        question += read.GetString(1);
                    }
            }
            conn.Close();
            return question;
        }
        public string[] getAllQuestions()
        {
            SqlConnection conn = new SqlConnection(abc.ToString());

            string[] allQuestions = null;
            using (conn)
            {
                int i = 0;
                SqlCommand command = new SqlCommand("SELECT * FROM dbo.Table_Questions", conn);
                conn.Open();
                SqlDataReader read = command.ExecuteReader();
                if (!read.HasRows)
                {
                    allQuestions[0] = "no rows";
                    return allQuestions;
                }
                else
                    while (read.Read())
                    {
                        allQuestions[i] = read.GetString(1);
                        i++;
                    }
            }
            conn.Close();

            return allQuestions;
        }
        public string findAnswer(int[] a)
        {
            SqlConnection conn = new SqlConnection(abc.ToString());

            if (a == null ||  a[0] == 0)
                return "We could not find any answers matching your search.";
            string answer = "";
            using (conn)
            {
                for (int i = 0; i < a.Length; i++)
                {
                    if (a[i] == 0)
                        break;
                    SqlCommand command = new SqlCommand("SELECT * FROM dbo.Table_Answer WHERE Id =" + a[i], conn);
                    conn.Open();
                    SqlDataReader read = command.ExecuteReader();
                    if (read.HasRows)
                        while (read.Read())
                        {
                            answer += "Answer number: " + a[i];
                            answer += System.Environment.NewLine;
                            answer += read.GetString(1);
                            answer += System.Environment.NewLine;
                            answer += "****************************************************************************";
                            answer += System.Environment.NewLine;

                        }
                    conn.Close();
                }
            }
            conn.Close();

            return answer;
        }
        public int numOfRows()
        {
            SqlConnection conn = new SqlConnection(abc.ToString());

            int numOfRows = 0;
            using (conn)
            {

                SqlCommand command = new SqlCommand("SELECT * FROM dbo.Table_Questions", conn);
                conn.Open();
                SqlDataReader read = command.ExecuteReader();
                if (!read.HasRows)
                    numOfRows = 0;
                else
                    while (read.Read())
                    {
                        numOfRows++;
                    }
            }
            conn.Close();

            return numOfRows;
        }
    }
}

