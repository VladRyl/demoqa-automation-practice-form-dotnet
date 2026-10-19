namespace DemoQA.Tests.Data;

public static class PhoneData
{
    public static IEnumerable<object[]> ValidPhoneNumbers =>
    [
        ["0000000000"],
        ["1234567890"],
        ["9999999999"]
    ];

    public static IEnumerable<object[]> InvalidPhoneNumbers =>
    [
        ["123456789"],
        ["abcdefghij"],
        ["12345abcde"],
        ["+123456789"],
        ["(123)45678"],
        ["123 456 78"],
        ["123-456-78"],
        ["!@#$%^&*()"],
        [""]
    ];
}