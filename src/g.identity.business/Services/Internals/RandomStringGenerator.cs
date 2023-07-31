using System.Security.Cryptography;
using System.Text;

namespace g.identity.business.Services.Internals;

internal class RandomStringGenerator : IRandomStringGenerator
{
    private const int DefaultLength = 8;

    public string Generate(int length = 8, bool complex = false)
    {
        if (length <= 0)
            length = DefaultLength;

        var result = string.Empty;
        do
        {
            result += GetStringPart(length - result.Length, complex);
        } while (result.Length != length);

        return result;
    }

    private static string GetStringPart(int length, bool complex)
    {
        var bytes = new byte[255];
        Func<byte, bool> expr = complex
            ? e => e is >= (byte)'0' and <= (byte)'9' or >= (byte)'@' and <= (byte)'z'
            : e => e is >= (byte)'0' and <= (byte)'9' or >= (byte)'A' and <= (byte)'Z' or >= (byte)'a' and <= (byte)'z';
        using var random = RandomNumberGenerator.Create();
        random.GetBytes(bytes, 0, 255);
        var chars = bytes.Where(expr).Take(length).ToArray();
        return new string(Encoding.ASCII.GetChars(chars));
    }
}