using System;
using System.Collections.Generic;
using crypto.Core.Extension;
using crypto.Desktop.Cnsl.Recources;

namespace crypto.Desktop.Cnsl
{
    public static class PasswordPrompt
    {
        public static string? ArgumentPw { get; set; }
        
        public static string PromptPassword(string? promptMessage = null, bool useArgument = true)
        {
            if (ArgumentPw != null && useArgument) return ArgumentPw;
            
            Console.Write(promptMessage ?? Strings.PasswordPrompt_PromptPassword_Enter_Password__);
            
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

        public static byte[] PromptPasswordAsHash(string? promptMessage = null) =>
            PromptPassword(promptMessage).Hash();

        public static string PromptPasswordWithConfirmation()
        {
            var pw = PromptPassword(Strings.PasswordPrompt_PromptPassword_Enter_Password__, false);
            var pwRe = PromptPassword(Strings.PasswordPrompt_PromptPasswordWithConfirmation_Confirm_Password__, false);

            if (pw != pwRe)
                throw new PasswordException(Strings.PasswordPrompt_PromptPasswordWithConfirmation_Password_didn_t_match_up);

            return pw;
        }

        public static byte[] PromptPasswordWithConfirmationAsHash() =>
            PromptPasswordWithConfirmation().Hash();

        private static byte[] Hash(this string s) => s.ApplySHA256();
    }
}