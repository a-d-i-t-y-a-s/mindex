using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using CodeChallenge.Repositories;

namespace CodeChallenge.Services
{
    // I created a service in order to process the compensation for task #2
    public class CompensationService : ICompensationService
    {
        private readonly ICompensationRepository _compRepository;
        private readonly ILogger<CompensationService> _logger;

        public CompensationService(
            ICompensationRepository compRepository,
            ILogger<CompensationService> logger)
        {
            _compRepository = compRepository;
            _logger = logger;
        }

        public Compensation Create(Compensation compensation)
        {
            _logger.LogDebug("Creating a new compensation record...");

            // Potential business logic or validations here:
            // e.g. check if salary is >= 0, check if EffectiveDate is valid, etc.

            var created = _compRepository.Create(compensation);
            _compRepository.SaveAsync().Wait();
            return created;
        }

        public Compensation GetByEmployeeId(string employeeId)
        {
            _logger.LogDebug($"Getting compensation by employeeId = {employeeId}");

            // Additional logic or transformations could be placed here.
            return _compRepository.GetByEmployeeId(employeeId);
        }

        public Task SaveAsync()
        {
            // The repository actually does the persistence
            return _compRepository.SaveAsync();
        }
    }
}