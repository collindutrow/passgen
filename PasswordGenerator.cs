using System;
using System.Collections.Generic;

namespace PassGen
{
    public class PasswordGenerator
    {
        public bool avoidAmbiguousChars = true;
        public bool azLower = true;
        public bool azUpper = true;
        public int length = 16;
        public int minAmmountOfNumbers = 1;
        public bool numbers = true;
        public bool requireEveryCharType = true;
        public bool special = true;
        private readonly string azCharList = Characters.AZCharList;
        private readonly string numCharList = Characters.NumCharList;
        private readonly string specialCharList = Characters.SpecialCharList;
        private string previousPassword = string.Empty;

        // Basic concept of the Password getter: infinitely loop through an enum that defines character types until
        // the length of the string matches the length property. Loop through the resulting string for
        // character requirements.
        public string Password
        {
            get
            {
                string password = string.Empty;
                int azUpperCount = 0;
                int azLowerCount = 0;
                int numberCount = 0;
                int specialCount = 0;

                Random r = new Random();
                for (int i = 0; i < length; i++)
                {
                    while (true)
                    {
                        Characters.CharType t = (Characters.CharType)r.Next(1, 5);
                        string charList = string.Empty;
                        switch (t)
                        {
                            case Characters.CharType.AZUpper:
                                if (!azUpper)
                                {
                                    continue;
                                }
                                charList = azCharList.ToUpper();
                                azUpperCount++;
                                break;

                            case Characters.CharType.AZLower:
                                if (!azLower)
                                {
                                    continue;
                                }
                                charList = azCharList.ToLower();
                                azLowerCount++;
                                break;

                            case Characters.CharType.Number:
                                if (!numbers)
                                {
                                    continue;
                                }
                                charList = numCharList;
                                numberCount++;
                                break;

                            case Characters.CharType.Special:
                                if (!special)
                                {
                                    continue;
                                }
                                charList = specialCharList;
                                specialCount++;
                                break;
                        }

                        int index = r.Next(0, charList.Length);
                        password += charList[index];
                        break;
                    }
                }

                if (requireEveryCharType)
                {
                    int requirementScore = 0;
                    int currentScore = 0;

                    if (azUpper && (azUpperCount == 0))
                    {
                        requirementScore++;
                    }

                    if (azLower && (azLowerCount == 0))
                    {
                        requirementScore++;
                    }

                    if (numbers && (numberCount == 0))
                    {
                        requirementScore++;
                    }

                    if (special && (specialCount == 0))
                    {
                        requirementScore++;
                    }

                    while (true)
                    {
                        if (requirementScore == currentScore)
                        {
                            break;
                        }

                        Characters.CharType t = (Characters.CharType)r.Next(1, 5);
                        switch (t)
                        {
                            case Characters.CharType.AZUpper:
                                if (azUpper && (azUpperCount == 0))
                                {
                                    password = RandomReplaceChar(Characters.CharType.AZUpper, password);
                                    azUpperCount++;
                                    currentScore++;
                                }
                                else
                                {
                                    continue;
                                }
                                break;

                            case Characters.CharType.AZLower:
                                if (azLower && (azLowerCount == 0))
                                {
                                    password = RandomReplaceChar(Characters.CharType.AZLower, password);
                                    azLowerCount++;
                                    currentScore++;
                                }
                                else
                                {
                                    continue;
                                }
                                break;

                            case Characters.CharType.Number:
                                if (numbers && (numberCount == 0))
                                {
                                    password = RandomReplaceChar(Characters.CharType.Number, password);
                                    numberCount++;
                                    currentScore++;
                                }
                                else
                                {
                                    continue;
                                }
                                break;

                            case Characters.CharType.Special:
                                if (special && (specialCount == 0))
                                {
                                    password = RandomReplaceChar(Characters.CharType.Special, password);
                                    specialCount++;
                                    currentScore++;
                                }
                                else
                                {
                                    continue;
                                }
                                break;
                        }
                    }
                }

                if (numbers)
                {
                    if (numberCount < minAmmountOfNumbers)
                    {
                        password = NumericFill(password, minAmmountOfNumbers);
                    }
                }

                if (password == previousPassword)
                {
                    password = Password;
                }
                else
                {
                    previousPassword = password;
                }

                if (avoidAmbiguousChars)
                {
                    password = ReplaceAmbiguousCharacters(password);
                }

                return password;
            }
        }

        /// <summary>
        /// Fills up the password with numbers by replacing random non-numeric characters until minNumCount is met.
        /// </summary>
        private string NumericFill(string s, int minNumCount)
        {
            int index = 0;
            List<int> azLowerIndexList = new List<int>();
            List<int> azUpperIndexList = new List<int>();
            List<int> numIndexList = new List<int>();
            List<int> specialIndexList = new List<int>();

            foreach (char c in s)
            {
                if (azCharList.ToUpper().Contains(c.ToString()))
                {
                    azUpperIndexList.Add(index);
                }

                if (azCharList.ToLower().Contains(c.ToString()))
                {
                    azLowerIndexList.Add(index);
                }

                if (numCharList.Contains(c.ToString()))
                {
                    numIndexList.Add(index);
                }

                if (specialCharList.Contains(c.ToString()))
                {
                    specialIndexList.Add(index);
                }

                index++;
            }

            int randomType = -100;
            int prevType = -100;

            Random r = new Random();
            while (true)
            {
                if (numIndexList.Count >= minNumCount)
                {
                    break;
                }

                do
                {
                    randomType = r.Next(0, 3);
                } while (randomType == prevType);

                prevType = randomType;
                string newNum = numCharList[r.Next(0, numCharList.Length)].ToString();

                switch (randomType)
                {
                    case 0:
                        if (azUpperIndexList.Count > 1)
                        {
                            int ranIndex = r.Next(0, azUpperIndexList.Count);
                            int oldIndex = azUpperIndexList[ranIndex];
                            s = s.Remove(oldIndex, 1).Insert(oldIndex, newNum);
                            azUpperIndexList.RemoveAt(ranIndex);
                            numIndexList.Add(oldIndex);
                        }
                        break;

                    case 1:
                        if (azLowerIndexList.Count > 1)
                        {
                            int ranIndex = r.Next(0, azLowerIndexList.Count);
                            int oldIndex = azLowerIndexList[ranIndex];
                            s = s.Remove(oldIndex, 1).Insert(oldIndex, newNum);
                            azLowerIndexList.RemoveAt(ranIndex);
                            numIndexList.Add(oldIndex);
                        }
                        break;

                    case 2:
                        if (specialIndexList.Count > 1)
                        {
                            int ranIndex = r.Next(0, specialIndexList.Count);
                            int oldIndex = specialIndexList[ranIndex];
                            s = s.Remove(oldIndex, 1).Insert(oldIndex, newNum);
                            specialIndexList.RemoveAt(ranIndex);
                            numIndexList.Add(oldIndex);
                        }
                        break;
                }
            }

            return s;
        }

        /// <summary>
        /// Returns a random string index matching the charType parameter.
        /// </summary>
        private int RandomCharIndex(Characters.CharType targetType, string s)
        {
            string charList = string.Empty;
            switch (targetType)
            {
                case Characters.CharType.AZUpper:
                    charList = azCharList.ToUpper();
                    break;

                case Characters.CharType.AZLower:
                    charList = azCharList.ToLower();
                    break;

                case Characters.CharType.Number:
                    charList = numCharList;
                    break;

                case Characters.CharType.Special:
                    charList = specialCharList;
                    break;
            }

            Random r = new Random();
            while (true)
            {
                char c = s[r.Next(0, s.Length)];
                if (charList.Contains(c.ToString()))
                {
                    return s.IndexOf(c);
                }
            }
        }

        /// <summary>
        /// Replaces a random character from a string with one matching the desiredType parameter.
        /// </summary>
        private string RandomReplaceChar(Characters.CharType desiredType, string password)
        {
            int azUpperCount = 0, azLowerCount = 0, numberCount = 0, specialCount = 0;
            foreach (char c in password)
            {
                if (azCharList.ToUpper().Contains(c.ToString()))
                {
                    azUpperCount++;
                }

                if (azCharList.ToLower().Contains(c.ToString()))
                {
                    azLowerCount++;
                }

                if (numCharList.Contains(c.ToString()))
                {
                    numberCount++;
                }

                if (specialCharList.Contains(c.ToString()))
                {
                    specialCount++;
                }
            }

            string charList = string.Empty;
            switch (desiredType)
            {
                case Characters.CharType.AZUpper:
                    charList = azCharList.ToUpper();
                    break;

                case Characters.CharType.AZLower:
                    charList = azCharList.ToLower();
                    break;

                case Characters.CharType.Number:
                    charList = numCharList;
                    break;

                case Characters.CharType.Special:
                    charList = specialCharList;
                    break;
            }

            Random r = new Random();
            bool charNotReplaced = true;
            while (charNotReplaced)
            {
                Characters.CharType t = (Characters.CharType)r.Next(1, 5);
                string newChar = charList[r.Next(0, charList.Length)].ToString();

                switch (t)
                {
                    case Characters.CharType.AZUpper:
                        if (azUpperCount > 1)
                        {
                            int oldCharIndex = RandomCharIndex(Characters.CharType.AZUpper, password);
                            password = password.Remove(oldCharIndex, 1).Insert(oldCharIndex, newChar);
                            charNotReplaced = false;
                        }
                        break;

                    case Characters.CharType.AZLower:
                        if (azLowerCount > 1)
                        {
                            int charIndex = RandomCharIndex(Characters.CharType.AZLower, password);
                            password = password.Remove(charIndex, 1).Insert(charIndex, newChar);
                            charNotReplaced = false;
                        }
                        break;

                    case Characters.CharType.Number:
                        if (numberCount > 1)
                        {
                            int charIndex = RandomCharIndex(Characters.CharType.Number, password);
                            password = password.Remove(charIndex, 1).Insert(charIndex, newChar);
                            charNotReplaced = false;
                        }
                        break;

                    case Characters.CharType.Special:
                        if (specialCount > 1)
                        {
                            int charIndex = RandomCharIndex(Characters.CharType.Special, password);
                            password = password.Remove(charIndex, 1).Insert(charIndex, newChar);
                            charNotReplaced = false;
                        }
                        break;
                }
            }

            return password;
        }

        private string ReplaceAmbiguousCharacters(string password)
        {
            string s = password;
            char replacement = '\0';

            for (int i = 0; i < s.Length; i++)
            {
                while (Characters.AmbiguousCharList.Contains(s[i].ToString()))
                {
                    var type = Characters.IdentifyCharType(Convert.ToChar(s[i]));
                    replacement = Characters.GetRandomChar(type);
                    if (!Characters.AmbiguousCharList.Contains(replacement.ToString()))
                    {
                        s = s.ReplaceAt(i, replacement);
                    }
                }
            }

            return s;
        }
    }
}