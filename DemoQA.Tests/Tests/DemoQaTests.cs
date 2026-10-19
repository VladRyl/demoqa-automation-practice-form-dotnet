using DemoQA.Tests.Pages;
using DemoQA.Tests.Tests.Base;
using DemoQA.Tests.Enums;
using DemoQA.Tests.Data;
using Xunit;
using Xunit.Priority;
using Allure.Xunit.Attributes;
using Allure.Net.Commons;

namespace DemoQA.Tests.Tests;

[TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
public class DemoQaTests : BaseTest
{
    private static bool _isFirstTestFailed = false;

    [Fact]
    [AllureEpic("Student Registration")]
    [AllureFeature("Practice Form")]
    [Priority(1)]
    public async Task OpenDemoQaPage()
    {
        try
        {
            var page = new DemoQaPage(Page);
            await page.NavigateAsync();
            await Expect(page.MainHeader).ToHaveTextAsync("Practice Form");
        }
        catch
        {
            _isFirstTestFailed = true;
            throw;
        }
    }

    [Fact]
    [Priority(2)]
    public async Task AllFormsFilledCorrectly()
    {
        if (_isFirstTestFailed)
            throw new SkipException("Skipping this test because the first test failed");

        var page = new DemoQaPage(Page);
        await page.NavigateAsync();
        await Expect(page.StudentRegistrationForm).ToHaveTextAsync("Student Registration Form");
        await page.FillFirstNameAsync("User");
        await page.FillLastNameAsync("Test");
        await page.FillEmailAsync("test.email@test.com");
        await page.SelectGenderAsync(Gender.Male);
        await page.FillMobileNumberAsync("1234567890");
        await page.FillDateOfBirthAsync("01 Jan 1990");
        await page.FillSubjectsAsync(["Maths", "Computer"]);
        await page.SelectHobbiesAsync([Hobby.Sports, Hobby.Reading, Hobby.Music]);
        await page.UploadPictureAsync("Assets/manul.jpg");
        await page.FillCurrentAddressAsync("Street");
        await page.SelectStateAsync(State.NCR);
        await page.SelectCityAsync(City.Delhi);
        await page.SubmitAsync();
        await page.VerifyModalVisibleAsync();
        await Expect(page.GetModalValueByLabel("Student Name")).ToHaveTextAsync("User Test");
        await Expect(page.GetModalValueByLabel("Student Email")).ToHaveTextAsync("test.email@test.com");
        await Expect(page.GetModalValueByLabel("Gender")).ToHaveTextAsync("Male");
        await Expect(page.GetModalValueByLabel("Mobile")).ToHaveTextAsync("1234567890");
        await Expect(page.GetModalValueByLabel("Date of Birth")).ToHaveTextAsync("01 January,1990");
        await Expect(page.GetModalValueByLabel("Subjects")).ToHaveTextAsync("Maths, Computer Science");
        await Expect(page.GetModalValueByLabel("Hobbies")).ToHaveTextAsync("Sports, Reading, Music");
        await Expect(page.GetModalValueByLabel("Picture")).ToHaveTextAsync("manul.jpg");
        await Expect(page.GetModalValueByLabel("Address")).ToHaveTextAsync("Street");
        await Expect(page.GetModalValueByLabel("State and City")).ToHaveTextAsync("NCR Delhi");
        await page.CloseModalAsync();
    }

    [Fact]
    [Priority(2)]
    public async Task MandatoryFieldsFilledCorrectly()
    {
        if (_isFirstTestFailed)
            throw new SkipException("Skipping this test because the first test failed");

        var page = new DemoQaPage(Page);
        await page.NavigateAsync();
        await page.FillFirstNameAsync("User");
        await page.FillLastNameAsync("Test");
        await page.SelectGenderAsync(Gender.Female);
        await page.FillMobileNumberAsync("1234567890");
        await page.FillDateOfBirthAsync("01 Jan 1990");
        await page.SubmitAsync();
        await page.VerifyModalVisibleAsync();
        await Expect(page.GetModalValueByLabel("Student Name")).ToHaveTextAsync("User Test");
        await Expect(page.GetModalValueByLabel("Student Email")).ToHaveTextAsync("");
        await Expect(page.GetModalValueByLabel("Gender")).ToHaveTextAsync("Female");
        await Expect(page.GetModalValueByLabel("Mobile")).ToHaveTextAsync("1234567890");
        await Expect(page.GetModalValueByLabel("Date of Birth")).ToHaveTextAsync("01 January,1990");
        await Expect(page.GetModalValueByLabel("Subjects")).ToHaveTextAsync("");
        await Expect(page.GetModalValueByLabel("Hobbies")).ToHaveTextAsync("");
        await Expect(page.GetModalValueByLabel("Picture")).ToHaveTextAsync("");
        await Expect(page.GetModalValueByLabel("Address")).ToHaveTextAsync("");
        await Expect(page.GetModalValueByLabel("State and City")).ToHaveTextAsync("");
    }

    [Fact]
    [Priority(2)]
    public async Task EmptyAllFields()
    {
        if (_isFirstTestFailed)
            throw new SkipException("Skipping this test because the first test failed");

        var page = new DemoQaPage(Page);
        await page.NavigateAsync();
        await page.SubmitAsync();

        // Required fields — should be invalid (red)
        await page.VerifyFieldColorAsync(page.InputFirstName, ValidationState.Invalid);
        await page.VerifyFieldColorAsync(page.InputLastName, ValidationState.Invalid);
        await page.VerifyFieldColorAsync(page.GenderWrapper, ValidationState.Invalid);
        await page.VerifyFieldColorAsync(page.MobileNumber, ValidationState.Invalid);

        // Non-required fields — should be valid (green)
        await page.VerifyFieldColorAsync(page.InputUserEmail, ValidationState.Valid);
        await page.VerifyFieldColorAsync(page.DateOfBirth, ValidationState.Valid);
        await page.VerifyFieldColorAsync(page.Subjects, ValidationState.Valid);
        await page.VerifyFieldColorAsync(page.HobbiesWrapper, ValidationState.Valid);
        await page.VerifyFieldColorAsync(page.CurrentAddress, ValidationState.Valid);
        await page.VerifyFieldColorAsync(page.StateSelect, ValidationState.Valid);
        await page.VerifyFieldColorAsync(page.CitySelect, ValidationState.Valid);
    }

    [Fact]
    [Priority(2)]
    public async Task ChangeColor()
    {
        if (_isFirstTestFailed)
            throw new SkipException("Skipping this test because the first test failed");

        var page = new DemoQaPage(Page);
        await page.NavigateAsync();
        await page.SubmitAsync();

        // Required fields should be invalid (red) on empty submit
        await page.VerifyFieldColorAsync(page.InputFirstName, ValidationState.Invalid);
        await page.VerifyFieldColorAsync(page.InputLastName, ValidationState.Invalid);
        await page.VerifyFieldColorAsync(page.GenderWrapper, ValidationState.Invalid);
        await page.VerifyFieldColorAsync(page.MobileNumber, ValidationState.Invalid);

        // Fill required fields
        await page.FillFirstNameAsync("User");
        await page.FillLastNameAsync("Test");
        await page.SelectGenderAsync(Gender.Other);
        await page.FillMobileNumberAsync("1234567890");

        // Required fields should now be valid (green)
        await page.VerifyFieldColorAsync(page.InputFirstName, ValidationState.Valid);
        await page.VerifyFieldColorAsync(page.InputLastName, ValidationState.Valid);
        await page.VerifyFieldColorAsync(page.GenderWrapper, ValidationState.Valid);
        await page.VerifyFieldColorAsync(page.MobileNumber, ValidationState.Valid);

        await page.FillDateOfBirthAsync("01 Jan 1990");
        await page.SubmitAsync();
        await page.VerifyModalVisibleAsync();
        await Expect(page.GetModalValueByLabel("Student Name")).ToHaveTextAsync("User Test");
        await Expect(page.GetModalValueByLabel("Student Email")).ToHaveTextAsync("");
        await Expect(page.GetModalValueByLabel("Gender")).ToHaveTextAsync("Other");
        await Expect(page.GetModalValueByLabel("Mobile")).ToHaveTextAsync("1234567890");
        await Expect(page.GetModalValueByLabel("Date of Birth")).ToHaveTextAsync("01 January,1990");
        await Expect(page.GetModalValueByLabel("Subjects")).ToHaveTextAsync("");
        await Expect(page.GetModalValueByLabel("Hobbies")).ToHaveTextAsync("");
        await Expect(page.GetModalValueByLabel("Picture")).ToHaveTextAsync("");
        await Expect(page.GetModalValueByLabel("Address")).ToHaveTextAsync("");
        await Expect(page.GetModalValueByLabel("State and City")).ToHaveTextAsync("");
    }

    [Theory]
    [Priority(2)]
    [MemberData(nameof(NameData.ValidNames), MemberType = typeof(NameData))]
    public async Task ValidNameRegex(string firstName, string lastName)
    {
        if (_isFirstTestFailed)
            throw new SkipException("Skipping this test because the first test failed");

        var page = new DemoQaPage(Page);
        await page.NavigateAsync();
        await page.FillFirstNameAsync(firstName);
        await page.FillLastNameAsync(lastName);
        await page.SubmitAsync();
        await page.VerifyFieldColorAsync(page.InputFirstName, ValidationState.Valid);
        await page.VerifyFieldColorAsync(page.InputLastName, ValidationState.Valid);
    }

    [Theory]
    [Priority(2)]
    [MemberData(nameof(NameData.InvalidNames), MemberType = typeof(NameData))]
    public async Task InvalidNameRegex(string firstName, string lastName)
    {
        if (_isFirstTestFailed)
            throw new SkipException("Skipping this test because the first test failed");

        var page = new DemoQaPage(Page);
        await page.NavigateAsync();
        await page.FillFirstNameAsync(firstName);
        await page.FillLastNameAsync(lastName);
        await page.SubmitAsync();
        await page.VerifyFieldColorAsync(page.InputFirstName, ValidationState.Invalid);
        await page.VerifyFieldColorAsync(page.InputLastName, ValidationState.Invalid);
    }

    [Theory]
    [Priority(2)]
    [MemberData(nameof(EmailData.ValidEmails), MemberType = typeof(EmailData))]
    public async Task ValidEmailRegex(string validEmail)
    {
        if (_isFirstTestFailed)
            throw new SkipException("Skipping this test because the first test failed");

        var page = new DemoQaPage(Page);
        await page.NavigateAsync();
        await page.FillEmailAsync(validEmail);
        await page.SubmitAsync();
        await page.VerifyFieldColorAsync(page.InputUserEmail, ValidationState.Valid);
    }

    [Theory]
    [Priority(2)]
    [MemberData(nameof(EmailData.InvalidEmails), MemberType = typeof(EmailData))]
    public async Task InvalidEmailRegex(string invalidEmail)
    {
        if (_isFirstTestFailed)
            throw new SkipException("Skipping this test because the first test failed");

        var page = new DemoQaPage(Page);
        await page.NavigateAsync();
        await page.FillEmailAsync(invalidEmail);
        await page.SubmitAsync();
        await page.VerifyFieldColorAsync(page.InputUserEmail, ValidationState.Invalid);
    }

    [Theory]
    [Priority(2)]
    [InlineData(Gender.Male)]
    [InlineData(Gender.Female)]
    [InlineData(Gender.Other)]
    public async Task GenderSwitch(Gender gender)
    {
        if (_isFirstTestFailed)
            throw new SkipException("Skipping this test because the first test failed");

        var page = new DemoQaPage(Page);
        await page.NavigateAsync();
        await page.SelectGenderAsync(gender);
        await page.VerifyOnlyOneGenderCheckedAsync(gender);
    }

    [Theory]
    [Priority(2)]
    [MemberData(nameof(PhoneData.ValidPhoneNumbers), MemberType = typeof(PhoneData))]
    public async Task ValidPhoneNumber(string validPhone)
    {
        if (_isFirstTestFailed)
            throw new SkipException("Skipping this test because the first test failed");

        var page = new DemoQaPage(Page);
        await page.NavigateAsync();
        await page.FillMobileNumberAsync(validPhone);
        await page.SubmitAsync();
        await page.VerifyFieldColorAsync(page.MobileNumber, ValidationState.Valid);
    }

    [Theory]
    [Priority(2)]
    [MemberData(nameof(PhoneData.InvalidPhoneNumbers), MemberType = typeof(PhoneData))]
    public async Task InvalidPhoneNumber(string invalidPhone)
    {
        if (_isFirstTestFailed)
            throw new SkipException("Skipping this test because the first test failed");

        var page = new DemoQaPage(Page);
        await page.NavigateAsync();
        await page.FillMobileNumberAsync(invalidPhone);
        await page.SubmitAsync();
        await page.VerifyFieldColorAsync(page.MobileNumber, ValidationState.Invalid);
    }

    [Theory]
    [Priority(2)]
    [MemberData(nameof(DateData.DatePicker), MemberType = typeof(DateData))]
    public async Task SelectDatesViaPicker(string day, string month, string year, string expectedValue)
    {
        if (_isFirstTestFailed)
            throw new SkipException("Skipping this test because the first test failed");

        var page = new DemoQaPage(Page);
        await page.NavigateAsync();
        await page.FillDateViaPickerAsync(day, month, year);
        await Expect(page.DateOfBirth).ToHaveValueAsync(expectedValue);
    }

    [Fact]
    [Priority(2)]
    public async Task InvalidFebruary31_ThrowsException()
    {
        if (_isFirstTestFailed)
            throw new SkipException("Skipping this test because the first test failed");

        var page = new DemoQaPage(Page);
        await page.NavigateAsync();

        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            await page.FillDateViaPickerAsync("31", "February", "2024");
        });
    }

    [Fact]
    [Priority(2)]
    public async Task SubjectsRemoveBadge()
    {
        if (_isFirstTestFailed)
            throw new SkipException("Skipping this test because the first test failed");

        var page = new DemoQaPage(Page);
        await page.NavigateAsync();
        await page.FillSubjectsAsync(["Maths", "Arts"]);
        await page.RemoveSubjectAsync("Maths");
        await page.VerifySelectedSubjectsAsync(["Arts"]);
        await page.RemoveSubjectAsync("Arts");
        await page.VerifyNoSubjectsSelectedAsync();
    }

    [Fact]
    [Priority(2)]
    public async Task IncorrectBadge()
    {
        if (_isFirstTestFailed)
            throw new SkipException("Skipping this test because the first test failed");

        var page = new DemoQaPage(Page);
        await page.NavigateAsync();
        await page.FillSubjectsAsync(["XYZ"]);
        await page.VerifyNoSubjectsSelectedAsync();
    }

    [Fact]
    [Priority(2)]
    public async Task UnselectHobby()
    {
        if (_isFirstTestFailed)
            throw new SkipException("Skipping this test because the first test failed");

        var page = new DemoQaPage(Page);
        await page.NavigateAsync();
        var hobbies = new List<Hobby> { Hobby.Sports, Hobby.Reading, Hobby.Music };
        await page.SelectHobbiesAsync(hobbies);
        await page.VerifyHobbiesCheckedAsync(hobbies);
        await page.SelectHobbiesAsync(hobbies);
        await page.VerifyHobbiesCheckedAsync([]);
    }

    [Theory]
    [Priority(2)]
    [MemberData(nameof(FailData.ValidFileNames), MemberType = typeof(FailData))]
    public async Task UploadValidImageFormats(string fileName)
    {
        if (_isFirstTestFailed)
            throw new SkipException("Skipping this test because the first test failed");

        string tempFilePath = Path.Combine(Path.GetTempPath(), fileName);
        await File.WriteAllTextAsync(tempFilePath, "dummy content");

        try
        {
            var page = new DemoQaPage(Page);
            await page.NavigateAsync();
            await page.UploadPictureAsync(tempFilePath);
            string expectedFileName = Path.GetFileName(fileName);
            await Expect(page.UploadPictureInput).ToHaveValueAsync($@"C:\fakepath\{expectedFileName}");
            await page.FillFirstNameAsync("User");
            await page.FillLastNameAsync("Test");
            await page.SelectGenderAsync(Gender.Male);
            await page.FillMobileNumberAsync("1234567890");
            await page.SubmitAsync();
            await Expect(page.ModalContent).ToBeVisibleAsync();
            await Expect(page.GetModalValueByLabel("Picture")).ToHaveTextAsync(expectedFileName);
        }
        finally
        {
            if (File.Exists(tempFilePath))
            {
                File.Delete(tempFilePath);
            }
        }
    }

    [Theory]
    [Priority(2)]
    [MemberData(nameof(FailData.InvalidFileNames), MemberType = typeof(FailData))]
    public async Task UploadInvalidFileFormats(string fileName)
    {
        if (_isFirstTestFailed)
            throw new SkipException("Skipping this test because the first test failed");

        string tempFilePath = Path.Combine(Path.GetTempPath(), fileName);
        await File.WriteAllTextAsync(tempFilePath, "dummy content");

        try
        {
            var page = new DemoQaPage(Page);
            await page.NavigateAsync();
            await page.UploadPictureAsync(tempFilePath);
            await page.FillFirstNameAsync("User");
            await page.FillLastNameAsync("Test");
            await page.SelectGenderAsync(Gender.Male);
            await page.FillMobileNumberAsync("1234567890");
            await page.SubmitAsync();
            await Expect(page.ModalContent).Not.ToBeVisibleAsync();
        }
        finally
        {
            if (File.Exists(tempFilePath))
            {
                File.Delete(tempFilePath);
            }
        }
    }

    [Fact]
    [Priority(2)]
    public async Task CurrentAddress_SpecialCharacters_PreservesValue()
    {
        if (_isFirstTestFailed)
            throw new SkipException("Skipping this test because the first test failed");

        const string specialCharsAddress = 
            "123/4B 'Main St.' & \"Oak Ave.\"; <script>alert(1)</script> " +
            "#404! ~`@$^*()_+=-[]{}\\|/? " +
            "Київ-Дніпро, вул. Залізнична, 15/A \n" +
            "Apartment ⚡️🏢 🔥 12345-6789";

        var page = new DemoQaPage(Page);
        await page.NavigateAsync();

        await page.FillCurrentAddressAsync(specialCharsAddress);
        await Expect(page.CurrentAddress).ToHaveValueAsync(specialCharsAddress);
    }

    [Fact]
    [Priority(2)]
    public async Task City_DisabledByDefault()
    {
        if (_isFirstTestFailed)
            throw new SkipException("Skipping this test because the first test failed");

        var page = new DemoQaPage(Page);
        await page.NavigateAsync();

        await page.VerifyCityIsDisabledAsync();
    }

    [Theory]
    [Priority(2)]
    [MemberData(nameof(StateCityData.StateCityMap), MemberType = typeof(StateCityData))]
    public async Task SelectStateAndCity_Cascade(State state, City[] cities)
    {
        if (_isFirstTestFailed)
            throw new SkipException("Skipping this test because the first test failed");

        var page = new DemoQaPage(Page);
        await page.NavigateAsync();

        await page.VerifyCityIsDisabledAsync();
        await page.SelectStateAsync(state);
        await page.VerifyCityIsEnabledAsync();

        City selectedCity = cities[0];
        await page.SelectCityAsync(selectedCity);

        await page.FillFirstNameAsync("User");
        await page.FillLastNameAsync("Test");
        await page.SelectGenderAsync(Gender.Male);
        await page.FillMobileNumberAsync("0000000000");
        await page.SubmitAsync();

        string expectedState = StateCityData.GetStateValue(state);
        string expectedCity = StateCityData.GetCityValue(selectedCity);

        await Expect(page.GetModalValueByLabelAsync("State and City")).ToHaveTextAsync($"{expectedState} {expectedCity}");
    }

    [Fact]
    [Priority(2)]
    public async Task ChangeState_ResetsCity()
    {
        if (_isFirstTestFailed)
            throw new SkipException("Skipping this test because the first test failed");

        var page = new DemoQaPage(Page);
        await page.NavigateAsync();

        await page.SelectStateAsync(State.NCR);
        await page.SelectCityAsync(City.Delhi);
        await page.SelectStateAsync(State.Rajasthan);
        await page.SelectCityAsync(City.Jaipur);
    }

    [Fact]
    [Priority(2)]
    public async Task SelectOnlyState_Cascade()
    {
    if (_isFirstTestFailed)
        throw new SkipException("Skipping this test because the first test failed");

        var page = new DemoQaPage(Page);
        await page.NavigateAsync();

        await page.SelectStateAsync(State.NCR);
        await page.FillFirstNameAsync("User");
        await page.FillLastNameAsync("Test");
        await page.SelectGenderAsync(Gender.Male);
        await page.FillMobileNumberAsync("0000000000");
        await page.SubmitAsync();

        string expectedState = StateCityData.GetStateValue(State.NCR);

        await Expect(page.GetModalValueByLabelAsync("State and City")).ToHaveTextAsync(expectedState);
    }

    [Fact]
    [Priority(2)]
    public async Task City_ResetsOnStateChange()
    {
        if (_isFirstTestFailed)
            throw new SkipException("Skipping this test because the first test failed");

        var page = new DemoQaPage(Page);
        await page.NavigateAsync();

        await page.SelectStateAsync(State.NCR);
        await page.SelectCityAsync(City.Delhi);
        await page.SelectStateAsync(State.Haryana);
        
        await page.VerifyCityIsResetAsync();
    }
}