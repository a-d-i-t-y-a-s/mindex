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
    // Compensation repository to work with the service
    public class CompensationRepository : ICompensationRepository
    {
        private readonly JsonDataContext _dataContext;
        private readonly ILogger<ICompensationRepository> _logger;

        public CompensationRepository(
            ILogger<ICompensationRepository> logger,
            JsonDataContext dataContext)
        {
            _logger = logger;
            _dataContext = dataContext;
        }

        public Compensation Create(Compensation compensation)
        {

            if (_dataContext.Compensations.Any(c => c.EmployeeId == compensation.EmployeeId))
            {
                throw new InvalidOperationException($"A compensation record already exists for EmployeeId: {compensation.EmployeeId}");
            }

            // Generate a new CompensationId if needed
            if (compensation.CompensationId == 0)
            {
                compensation.CompensationId = _dataContext.Compensations.Any()
                    ? _dataContext.Compensations.Max(c => c.CompensationId) + 1
                    : 1;
            }

            _dataContext.Compensations.Add(compensation);
            return compensation;
        }

        public Compensation GetByEmployeeId(string employeeId)
        {
            return _dataContext.Compensations
                .FirstOrDefault(c => c.EmployeeId == employeeId);
        }

        public Task SaveAsync()
        {
            _dataContext.SaveChanges();
            return Task.CompletedTask;
        }
    }
}