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

            string[] allQuestions = new string[numOfRows("dbo.Table_Questions")];
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
        public int numOfRows(string table)
        {
            SqlConnection conn = new SqlConnection(abc.ToString());

            int numOfRows = 0;
            using (conn)
            {

                SqlCommand command = new SqlCommand("SELECT * FROM " + table, conn);
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
        public void feedback(string q, string a, int yn)
        {
            SqlConnection conn = new SqlConnection(abc.ToString());
            int rowNumber = numOfRows("dbo.Table_Feedback");
            rowNumber++;
            using (conn)
            {
                
              //  SqlCommand command = new SqlCommand("INSERT INTO [dbo].[Table_Feedback] ([Id], [Question], [Answer], [Helpful]) VALUES ("+rowNumber+", N'"+q+"', N'"+a+"', "+yn+")", conn);
                SqlCommand command = new SqlCommand("INSERT INTO [dbo].[Table_Feedback] ([Id], [Question], [Answer], [Helpful]) VALUES (@rowNumber, @QUestion, @Answer, @Helpful)", conn);
                command.Connection = conn;
                command.Parameters.AddWithValue("@rowNumber", rowNumber);
                command.Parameters.AddWithValue("@QUestion", q);
                command.Parameters.AddWithValue("@Answer", a);
                command.Parameters.AddWithValue("@Helpful", yn);
                conn.Open();
                command.ExecuteNonQuery();
            }
            conn.Close();

            
        }
        public int haveAnswer(string q)
        {
            SqlConnection conn = new SqlConnection(abc.ToString());

            int answerNum = 0;
            string[] Questions = getAllQuestions();
            for (int i=0;i<Questions.Length;i++)
            {
                if (q.Equals(Questions[i]))
                {
                    answerNum = i+1;
                    break;
                }
            }
            return answerNum;
        }
        public string getAnswer(int a)
        {
            SqlConnection conn = new SqlConnection(abc.ToString());

            
            string answer = "";
            using (conn)
            {
                
                    SqlCommand command = new SqlCommand("SELECT * FROM dbo.Table_Answer WHERE Id =" + a, conn);
                    conn.Open();
                    SqlDataReader read = command.ExecuteReader();
                    if (read.HasRows)
                        while (read.Read())
                        {
                            answer += System.Environment.NewLine;
                            answer += read.GetString(1);
                            answer += System.Environment.NewLine;


                        }
                    conn.Close();
                
            }
            conn.Close();

            return answer;
        }

        public string advancedSearch(string[] keyWordsArray)
        {
            string temp = "";
            string[] allQuestions = getAllQuestions();
            int[] answerRating = new int[allQuestions.Length];
            int max = 0;

            for (int i = 0; i < allQuestions.Length; i++)
                for (int j = 0; j < keyWordsArray.Length;j++)
                {
                    if (keyWordsArray[j] == "-0-")
                        break;
                    if (allQuestions[i].Contains(keyWordsArray[j]))
                        answerRating[i]++;
                }

            int[] bestAnswers = new int[5];

            for (int i = 0; i < answerRating.Length;i++)
            {
                if (max < answerRating[i])
                    max = answerRating[i];
            }
            int location = 0;
            int tempMax = max;
            for (int i = 0; i < answerRating.Length; i++)
                for (tempMax = max; tempMax > 0; tempMax-- )
                    if(answerRating[i]==max)
                    {
                        bestAnswers[location] = i + 1;
                        if (location == 4)
                        {
                            temp = findAnswer(bestAnswers);
                            return temp;
                        }
                    }

            temp = findAnswer(bestAnswers);
            return temp;
        }
    }
}

