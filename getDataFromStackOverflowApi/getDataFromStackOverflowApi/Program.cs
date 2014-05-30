using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
//using Json.Net;

namespace getDataFromStackOverflowApi
{
    class Program
    {
        static void Main(string[] args)
        {
            //  string mydocpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            StringBuilder sb = new StringBuilder();
            string txtName = @"E:\My Files\Documents\School\PROJECT\KeyWord_Extractor\Q\Q.txt";
            //  foreach (string txtName in Directory.EnumerateFiles(mydocpath, "*.txt"))
            {
                using (StreamReader sr = new StreamReader(txtName))
                {
                    sb.AppendLine(txtName.ToString());
                    sb.AppendLine("= = = = = =");
                    sb.Append(sr.ReadToEnd());
                    sb.AppendLine();
                    sb.AppendLine();
                }
            }

            using (StreamWriter outfile = new StreamWriter(@"E:\My Files\Documents\School\PROJECT\KeyWord_Extractor\Q\MyQ.txt"))
            {
                outfile.Write(sb.ToString());
            }


        }
        public static void readQ()
        {
           
        }
        
        
    }
}
