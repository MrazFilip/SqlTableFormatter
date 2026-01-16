using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

class Program
{
    static void Main()
    {
        Console.WriteLine("Paste your data below. When done, press ENTER on an empty line:\n");

        var input = new StringBuilder();
        string line;

        while (true)
        {
            line = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
                break;

            input.AppendLine(line);
        }

        if (input.Length == 0)
        {
            Console.WriteLine("No data entered.");
            return;
        }

        var lines = input.ToString()
            .Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

        var table = lines.Select(l => l.Split('\t')).ToList();

        var html = new StringBuilder();

        html.AppendLine("<!DOCTYPE html>");
        html.AppendLine("<html><head><meta charset='utf-8'>");
        html.AppendLine("<title>Email Table</title>");
        html.AppendLine(@"
<style>
body { font-family: Arial; padding:20px; }
table { border-collapse: collapse; font-size: 12px; }
th, td { border: 1px solid #888; padding: 6px 10px; }
th { background: #f0f0f0; }
</style>
</head><body>");

        html.AppendLine("<h3>Rendered Data Table</h3>");
        html.AppendLine("<table>");

        // Header
        html.AppendLine("<tr>");
        foreach (var col in table[0])
            html.AppendLine($"<th>{System.Net.WebUtility.HtmlEncode(col)}</th>");
        html.AppendLine("</tr>");

        // Rows
        foreach (var row in table.Skip(1))
        {
            html.AppendLine("<tr>");
            foreach (var cell in row)
                html.AppendLine($"<td>{System.Net.WebUtility.HtmlEncode(cell)}</td>");
            html.AppendLine("</tr>");
        }

        html.AppendLine("</table></body></html>");

        var filePath = Path.Combine(Path.GetTempPath(), "EmailTable.html");
        File.WriteAllText(filePath, html.ToString(), Encoding.UTF8);

        Process.Start(new ProcessStartInfo
        {
            FileName = filePath,
            UseShellExecute = true
        });
    }
}
