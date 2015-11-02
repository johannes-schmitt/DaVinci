# Only one level of indentation per method #

## Description ##
Having too many levels of indentation in your code is often bad for readability, and maintainability. Most of the time, you can't easily understand the code without compiling it in your head, especially if you have various conditions at different level, or a loop in another loop.

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
You can use the extract method pattern.

## Further Links ##
[http://www.cs.helsinki.fi/u/luontola/tdd-2009/ext/ObjectCalisthenics.pdf](http://www.cs.helsinki.fi/u/luontola/tdd-2009/ext/ObjectCalisthenics.pdf "http://www.cs.helsinki.fi/u/luontola/tdd-2009/ext/ObjectCalisthenics.pdf")

[http://refactoring.com/catalog/extractMethod.html](http://refactoring.com/catalog/extractMethod.html "http://refactoring.com/catalog/extractMethod.html")