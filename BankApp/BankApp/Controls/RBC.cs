using BankApp.Data;
using BankApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Control
{
    public class RBC : Bank
    {
        public RBC()
        {
            name = "RBC";
        }

        public override bool checkTerms(LoanApplication application)
        {
            DataAccess db = new DataAccess();

            if (!(application.client.monthlyIncome < 2 * application.loanAmount / application.numberOfEMI) && !db.GetClientLoan(application.client.idNumber, "RBC"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        
    }
}
