namespace DemoQA.Tests.Data;

using DemoQA.Tests.Enums;

public static class StateCityData
{
    public static IEnumerable<object[]> StateCityMap =>
    [
        [State.NCR, new[] { City.Delhi, City.Gurgaon, City.Noida }],
        [State.UttarPradesh, new[] { City.Agra, City.Lucknow, City.Merrut }],
        [State.Haryana, new[] { City.Karnal, City.Panipat }],
        [State.Rajasthan, new[] { City.Jaipur, City.Jaiselmer }]
    ];

    public static string GetStateValue(State state) => state switch
    {
        State.UttarPradesh => "Uttar Pradesh",
        _ => state.ToString()
    };

    public static string GetCityValue(City city) => city.ToString();
}