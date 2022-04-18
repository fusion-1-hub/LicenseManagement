using System.Runtime.InteropServices;

[DllImport("resman2.dll")]
static extern IntPtr GenerateUID([MarshalAs(UnmanagedType.LPStr)] string appName);

IntPtr intptr = GenerateUID("test");
var result = Marshal.PtrToStringAnsi(intptr);
Console.WriteLine(result);

Console.WriteLine("\npress any key to exit the process...");
Console.ReadKey();
