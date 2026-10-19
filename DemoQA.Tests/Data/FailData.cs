namespace DemoQA.Tests.Data;

public static class FailData
{
    public static IEnumerable<object[]> ValidFileNames =>
    [
        ["avatar.jpg"],
        ["photo.jpeg"],
        ["image.png"],
        ["picture.gif"],
        ["icon.webp"]
    ];

    public static IEnumerable<object[]> InvalidFileNames =>
    [
        ["document.pdf"],
        ["script.exe"],
        ["data.json"],
        ["archive.zip"],
        ["virus.bat"]
    ];
}