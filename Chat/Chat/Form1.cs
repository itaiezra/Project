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
            keyWords(question);
            int counter = 0,numOfKeyWords = keyWordsArray.Length, i=0,j=0;
            while(counter < db.numOfRows())
            {
                while (i < numOfKeyWords)
                {
                    if (j == 5)
                        return;
                    if (keyWordsArray[i]==null)
                        break;
                    if (db.findQuestion(counter).Contains(keyWordsArray[i]))
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

            finalAnswer = db.findAnswer(returningAnswers);
            answare(finalAnswer);


            //answare(db.findQuestion(3));
            //     answare(keyWords(question));
            //    string[] a = Extractor();

        }
        public void answare(string a)
        {
            textBox1.Text += "\r\n" + a;
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
                keyWords += "  |  " + key.Word;
                keyWordsArray[numOfKeywords] = key.Word;
                numOfKeywords++;
                if (numOfKeywords == 19)
                    break;
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
    }
}
