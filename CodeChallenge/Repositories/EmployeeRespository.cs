using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using CodeChallenge.Data;

namespace CodeChallenge.Repositories
{
    public class EmployeeRespository : IEmployeeRepository
    {
        private readonly EmployeeContext _employeeContext;
        private readonly ILogger<IEmployeeRepository> _logger;

        public EmployeeRespository(ILogger<IEmployeeRepository> logger, EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
            _logger = logger;
        }

        public Employee Add(Employee employee)
        {
            employee.EmployeeId = Guid.NewGuid().ToString();
            _employeeContext.Employees.Add(employee);
            return employee;
        }

        public Employee GetById(string id)
        {
            return _employeeContext.Employees.SingleOrDefault(e => e.EmployeeId == id);
        }

        public Task SaveAsync()
        {
            return _employeeContext.SaveChangesAsync();
        }

        public Employee Remove(Employee employee)
        {
            return _employeeContext.Remove(employee).Entity;
        }

    
        public int GetNumberOfDirectReports(string employeeId)
        {
            var employee = _employeeContext.Employees
                .Include(e => e.DirectReports)
                .SingleOrDefault(e => e.EmployeeId == employeeId);

            if (employee == null)
            {
                return 0;
            }

            return CountAllReports(employee);
        }

        // -----------------------------------
        // Helper Method: Recursively traverses the hierarchy
        // -----------------------------------
        private int CountAllReports(Employee employee)
        {
            if (employee.DirectReports == null || !employee.DirectReports.Any())
            {
                return 0;
            }

            int total = 0;

            // For each direct report, load that object fully from the DB (so we can see *their* DirectReports)
            foreach (var dr in employee.DirectReports)
            {
                var fullyLoadedDr = _employeeContext.Employees
                    .Include(e => e.DirectReports)
                    .SingleOrDefault(e => e.EmployeeId == dr.EmployeeId);

                // 1 for this direct report + however many reports *they* have
                total += 1 + CountAllReports(fullyLoadedDr);
            }

            return total;
        }



    }
}
