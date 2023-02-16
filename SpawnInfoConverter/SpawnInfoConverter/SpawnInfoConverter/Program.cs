using System.Reflection;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Press enter to continue.");
Console.ReadLine();

var directory = Directory.GetCurrentDirectory();
var textFile = Path.Combine(directory, "input.txt");
var lines = textFile.Split("\n");

foreach (var line in lines)
{
    string newLine = line.Replace("new SpawnInfo(", "yield return SpawnPrefabGlobally(")
                        .Replace(", new Vector3(", ", new Vector3(")
                        .Replace("));", "), Vector3.one);");
    Console.WriteLine(newLine);
}

Console.WriteLine("Press enter to close.");
Console.ReadLine();