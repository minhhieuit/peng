using System.Security.Cryptography;

namespace Peng.Modules.Members.Application;

/// <summary>
/// Generates a cryptographically random temporary password for admin-created members.
/// Guarantees at least one lower, upper and digit so it satisfies typical password rules.
/// </summary>
public static class TemporaryPasswordGenerator
{
    private const string Lower = "abcdefghijkmnpqrstuvwxyz";
    private const string Upper = "ABCDEFGHJKLMNPQRSTUVWXYZ";
    private const string Digits = "23456789";
    private const string All = Lower + Upper + Digits;

    public static string Generate(int length = 12)
    {
        if (length < 4) length = 4;

        var chars = new char[length];
        chars[0] = Lower[RandomNumberGenerator.GetInt32(Lower.Length)];
        chars[1] = Upper[RandomNumberGenerator.GetInt32(Upper.Length)];
        chars[2] = Digits[RandomNumberGenerator.GetInt32(Digits.Length)];
        for (var i = 3; i < length; i++)
            chars[i] = All[RandomNumberGenerator.GetInt32(All.Length)];

        // Fisher–Yates shuffle so the guaranteed chars are not always in front.
        for (var i = chars.Length - 1; i > 0; i--)
        {
            var j = RandomNumberGenerator.GetInt32(i + 1);
            (chars[i], chars[j]) = (chars[j], chars[i]);
        }

        return new string(chars);
    }
}
