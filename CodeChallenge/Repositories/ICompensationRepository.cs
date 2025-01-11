using CodeChallenge.Models;
using System;
using System.Threading.Tasks;

namespace CodeChallenge.Repositories
{

	public interface ICompensationRepository
	{
		Compensation Create(Compensation compensation);
		Compensation GetByEmployeeId(string employeeId);
		Task SaveAsync();
	}
}
