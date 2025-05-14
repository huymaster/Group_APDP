namespace APDP;

public static class Functions
{
    public static T ReadInput<T>(
        string message,
        Func<string, T> transformer,
        Func<T, bool>? validator = null
    )
    {
        var nNValidator = validator ?? (_ => true);
        while (true)
        {
            Console.Write($"{message}: ");
            try
            {
                var result = transformer(Console.ReadLine() ?? "");
                if (nNValidator(result))
                    return result;
                Console.WriteLine("Invalid input.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed to parse input: {e.Message}.");
            }
        }
    }

    public static void ReadInput<T>(
        string message,
        Func<string, T> transformer,
        ref T writeTo,
        Func<T, bool>? validator = null
    )
    {
        writeTo = ReadInput(message, transformer, validator);
    }
}