namespace DemoQA.Tests.Data;

public static class NameData
{
    public static IEnumerable<object[]> ValidNames =>
    [
        ["User", "Test"],
        ["A", "B"],
        ["John-Paul", "O'Connor"],
        ["Jean Luc", "Van Damme"],
        [new string('a', 50), new string('b', 50)],
    ];

    public static IEnumerable<object[]> InvalidNames =>
    [
        ["", ""],
        ["User123", "Test"],
        ["User!", "Test#"],
        ["   ", "   "],
    ];
}
