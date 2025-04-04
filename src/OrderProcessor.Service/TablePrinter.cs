﻿using OrderProcessor.Service.DTO;
using System.Reflection;

namespace OrderProcessor.Service
{
    public class TablePrinter
    {
        private readonly List<string> headers = new();
        private readonly List<List<string>> rows = new();

        #region Public Methods
        public static TablePrinter CreateTable(IEnumerable<OrderData> orders)
        {
            var printer = new TablePrinter();
            var classType = typeof(OrderData);

            // Add columns
            foreach (PropertyInfo propertyInfo in classType.GetProperties())
            {
                if (propertyInfo.Name != "CreationTime" && propertyInfo.Name != "CustomerType" && propertyInfo.Name != "CustomerName")
                {
                    printer.AddColumn(propertyInfo.Name);
                }
            }
            // Add rows
            foreach (var order in orders)
            {
                printer.AddRow(
                    order.Id,
                    order.Value,
                    order.ProductName,
                    order.ShippingAddress,
                    order.Quantity,
                    order.Status,
                    order.PaymentMethod
                );
            }

            return printer;
        }

        public void PrintTable()
        {
            if (headers.Count == 0)
            {
                Console.WriteLine("No columns defined.");
                return;
            }

            // Calculate the max width for each column
            var widths = new int[headers.Count];
            for (int i = 0; i < headers.Count; i++)
            {
                widths[i] = Math.Max(widths[i], headers[i].Length);
            }

            foreach (var row in rows)
            {
                for (int i = 0; i < row.Count; i++)
                {
                    widths[i] = Math.Max(widths[i], row[i].Length);
                }
            }

            // Print header row
            PrintRow(headers, widths, isHeader: true);

            // Print all data rows
            foreach (var row in rows)
            {
                PrintRow(row, widths, isHeader: false);
            }
        }
        #endregion

        #region Private Methods
        private static void PrintRow(List<string> rowData, int[] widths, bool isHeader)
        {
            var row = "| ";
            for (int i = 0; i < rowData.Count; i++)
            {
                row += rowData[i].PadRight(widths[i]) + " | ";
            }
            Console.WriteLine(row);

            // Underline header row
            if (isHeader)
            {
                Console.WriteLine(new string('-', row.Length));
            }
        }

        private void AddColumn(string header)
        {
            headers.Add(header);
        }

        private void AddRow(params object[] columns)
        {
            var row = columns.Select(col => col?.ToString() ?? string.Empty).ToList();
            rows.Add(row);
        }

        #endregion

    }
}
