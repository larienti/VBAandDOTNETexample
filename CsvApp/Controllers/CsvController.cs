using Microsoft.AspNetCore.Mvc;
using CsvApp.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace CsvApp.Controllers
{
    public class CsvController : Controller
    {
        // Store uploaded CSV data in memory (for simplicity)
        private static List<Chemical> _csvData = new();

        // Display the uploaded data on the Index page
        public IActionResult Index()
        {
            return View(_csvData); // Pass the data to the view
        }

        // Handle CSV uploads
        [HttpPost]
        public IActionResult UploadCsv(IFormFile csvFile)
        {
            if (csvFile == null || csvFile.Length == 0)
            {
                ModelState.AddModelError("File", "Please upload a valid CSV file.");
                return RedirectToAction("Index");
            }

            try
            {
                using (var stream = new StreamReader(csvFile.OpenReadStream()))
                {
                    var csvData = new List<Chemical>();
                    string? line;
                    int lineNumber = 0;

                    // Read the file line by line
                    while ((line = stream.ReadLine()) != null)
                    {
                        if (lineNumber++ == 0) continue; // Skip the header row

                        var columns = line.Split(',');

                        // Validate row length
                        if (columns.Length < 6)
                        {
                            ModelState.AddModelError("File", $"Invalid CSV format: Row {lineNumber} has fewer than 6 columns.");
                            return RedirectToAction("Index");
                        }

                        // Validate and parse the columns
                        if (!bool.TryParse(columns[1], out var natural))
                        {
                            ModelState.AddModelError("File", $"Invalid data in row {lineNumber}: 'Natural' must be true or false.");
                            return RedirectToAction("Index");
                        }

                        if (!long.TryParse(columns[3], out var parts))
                        {
                            ModelState.AddModelError("File", $"Invalid data in row {lineNumber}: 'Parts' must be a whole number.");
                            return RedirectToAction("Index");
                        }

                        if (!double.TryParse(columns[4], out var cost))
                        {
                            ModelState.AddModelError("File", $"Invalid data in row {lineNumber}: 'Cost' must be a number.");
                            return RedirectToAction("Index");
                        }

                        if (!DateTime.TryParse(columns[5], out var latestDate))
                        {
                            ModelState.AddModelError("File", $"Invalid data in row {lineNumber}: 'Latest Date' must be a valid date.");
                            return RedirectToAction("Index");
                        }

                        // Add the row to the data list
                        csvData.Add(new Chemical
                        {
                            Code = columns[0],
                            Natural = natural,
                            CAS = columns[2],
                            Parts = parts,
                            Cost = cost,
                            LatestDate = latestDate
                        });
                    }

                    // Update the in-memory data store
                    _csvData = csvData;
                }
            }
            catch (Exception ex)
            {
                // Log the exception and show a generic error message
                ModelState.AddModelError("File", $"An error occurred while processing the file: {ex.Message}");
                return RedirectToAction("Index");
            }

            // Redirect to the Index view to display the uploaded data
            return RedirectToAction("Index");
        }

        // Determine over 80% natural and formula cost
        public IActionResult RunCommand()
        {
            if (_csvData == null || !_csvData.Any())
            {
                ViewBag.Message = "No data available to process. Please upload a CSV file.";
                return View("Index", _csvData); // Return the Index view with the existing model
            }

            // Calculate total cost
            double totalCost = _csvData.Sum(item => item.Cost);

            // Calculate percentage of parts that are natural
            long totalParts = _csvData.Sum(item => item.Parts);
            long naturalParts = _csvData.Where(item => item.Natural).Sum(item => item.Parts);
            double naturalPercentage = totalParts > 0 ? (double)naturalParts / totalParts * 100 : 0;

            // Store results in ViewBag
            ViewBag.TotalCost = totalCost;
            ViewBag.TotalParts = totalParts;
            ViewBag.IsAbove80PercentNatural = naturalPercentage > 80;
            ViewBag.NaturalPercentage = naturalPercentage;

            // Success message
            ViewBag.Message = "Command executed successfully!";

            return View("Index", _csvData); // Return the Index view with the processed model
        }
    }
}