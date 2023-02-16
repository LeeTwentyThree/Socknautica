using System.Reflection;
using System.Text;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Press enter to continue.");
Console.ReadLine();

var directory = Directory.GetCurrentDirectory();
var textFile = Path.Combine(directory, "input.txt");
var outputFile = Path.Combine(directory, "output.txt");
var lines = File.ReadAllText(textFile).Split("\n");

StringBuilder output = new StringBuilder();

foreach (var line in lines)
{
    string newLine = line.Replace("new SpawnInfo(", "yield return SpawnPrefabGlobally(")
                        .Replace(", new Vector3(", ", new Vector3(")
                        .Replace("));", "), Vector3.one);");
    Console.WriteLine(newLine);
    output.AppendLine(newLine);
}

File.WriteAllText(outputFile, output.ToString());
Console.WriteLine("Saved!");
Console.WriteLine("Press enter to close.");
Console.ReadLine();