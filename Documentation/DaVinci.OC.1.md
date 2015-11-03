# Only one level of indentation per method #

## Description ##
Having too many levels of indentation in your code is a sign that the Single Responsibility Principle (SRP) is violated. Mostly, the method is doing things on different abstraction layers. 
Because there are too many abstractions, the code can be hard to understand. 

## Example ##
```C#
class SomeClass
{
	private readonly int[,] grid = new int[10, 15];

	public string SomeMethod()
	{
		string result = string.Empty;

		for (int column = 0; column < 10; column++)
		{
			// 1st indentation
			for (int row = 0; row < 10; row++)
			{
				// 2nd indentation
				result += this.grid[row, column].ToString();
			}
		}

		return result;
	}
}
```

## Refactoring patterns ##
- [Extract Method](http://refactoring.com/catalog/extractMethod.html)

## Category ##
Object Calisthenics (Jeff Bay)
