using BankApp.Data;
using BankApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Control
{
    internal class BankControl
    {
        
        public static void ApplyingForLoan(LoanApplication application)
        {
            DataAccess db = new DataAccess();
            bool isAccepted = false;


            Bank bank = null;
            if ((application.bank).Equals("RBC"))
            {
                bank = new RBC();
                isAccepted = bank.checkTerms(application);
            }
            else if ((application.bank).Equals("Santander"))
            {
                bank = new Santander();
                isAccepted = bank.checkTerms(application); ;
            }
            else if ((application.bank).Equals("Wells Fargo"))
            {
                bank = new WellsFargo();
                isAccepted = bank.checkTerms(application);
            }
            

            if (isAccepted)
            {
                clientAcceptedMsg(application.client.name, application.client.idNumber, application.bank);
                db.AddLoan(application);
            }
            else
            {
                clientDeclinedMsg(application.client.name, application.client.idNumber);
            }
        }

        public static bool checkBank(string clientId, string bankName)
        {
            DataAccess db = new DataAccess();
            List<LoanApplication> application;

            application = db.GetClientLoans(clientId, bankName);

            if (application.Count == 0)
            {
                Client client = new Client();
                client = db.GetClient(clientId);

                Console.WriteLine(String.Format("Klijent {0} sa JMBG-om {1} nema odobreni kredit u banci {2}.", client.name, client.idNumber, bankName));

                Console.WriteLine("Molimo vas unesite drugo ime banke. Za povratak u glavni meni unesite 0.");
                return false;
            }
            else
            {

                //ne radi za app == null
                Console.WriteLine("Klijent " + application[0].client.name + " sa JMBG - om " + application[0].client.idNumber + " ima sledeci kredit/kredite: ");
                for (int i = 0; i < application.Count; i++)
                {
                    Console.WriteLine(String.Format("Kredit kod banke {0} u iznosu od {1} a preostali broj rata je {2}.", application[i].bank, application[i].loanAmount, application[i].numberOfEMI));
                }
                return true;
            }
        }

        public static void bankEnt(string clientId, string bankName)
        {


            bool bankEx = checkBank(clientId, bankName);
            while (!bankEx)
            {
                bankName = Console.ReadLine();
                if (bankName.Equals("0"))
                {
                    Console.Clear();
                    app.menu();
                }
                bankEx = checkBank(clientId, bankName);
            }

            Console.WriteLine("Za povratak u glavni meni unesite 0. Za proveru kredita u drugoj banci, unesite ime banke.");
            string opt = Console.ReadLine();
            if (opt.Equals("0"))
            {
                Console.Clear();
                app.menu();
            }
            else
            {
                bankEnt(clientId, opt);
            }
        }
        public static void clientAcceptedMsg(string clientName, string clientId, string bankName)
        {
            Console.WriteLine(String.Format("Klijent {0}, JMBG: {1} je uspešno aplicirao za kredit u banci {2}.", clientName, clientId, bankName));
        }

        public static void clientDeclinedMsg(string clientName, string clientId)
        {
            Console.WriteLine(String.Format("Klijent {0}, JMBG: {1} ne ispunjava uslove za odobrenje kredita!", clientName, clientId));
        }
    }
}
