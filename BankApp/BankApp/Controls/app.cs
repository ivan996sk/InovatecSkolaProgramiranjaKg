using BankApp.Control;
using BankApp.Data;
using BankApp.Models;
using System.Collections;

namespace BankApp
{
    public class app
    {
        public static void menu()
        {
            DataAccess db = new DataAccess();

            Console.WriteLine("GLAVNI MENI\n" +
            "Izaberite jednu od ponudjenih opcija:\n" +
            "1. Plati ratu kredita.\n" +
            "2. Prikaži informacije o kreditu.\n" +
            "3. Unesi nove kreditne aplikacije iz CSV-a.\n" +
            "4. Unesi aplikaciju za kredit.\n");

            Console.WriteLine("Odabir se vrši unosom rednog broja opcije.");

            string optionNo = Console.ReadLine();



            if (optionNo.Equals("1") || optionNo.Equals("1."))
            {
                List<LoanApplication> application;

                string clientId;
                string bankName;

                Console.WriteLine("Uspesno ste izabrali opciju 1.\nDa biste platili ratu, molimo vas unesite JMBG klijenta.");
                clientId = Console.ReadLine();

                while (clientId.Length != 13 )
                {
                    Console.WriteLine("Neispravan unos JMBG-a. Pokušajte ponovo.");
                    clientId = Console.ReadLine();
                }

                bool userEx;
                userEx = db.ClientExists(clientId);

                while (!userEx)
                {
                    Console.WriteLine("Klijent nema ni u jednoj banci kredit. Molimo vas unesite novi JMBG ili unesite 0 za povratak u glavni meni");

                    clientId = Console.ReadLine();

                    while (clientId.Length != 13 && clientId != "0")
                    {
                        Console.WriteLine("Neispravan unos JMBG-a. Pokušajte ponovo.");
                        clientId = Console.ReadLine();
                    }

                    if (clientId.Equals("0"))
                    {
                        Console.Clear();
                        menu();
                    }
                    userEx = db.ClientExists(clientId);
                }

                Console.WriteLine("Lista banaka kod kojih klijent ima aktivan kredit:");

                application = db.GetClientLoans(clientId);

                ArrayList banks = new ArrayList();

                for (int i = 0; i < application.Count; i++)
                {
                    if (!banks.Contains(application[i].bank))
                        banks.Add(application[i].bank);
                }


                for (int i = 0; i < banks.Count; i++)
                {
                    Console.WriteLine(String.Format("{0}. {1}", i + 1, banks[i]));
                }


                Console.WriteLine("\nUnesite redni broj banke kod koje zelite da platite ratu kredita.");


                int bankNo = Convert.ToInt32(Console.ReadLine()) - 1;

                while (bankNo + 1 > banks.Count)
                {
                    Console.WriteLine("Nije unet odgovarajući redni broj.\nUnesite ponovo");
                    bankNo = Convert.ToInt32(Console.ReadLine()) - 1;
                }

                application = db.GetClientLoans(clientId, banks[bankNo].ToString());

                if (application == null)
                {
                    Client client;
                    client = db.GetClient(clientId);

                    Console.WriteLine(String.Format("\nKlijent {0} sa JMBG-om {1} nema odobreni kredit u banci {2}.", client.name, client.idNumber, application[bankNo].bank));
                    Console.WriteLine("Za povratak u glavni meni unesite 0");
                    string trash = Console.ReadLine();
                    while (!trash.Equals("0"))
                    {
                        Console.WriteLine("Za povratak u glavni meni unesite 0");
                        trash = Console.ReadLine();
                    }
                    Console.Clear();
                    menu();
                }
                else
                {
                    Console.WriteLine(String.Format("\nKlijent {0} sa JMBG - om {1} ima sledeći kredit/kredite: ", application[0].client.name, application[0].client.idNumber));
                    for (int i = 0; i < application.Count; i++)
                    {
                        Console.WriteLine(String.Format("{0}. Kredit kod banke {1} u iznosu od {2} a preostali broj rata je {3}", (i + 1), application[i].bank, application[i].loanAmount, application[i].numberOfEMI));
                    }
                    //range pazii
                    Console.WriteLine("\nMolimo vas odaberite redni broj kredita koji zelite da platite.\n" +
                        "Za povratak u glavni meni unesite 0.");
                    string ordNum = Console.ReadLine();

                    if (ordNum.Equals("0"))
                    {
                        Console.Clear();
                        menu();
                    }
                    else
                    {
                        int ordNumInt = Convert.ToInt32(ordNum) - 1;

                        while (ordNumInt > application.Count - 1)
                        {
                            Console.WriteLine("Nije unet odgovarajuci redni broj. Molimo vas unesite ponovo.");
                            ordNumInt = Convert.ToInt32(Console.ReadLine()) - 1;
                        }


                        Console.WriteLine("Izabrali ste kredit pod rednim brojem " + ordNum +
                            "\nIznos vase rate je " + application[ordNumInt].loanAmount / application[ordNumInt].numberOfEMI);
                        Console.WriteLine("Da biste platili ratu kredita izaberite opciju\n1. Placam ratu\n2. Odbijam");

                        string acc = Console.ReadLine();
                        if (acc.Equals("1"))
                        {
                            db.PayLoan(application[ordNumInt]._id);
                            Console.WriteLine("Rata je uspesno isplacena.");

                            Console.WriteLine("Ukoliko želite da napustite aplikaciju unesite 0, za povratak u glavni meni pritisnite Enter");

                            if (Console.ReadLine() == "")
                            {
                                Console.Clear();
                                menu();
                            }


                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("Placanje je odbijeno. Uskoro ćemo vas prebaciti u glavni meni.");
                            Thread.Sleep(2000);
                            Console.Clear();
                            menu();
                        }
                    }
                }


            }
            else if (optionNo.Equals("2") || optionNo.Equals("2."))
            {
                string clientId;

                Console.WriteLine("Uspesno ste izabrali opciju 2, molimo vas unesite JMBG klijenta cije kredite zelite da vidite.");
                clientId = Console.ReadLine();

                while (clientId.Length != 13)
                {
                    Console.WriteLine("Neispravan unos JMBG-a. Pokušajte ponovo.");
                    clientId = Console.ReadLine();
                }

                bool userEx;
                userEx = db.ClientExists(clientId);

                while (!userEx)
                {
                    Console.WriteLine("Klijent nema ni u jednoj banci kredit. Molimo vas unesite novi JMBG ili unesite 0 za povratak u glavni meni");

                    while (clientId.Length != 13 && clientId != "0")
                    {
                        Console.WriteLine("Neispravan unos JMBG-a. Pokušajte ponovo.");
                        clientId = Console.ReadLine();
                    }

                    clientId = Console.ReadLine();
                    if (clientId.Equals("0"))
                    {
                        Console.Clear();
                        menu();
                    }
                    userEx = db.ClientExists(clientId);
                }

                Console.WriteLine("Unesite ime banke kod koje klijent ima kredit.");
                string bankName;
                bankName = Console.ReadLine();
                BankControl.bankEnt(clientId, bankName);

            }
            else if (optionNo.Equals("3") || optionNo.Equals("3."))
            {
                Console.Clear();
                Console.WriteLine("Da li CSV ima header?\n" +
                    "1. Ima\n" +
                    "2. Nema\n" +
                    "Odaberite opciju.");
                string header = Console.ReadLine();
                while (header != "1" && header != "2" && header != "1." && header != "2.")
                {
                    Console.WriteLine("Opcija nije uspešno odabrana. Pokušajte ponovo.");
                    header = Console.ReadLine();
                }
                Console.WriteLine("Molimo vas unesite putanju do CSV fajla.");
                //@"C:\Users\ROG\Desktop\aplikacije.csv"
                string path = Console.ReadLine();

                bool head;
                if(header == "1" || header == "1.")
                    head = true;
                else
                    head = false;

                CsvParse.readCSV(path, head);

                Console.WriteLine("\nZa ucitavanje novog CSV-a unesite novu putanju\n" +
                    "Za povratak u glavni meni unesite 0");
                path = Console.ReadLine();
                while (path != "0")
                {
                    CsvParse.readCSV(path, head);
                    path = Console.ReadLine();
                }
                Console.Clear();
                menu();



            }
            else if (optionNo.Equals("4") || optionNo.Equals("4."))
            {
                Console.Clear();
                Console.WriteLine("Molimo vas unesite sledece sledece potrebne podatke za aplikaciju kredita.\n");
                LoanApplication loan = new LoanApplication();

                Bank bank = null;
                Client client = new Client();

                Console.Write("Ime banke:\n");
                Console.WriteLine("1. RBC\n2. Santander\n3. Wells Fargo\nUnesite redni broj banke kod koje zelite da aplicirate za kredit.");
                string opt = Console.ReadLine();
                while(opt != "1" && opt != "2" && opt != "3")
                {
                    Console.WriteLine("Niste dobro odabrali banku, pokusajte ponovo. Za povratak u glavni meni unesite 0.");
                    opt = Console.ReadLine();
                    if(opt == "0")
                    {
                        Console.Clear();
                        menu();
                    }
                }
                
                if (opt == "1")
                    bank = new RBC();
                else if (opt == "2")
                    bank = new Santander();
                else 
                    bank = new WellsFargo();

                loan.bank = bank.name;


                Console.Write("Ime podnosioca aplikacije: ");
                client.name = Console.ReadLine();

                Console.Write("JMBG podnosioca aplikacije: ");
                client.idNumber = Console.ReadLine();

                while(client.idNumber.Length != 13)
                {
                    Console.WriteLine("JMBG mora imati 13 karaktera. Pokusajte ponovo.");
                    client.idNumber = Console.ReadLine();
                }

                Console.Write("Mesećna primanja podnosioca: ");
                client.monthlyIncome = Convert.ToDouble(Console.ReadLine());

                Console.Write("Godine radnog staža podnosioca: ");
                client.yearsExp = Convert.ToInt32(Console.ReadLine());

                loan.client = client;

                Console.Write("Iznos kredita: ");
                loan.loanAmount = Convert.ToDouble(Console.ReadLine());

                Console.Write("Broj rata: ");
                loan.numberOfEMI = Convert.ToInt32(Console.ReadLine());

                Console.WriteLine();
                BankControl.ApplyingForLoan(loan);
                
                Console.WriteLine("Po zavrsetku obrade zahteva, prebacicemo vas u glavni meni.");
                Thread.Sleep(8000);
                Console.Clear();
                menu();
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Nispravan unos.\nMolimo vas unesite ponovo opciju.\n");

                menu();
            }


        }
    }
}
