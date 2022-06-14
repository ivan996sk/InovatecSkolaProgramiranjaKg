using BankApp.Control;
using BankApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Data
{
    public static class CsvParse
    {
        public static void readCSV(string path, bool header)
        {
            Console.WriteLine();
            try
            {
                var reader = new StreamReader(path);

                List<string> listA = new List<string>();
                string line;

                if(header)
                     line = reader.ReadLine();

                while (!reader.EndOfStream)
                {
                    LoanApplication acceptedLoanApplications = new LoanApplication();
                    line = reader.ReadLine();
                    listA.Add(line);

                    //Console.WriteLine(line);     

                    var values = line.Split(',');

                    acceptedLoanApplications._id = "";
                    acceptedLoanApplications.bank = values[0].ToString() ;
                    acceptedLoanApplications.client = new Client()
                    {
                        name = values[1].ToString(),
                        idNumber = values[2].ToString(),
                        monthlyIncome = Convert.ToDouble(values[3]),
                        yearsExp = Convert.ToInt32(values[4])
                    };
                    acceptedLoanApplications.loanAmount = Convert.ToDouble(values[5]);
                    acceptedLoanApplications.numberOfEMI = Convert.ToInt32(values[6]);

                    BankControl.ApplyingForLoan(acceptedLoanApplications);

                }
            }
            catch
            {
                Console.WriteLine("Na datoj putanji ne postoji csv fajl. " +
                    "Molimo vas unesite drugu putunju\n" +
                    "Za povratak u glavni meni unesite 0.");

                path = Console.ReadLine();

                if (path == "0")
                {
                    Console.Clear();
                    App.menu();
                }
                else
                {
                    readCSV(path, header);
                }
            }
        }

    }
}
