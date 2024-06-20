namespace WebApplication3.Models
{
    public class AnalysisModel
    {
        public class EmployeeEntry
        {
            public string ?Department { get; set; }
            public int EntryCount { get; set; }
            public int Month { get; set; }
            public int Year { get; set; }
        }

        public class EmployeeExit
        {
            public string ?Department { get; set; }
            public int ExitCount { get; set; }
            public int Month { get; set; }
            public int Year { get; set; }
        }

        public class EmployeeSalary
        {
            public string ?Department { get; set; }
            public decimal AverageSalary { get; set; }
        }
    }
}
