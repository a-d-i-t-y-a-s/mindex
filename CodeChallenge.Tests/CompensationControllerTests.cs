using System.Net;
using System.Net.Http;
using System.Text;

using CodeChallenge.Models;

using CodeCodeChallenge.Tests.Integration.Extensions;
using CodeCodeChallenge.Tests.Integration.Helpers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeCodeChallenge.Tests.Integration
{
	[TestClass]
	public class CompensationControllerTests
	{
		private static HttpClient _httpClient;
		private static TestServer _testServer;

		[ClassInitialize]
		// Attribute ClassInitialize requires this signature
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
		public static void InitializeClass(TestContext context)
		{
			_testServer = new TestServer();
			_httpClient = _testServer.NewClient();
		}

		[ClassCleanup]
		public static void CleanUpTest()
		{
			_httpClient.Dispose();
			_testServer.Dispose();
		}

		[TestMethod]
		public void CreateCompensation_Returns_Created()
		{
			// Arrange
			var compensation = new Compensation()
			{
				EmployeeId = "b7839309-3348-463b-a7e3-5de1c168beb3",
				Salary = 120000,
				EffectiveDate = System.DateTime.UtcNow
			};

			var requestContent = new JsonSerialization().ToJson(compensation);

			// Execute
			var postRequestTask = _httpClient.PostAsync("api/compensation",
				new StringContent(requestContent, Encoding.UTF8, "application/json"));
			var response = postRequestTask.Result;

			// Assert
			Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

			var newCompensation = response.DeserializeContent<Compensation>();
			Assert.IsNotNull(newCompensation.CompensationId);
			Assert.AreEqual(compensation.EmployeeId, newCompensation.EmployeeId);
			Assert.AreEqual(compensation.Salary, newCompensation.Salary);
			Assert.AreEqual(compensation.EffectiveDate, newCompensation.EffectiveDate);
		}

		[TestMethod]
		public void GetCompensationByEmployeeId_Returns_Ok()
		{
			// Arrange
			var employeeId = "b7839309-3348-463b-a7e3-5de1c168beb3";

			// Execute
			var getRequestTask = _httpClient.GetAsync($"api/compensation/{employeeId}");
			var response = getRequestTask.Result;

			// Assert
			Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

			var compensation = response.DeserializeContent<Compensation>();
			Assert.AreEqual(employeeId, compensation.EmployeeId);
		}

		[TestMethod]
		public void GetCompensationByEmployeeId_Returns_NotFound()
		{
			// Arrange
			var invalidEmployeeId = "Invalid_Id";

			// Execute
			var getRequestTask = _httpClient.GetAsync($"api/compensation/{invalidEmployeeId}");
			var response = getRequestTask.Result;

			// Assert
			Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
		}
	}
}
