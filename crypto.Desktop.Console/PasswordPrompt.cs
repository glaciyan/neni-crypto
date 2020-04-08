using System;
using System.Collections.Generic;

namespace crypto.Desktop.Cnsl
{
    public static class PasswordPrompt
    {
        public static string PromptPassword()
        {
            Console.Write("Enter Password: ");

            var password = new List<char>();
            ConsoleKeyInfo pressedKey;
            while ((pressedKey = Console.ReadKey(true)).Key != ConsoleKey.Enter)
                if (pressedKey.Key == ConsoleKey.Backspace)
                {
                    if (password.Count != 0) password.RemoveAt(password.Count - 1);
                }
                else
                {
                    password.Add(pressedKey.KeyChar);
                }

            Console.Write('\n');
            return new string(password.ToArray());
        }

        public static string? PromptPasswordWithConfirmation()
        {
            var pw = PromptPassword();
            Console.Write(@"(Confirm Password: ) ");
            var pwRe = PromptPassword();

            return pw != pwRe ? null : pw;
        }
    }
}