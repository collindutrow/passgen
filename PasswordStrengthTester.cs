using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassGen
{
    public static class PasswordStrengthTester
    {
        private static readonly string azCharList = Characters.AZCharList;
        private static readonly string numCharList = Characters.NumCharList;
        private static readonly string specialCharList = Characters.SpecialCharList;

        public static int Test(string password)
        {
            int index = 0;
            List<int> azLowerIndexList = new List<int>();
            List<int> azUpperIndexList = new List<int>();
            List<int> numIndexList = new List<int>();
            List<int> specialIndexList = new List<int>();

            int middleNumbers = 0, middleSpecial = 0;
            int consecutiveAZUpper = 0, consecutiveAZLower = 0, consecutiveNumbers = 0, consecutiveSpecial = 0;
            int requirementScore = 0;

            Characters.CharType previousType = Characters.CharType.None;

            foreach (char c in password)
            {
                if (azCharList.ToUpper().Contains(c))
                {
                    azUpperIndexList.Add(index);
                    if (azUpperIndexList.Count == 1)
                    {
                        requirementScore++;
                    }

                    if (previousType == Characters.CharType.AZUpper)
                    {
                        consecutiveAZUpper++;
                    }

                    previousType = Characters.CharType.AZUpper;
                }

                if (azCharList.ToLower().Contains(c))
                {
                    azLowerIndexList.Add(index);
                    if (azLowerIndexList.Count == 1)
                    {
                        requirementScore++;
                    }

                    if (previousType == Characters.CharType.AZLower)
                    {
                        consecutiveAZLower++;
                    }

                    previousType = Characters.CharType.AZLower;
                }

                if (numCharList.Contains(c))
                {
                    numIndexList.Add(index);
                    if (numIndexList.Count == 1)
                    {
                        requirementScore++;
                    }

                    if (index != 0 && index != (password.Length - 1))
                    {
                        middleNumbers++;
                    }

                    if (previousType == Characters.CharType.Number)
                    {
                        consecutiveNumbers++;
                    }

                    previousType = Characters.CharType.Number;
                }

                if (specialCharList.Contains(c))
                {
                    specialIndexList.Add(index);
                    if (specialIndexList.Count == 1)
                    {
                        requirementScore++;
                    }

                    if (index != 0 && index != (password.Length - 1))
                    {
                        middleSpecial++;
                    }

                    if (previousType == Characters.CharType.Special)
                    {
                        consecutiveSpecial++;
                    }

                    previousType = Characters.CharType.Special;
                }
                index++;
            }

            // Additions
            int lengthScore = (password.Length * 4);
            int azUpperCountScore = (azUpperIndexList.Count > 0) ? ((password.Length - azUpperIndexList.Count) * 2) : 0;
            int azLowerCountScore = (azLowerIndexList.Count > 0) ? ((password.Length - azLowerIndexList.Count) * 2) : 0;
            int numCountScore = (numIndexList.Count * 4);
            int specialCountScore = (specialIndexList.Count * 6);
            int middleCountScore = ((middleNumbers + middleSpecial) * 2);

            if (password.Length >= 8)
            {
                requirementScore++;
                requirementScore = (requirementScore * 2);
            }
            else
            {
                requirementScore = 0;
            }

            int additionPoints = lengthScore + azUpperCountScore + azLowerCountScore + numCountScore + specialCountScore + middleCountScore + requirementScore;

            // Deductions
            int deductionPoints = 0;

            if (password.Length == (azLowerIndexList.Count + azUpperIndexList.Count))
            {
                deductionPoints += password.Length;
            }

            if (password.Length == numIndexList.Count)
            {
                deductionPoints += password.Length;
            }

            // Count the number of characters that are repeated.
            int repeatingChars = 0;
            foreach (char c in password)
            {
                if (password.Count(x => x == c) > 1)
                {
                    repeatingChars++;
                }
            }

            // The following code checks and counts character sequences (E.G. abc 123 !@#.) 
            int azSequenceCount = 0;
            int numSequenceCount = 0;
            int specialSequenceCount = 0;

            for (int i = 0; i < password.Length; i++ )
            {
                if (i > 0)
                {
                    char c = password[i];

                    if (azCharList.Contains(c))
                    {
                        int cIndex = azCharList.IndexOf(c);
                        char prevChar = (i > 0) ? password[i - 1] : '\0';
                        char nextChar = (i < password.Length - 1) ? password[i + 1] : '\0';

                        int prevIndex = cIndex - 1;
                        int nextIndex = cIndex + 1;

                        if (prevIndex >= 0 && nextIndex < azCharList.Length)
                        {
                            if (azCharList[cIndex - 1] == prevChar && azCharList[cIndex + 1] == nextChar)
                            {
                                azSequenceCount++;
                            }
                        }
                    }
                    else if (numCharList.Contains(c))
                    {
                        int cIndex = numCharList.IndexOf(c);
                        char prevChar = (i > 0) ? password[i - 1] : '\0';
                        char nextChar = (i < password.Length - 1) ? password[i + 1] : '\0';

                        int prevIndex = cIndex - 1;
                        int nextIndex = cIndex + 1;

                        if (prevIndex >= 0 && nextIndex < numCharList.Length)
                        {
                            if (numCharList[cIndex - 1] == prevChar && numCharList[cIndex + 1] == nextChar)
                            {
                                numSequenceCount++;
                            }
                        }
                    }
                    else if (specialCharList.Contains(c))
                    {
                        int cIndex = specialCharList.IndexOf(c);
                        char prevChar = (i > 0) ? password[i - 1] : '\0';
                        char nextChar = (i < password.Length - 1) ? password[i + 1] : '\0';

                        int prevIndex = cIndex - 1;
                        int nextIndex = cIndex + 1;

                        if (prevIndex >= 0 && nextIndex < specialCharList.Length)
                        {
                            if (specialCharList[cIndex - 1] == prevChar && specialCharList[cIndex + 1] == nextChar)
                            {
                                specialSequenceCount++;
                            }
                        }
                    }
                }
            }

            deductionPoints += repeatingChars;
            deductionPoints += (consecutiveAZUpper * 2);
            deductionPoints += (consecutiveAZLower * 2);
            deductionPoints += (consecutiveNumbers * 2);
            // deductionPoints += (consecutiveSpecial * 2);
            deductionPoints += (azSequenceCount * 3);
            deductionPoints += (numSequenceCount * 3);
            deductionPoints += (specialSequenceCount * 3);

            return additionPoints - deductionPoints;
        }
    }
}
