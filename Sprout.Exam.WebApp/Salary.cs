using Microsoft.EntityFrameworkCore.Storage;
using System;

namespace Sprout.Exam.WebApp
{
    public class Salary
    {
        public decimal AbsentDays { get; set; }
        public decimal WorkedDays { get; set; }

        public interface Calculation
        {
            decimal Compute(decimal days);
        }

        public class Regular : Calculation
        {
            const decimal REGULAR_SALARY = 20000;
            const decimal TAX_DEDUCTION = 0.12M;
            const decimal WORKING_DAYS = 22;
            public decimal Compute(decimal absentDays)
            {
                return Math.Round(REGULAR_SALARY - (absentDays * (REGULAR_SALARY / WORKING_DAYS)) - (REGULAR_SALARY * TAX_DEDUCTION), 2);
            }
        }
        public class Contractual : Calculation
        {
            const decimal CONTRACTUAL_SALARY = 500;
            public decimal Compute(decimal workedDays)
            {
                return Math.Round(CONTRACTUAL_SALARY * workedDays, 2);
            }
        }
    }


}
