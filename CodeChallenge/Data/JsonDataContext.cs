using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using CodeChallenge.Models;

namespace CodeChallenge.Data
{
    //In order to achieve persistence, I decided to store the compensation data in a json file
    public class JsonDataContext
    {
        private readonly string _filePath;

        public List<Employee> Employees { get; set; } = new List<Employee>();
        public List<Compensation> Compensations { get; set; } = new List<Compensation>();

        public JsonDataContext(string filePath)
        {
            _filePath = filePath;
            LoadData();
        }

        private void LoadData()
        {
            if (!File.Exists(_filePath))
            {
                Employees = new List<Employee>();
                Compensations = new List<Compensation>();
                return;
            }

            var json = File.ReadAllText(_filePath);
            var dataWrapper = JsonSerializer.Deserialize<JsonDataWrapper>(json);

            if (dataWrapper != null)
            {
                Employees = dataWrapper.Employees ?? new List<Employee>();
                Compensations = dataWrapper.Compensations ?? new List<Compensation>();
            }
        }

        public void SaveChanges()
        {
            var dataWrapper = new JsonDataWrapper
            {
                Employees = Employees,
                Compensations = Compensations
            };

            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(dataWrapper, options);
            File.WriteAllText(_filePath, json);
        }

        private class JsonDataWrapper
        {
            public List<Employee> Employees { get; set; }
            public List<Compensation> Compensations { get; set; }
        }
    }
}
