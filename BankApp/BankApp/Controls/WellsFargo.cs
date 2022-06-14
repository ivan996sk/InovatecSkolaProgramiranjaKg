using BankApp.Data;
using BankApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Control
{
    public class WellsFargo : Bank
    {
        public WellsFargo()
        {
            name = "Wells Fargo";
        }

        public override bool checkTerms(LoanApplication application)
        {
            DataAccess db = new DataAccess();

            if (application.client.monthlyIncome > (application.loanAmount / application.numberOfEMI))
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
