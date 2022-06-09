using System.Runtime.InteropServices;

[DllImport("lib.dll")]
static extern int Sum(int a, int b);

Console.WriteLine(Sum(1, 2));