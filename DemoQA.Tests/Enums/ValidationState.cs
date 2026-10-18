namespace DemoQA.Tests.Enums;

public enum ValidationState
{
    Valid,
    Invalid
}

public static class ValidationStateExtensions
{
    public static string ToRgb(this ValidationState state) => state switch
    {
        ValidationState.Valid => "rgb(25, 135, 84)",
        ValidationState.Invalid => "rgb(220, 53, 69)",
        _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
    };
}