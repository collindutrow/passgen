using System;

namespace PassGen
{
    internal static class Characters
    {
        public const string AZCharList = "abcdefghijklmnopqrstuvwxyz";
        public const string NumCharList = "0123456789";
        public const string SpecialCharList = "`~!@#$%^&*()-_=+[{]}\\|;:\'\",<.>/?";
        public const string AmbiguousCharList = "1LlioO0";

        internal enum CharType
        {
            None = 0,
            AZUpper = 1,
            AZLower = 2,
            Number = 3,
            Special = 4
        }

        internal static CharType IdentifyCharType(char c)
        {
            string character = c.ToString();
            Characters.CharType type = Characters.CharType.None;

            if (Characters.AZCharList.ToUpper().Contains(character))
            {
                type = Characters.CharType.AZUpper;
            }
            else if (Characters.AZCharList.ToLower().Contains(character))
            {
                type = Characters.CharType.AZLower;
            }
            else if (Characters.NumCharList.Contains(character))
            {
                type = Characters.CharType.Number;
            }
            else if (Characters.SpecialCharList.Contains(character))
            {
                type = Characters.CharType.Special;
            }

            return type;
        }

        internal static char GetRandomChar(Characters.CharType returnType)
        {
            string charList = string.Empty;

            switch (returnType)
            {
                case Characters.CharType.None:
                    charList += Characters.AZCharList.ToUpper();
                    charList += Characters.AZCharList.ToLower();
                    charList += Characters.NumCharList;
                    charList += Characters.SpecialCharList;
                    break;

                case Characters.CharType.AZUpper:
                    charList += Characters.AZCharList.ToUpper();
                    break;

                case Characters.CharType.AZLower:
                    charList += Characters.AZCharList.ToLower();
                    break;

                case Characters.CharType.Number:
                    charList += Characters.NumCharList;
                    break;

                case Characters.CharType.Special:
                    charList += Characters.SpecialCharList;
                    break;
            }

            int i = new Random().Next(0, charList.Length);
            return charList[i];
        }
    }
}