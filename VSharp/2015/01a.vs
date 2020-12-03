// Solved with 3802a56dd69c55cba9aadf988b9c9587f26a4f3f
with Sys;
with Sys.IO;

Console.Write("Enter path to input: ");
var path = Console.ReadLine();
var input = File.ReadAllText(path);
if (input == "")
{
    Console.WriteLine("Could not read input.");
    return;
}

var result = 0;
for (var i = 0; i < String.Length(input); i++)
{
    var c = input |> String.CharAt(i);
    if (c == '(')
        result++;
    else
        result--;
}

Console.WriteLine("The result is " + result);