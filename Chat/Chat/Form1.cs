using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SemanticLibrary;


namespace Chat
{
    public partial class Form1 : Form
    {

        public static string question, finalAnswer;
        public static string[] keyWordsArray = new string[20];
        public static int[] returningAnswers = new int[5];
        public string q = "";
        public static int fileNumber = 1;
        public static string fileName = fileNumber + ".txt";
        public static readonly int NUM_OF_FILES = 56;

        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text += "\r\n" + textBox2.Text;
            question = textBox2.Text;
            textBox2.Text = "";
            calc();

        }
        public void calc()
        {

            DBHandler db = new DBHandler();

            if (keyWords(question) != "")
            {
                int counter = 1, numOfKeyWords = keyWordsArray.Length, i = 0, j = 0, numOfRows = db.numOfRows();
                while (counter < numOfRows)
                {
                    while (i < numOfKeyWords)
                    {
                        if (j == 5)
                        {
                            finalAnswer = db.findAnswer(returningAnswers);
                            clearReturningAnswers();
                            answare(finalAnswer);
                            return;
                        }
                        if (keyWordsArray[i] == null)
                        {
                            i = 0;
                            break;
                        }
                        string a = db.findQuestion(counter);
                        string b = keyWordsArray[i];
                        if (a.Contains(b))
                        {
                            returningAnswers[j] = counter;
                            j++;
                            i = 0;
                            break;
                        }

                        i++;

                    }

                    counter++;
                }
                if (j != 0)
                {
                    finalAnswer = db.findAnswer(returningAnswers);
                    clearReturningAnswers();
                    answare(finalAnswer);
                }
                else
                {
                    answare("");

                }
            }
            else
            {
                answare("");
            }
        }
        public void answare(string a)
        {
            textBox1.Text += "your Key-Words are:";
            textBox1.Text += System.Environment.NewLine;
            textBox1.Text += System.Environment.NewLine;
            textBox1.Text += keyWords(question);
            textBox1.Text += System.Environment.NewLine;
            textBox1.Text += System.Environment.NewLine;
            if(a=="")
                textBox1.Text += "We could not find an answe to your question in our DB.";
            else
                textBox1.Text += a;

        }
        public static string keyWords(string question)
        {
            //Note: you will have to supply your own text files
            //string gettys = File.ReadAllText(question);
            //string gu = File.ReadAllText(question);

            string keyWords = "";
            string gettys = question;
            string gu = question;
            int numOfKeywords = 0;



            KeywordAnalyzer ka = new KeywordAnalyzer();

            var g = ka.Analyze(gettys);

            var s = ka.Analyze(gu);

            Console.WriteLine("gettys");
            foreach (var key in g.Keywords)
            {
                Console.WriteLine("   key: {0}, rank: {1}", key.Word, key.Rank);
            }

            Console.WriteLine("gu");
            foreach (var key in s.Keywords)
            {
                Console.WriteLine("   key: {0}, rank: {1}", key.Word, key.Rank);
            }

            Console.WriteLine("gettys");
            var gty = (from n in g.Keywords select n).Take(10);
            foreach (var key in gty)
            {
                Console.WriteLine("   {0}", key.Word);
            }

            Console.WriteLine("gu");
            var gus = (from n in s.Keywords select n).Take(10);
            foreach (var key in gus)
            {
                Console.WriteLine("   {0}", key.Word);
                //-----------------------------------------------------------------------------------------------
                if (!(key.Word == "use" || key.Word == "how"))
                {
                    keyWords += "  |  " + key.Word;
                    keyWordsArray[numOfKeywords] = key.Word;
                    numOfKeywords++;
                    if (numOfKeywords == 19)
                        break;
                }
            }
            keyWordsArray[numOfKeywords] = "-0-";

            Console.ReadLine();
            return keyWords;
        }

        public static string[] Extractor()
        {

            string[] posibleAnswares = new string[5];
            int numOfPosibleAnswares = 0;
            // bool exists = false;
            string keyWord;
            string newQuestion = "";
            for (fileNumber = 1; fileNumber <= NUM_OF_FILES; fileNumber++)
            {
                for (int i = 0; i < 20; i++)
                {
                    fileName = fileNumber + ".txt";
                    keyWord = keyWordsArray[i];
                    if (keyWord == "-0-")
                        break;
                    /*
                    exists = (from line in File.ReadAllLines(@"E:\My Files\Documents\School\PROJECT\Chat\DB\Q\" + fileName)
                              where line == keyWord
                              select line).Count() > 0;
                     
                    if (exists)
                    */
                    newQuestion = File.ReadAllText(@"E:\My Files\Documents\School\PROJECT\Chat\DB\Q\" + fileName);
                    if (newQuestion.Contains(keyWord))
                    {
                        posibleAnswares[numOfPosibleAnswares] = fileName;
                        numOfPosibleAnswares++;
                        if (numOfPosibleAnswares == 5)
                            return posibleAnswares;
                        break;
                        // exists = false;
                    }
                }
            }
            return posibleAnswares;
        }
        public static void clearReturningAnswers()
        {
            for (int i = 0; i < 5; i++)
                returningAnswers[i] = 0;
        }
    }
}
