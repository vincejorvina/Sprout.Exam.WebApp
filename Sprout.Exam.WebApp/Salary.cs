using Microsoft.EntityFrameworkCore.Storage;
using System;

namespace Sprout.Exam.WebApp
{
    public class Salary
    {
        private const decimal REGULAR_SALARY = 20000;
        private const decimal CONTRACTUAL_SALARY = 500;
        private const decimal TAX_DEDUCTION = 0.12M;
        public decimal AbsentDays { get; set; }
        public decimal WorkedDays { get; set; }

        public interface Calculation
        {
            decimal Compute(decimal days);
        }

        public class Regular : Calculation
        {
            public decimal Compute(decimal absentDays)
            {
                var result = REGULAR_SALARY - (absentDays * (REGULAR_SALARY / 22)) - (REGULAR_SALARY * TAX_DEDUCTION);
                return Math.Round(result, 2);
            }
        }
        public class  Contractual : Calculation
        {
            public decimal Compute(decimal workedDays)
            {
                var result = CONTRACTUAL_SALARY * workedDays;
                return Math.Round(result, 2);
            }
        }
    }


}
