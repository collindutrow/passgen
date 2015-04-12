# passgen
A fully configurable .NET password generator and strength tester library. Supporting alphanumeric and special character options and much more.

Sample usage

```
PasswordGenerator p = new PasswordGenerator();

p.length = 8;
p.azUpper = true;
p.azLower = false;
p.numbers = true;
p.special = false;
p.minAmmountOfNumbers = 1;
p.requireEveryCharType = true;
p.avoidAmbiguousChars = true;

string password = p.Password;

int i = PasswordStrengthTester.Test(password);
int passwordStrength = i > 100 ? 100 : i;
```
