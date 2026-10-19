using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;
using Xunit;
using DemoQA.Tests.Pages.Base;
using DemoQA.Tests.Enums;
using DemoQA.Tests.Data;

namespace DemoQA.Tests.Pages;

public class DemoQaPage : BasePage
{
    public DemoQaPage(IPage page) : base(page) { }

    public ILocator MainHeader => Page.Locator(".container.playgound-body h1");
    public ILocator StudentRegistrationForm => Page.Locator(".container.playgound-body h5");
    public ILocator InputFirstName => Page.Locator("#firstName");
    public ILocator InputLastName => Page.Locator("#lastName");
    public ILocator InputUserEmail => Page.Locator("#userEmail");
    public ILocator GenderWrapper => Page.Locator("#genterWrapper");
    public ILocator MobileNumber => Page.Locator("#userNumber");
    public ILocator DateOfBirth => Page.Locator("#dateOfBirthInput");
    public ILocator Subjects => Page.Locator("#subjectsInput");
    public ILocator HobbiesWrapper => Page.Locator("#hobbiesWrapper");
    public ILocator UploadPictureInput => Page.Locator("#uploadPicture");
    public ILocator CurrentAddress => Page.Locator("#currentAddress");
    public ILocator StateSelect => Page.Locator("#state");
    public ILocator CitySelect => Page.Locator("#city");
    public ILocator ModalContent => Page.Locator(".modal-content");
    private ILocator SubmitButton => Page.Locator("#submit");
    private ILocator ModalTitle => ModalContent.Locator("#example-modal-sizes-title-lg");
    private ILocator ModalTable => ModalContent.Locator("table");
    private ILocator CloseModalButton => ModalContent.Locator("#closeLargeModal");

    public async Task OpenAsync()
    {
        await NavigateToAsync("/automation-practice-form");
        await MainHeader.WaitForAsync();
    }

    public Task NavigateAsync() => OpenAsync();

    public async Task FillFirstNameAsync(string firstName) => await InputFirstName.FillAsync(firstName);
    public async Task FillLastNameAsync(string lastName) => await InputLastName.FillAsync(lastName);
    public async Task FillEmailAsync(string email) => await InputUserEmail.FillAsync(email);
    public async Task SelectGenderAsync(Gender gender) => await Page.GetByText(gender.ToString(), new() { Exact = true }).ClickAsync();
    public async Task FillMobileNumberAsync(string mobileNumber) => await MobileNumber.FillAsync(mobileNumber);
    public async Task UploadPictureAsync(string filePath) => await UploadPictureInput.SetInputFilesAsync(filePath);
    public async Task FillCurrentAddressAsync(string currentAddress) => await CurrentAddress.FillAsync(currentAddress);
    public async Task SubmitAsync() => await SubmitButton.ClickAsync();

    public async Task FillDateOfBirthAsync(string dateOfBirth)
    {
        await DateOfBirth.FillAsync(dateOfBirth);
        await DateOfBirth.PressAsync("Enter");
    }

    public async Task FillDateViaPickerAsync(string day, string month, string year)
    {
        await DateOfBirth.ClickAsync();
        await Page.Locator(".react-datepicker__month-select").SelectOptionAsync(new SelectOptionValue { Label = month });
        await Page.Locator(".react-datepicker__year-select").SelectOptionAsync(year);

        string dayCode = int.Parse(day).ToString("000");
        ILocator dayLocator = Page.Locator($".react-datepicker__day--{dayCode}:not(.react-datepicker__day--outside-month)");

        var count = await dayLocator.CountAsync();
        if (count == 0)
        {
            throw new ArgumentException($"Day '{day}' does not exist in {month} {year}.");
        }

        await dayLocator.ClickAsync();
    }

    public async Task FillSubjectsAsync(List<string> subjects)
    {
        ILocator firstOption = Page.Locator(".subjects-auto-complete__option").First;
        foreach (string subject in subjects)
        {
            await Subjects.FillAsync(subject);
            try
            {
                await firstOption.WaitForAsync(new() { State = WaitForSelectorState.Visible, Timeout = 2000 });
                await firstOption.ClickAsync();
            }
            catch (Exception)
            {
                await Subjects.PressAsync("Escape");
            }
        }
    }

    public async Task RemoveSubjectAsync(string subject)
    {
        ILocator badgeRemoveBtn = Page.Locator($".subjects-auto-complete__multi-value:has-text('{subject}') .subjects-auto-complete__multi-value__remove");
        await badgeRemoveBtn.ClickAsync();
    }

    public async Task VerifySelectedSubjectsAsync(List<string> expectedSubjects)
    {
        await Expect(Page.Locator(".subjects-auto-complete__multi-value")).ToHaveTextAsync(expectedSubjects);
    }

    public async Task VerifyNoSubjectsSelectedAsync()
    {
        await Expect(Page.Locator(".subjects-auto-complete__multi-value")).ToHaveCountAsync(0);
    }

    public async Task SelectHobbiesAsync(List<Hobby> hobbies)
    {
        foreach (var hobby in hobbies)
        {
            ILocator hobbyLabel = HobbiesWrapper.GetByText(hobby.ToString(), new() { Exact = true });
            await hobbyLabel.ClickAsync();
        }
    }

    public async Task VerifyHobbiesCheckedAsync(List<Hobby> hobbies)
    {
        List<Hobby> allHobbies = [Hobby.Sports, Hobby.Reading, Hobby.Music];
        List<string> expectedValues = hobbies.Select(h => ((int)h).ToString()).ToList();

        foreach (var hobby in allHobbies)
        {
            ILocator checkbox = HobbiesWrapper.Locator($"input[type='checkbox'][value='{(int)hobby}']");
            if (hobbies.Contains(hobby))
                await Expect(checkbox).ToBeCheckedAsync();
            else
                await Expect(checkbox).ToBeCheckedAsync(new() { Checked = false });
        }
    }

    public async Task SelectStateAsync(State state)
    {
        string stateString = StateCityData.GetStateValue(state);
        await StateSelect.ClickAsync();
        var inputInsideState = StateSelect.Locator("input");
        await inputInsideState.FillAsync(stateString);
        await inputInsideState.PressAsync("Enter");
    }

    public async Task SelectCityAsync(City city)
    {
        string cityString = StateCityData.GetCityValue(city);
        await CitySelect.ClickAsync();
        var inputInsideCity = CitySelect.Locator("input");
        await inputInsideCity.FillAsync(cityString);
        await inputInsideCity.PressAsync("Enter");
    }

    public async Task VerifyCityIsDisabledAsync() => await Expect(CitySelect.Locator("input")).ToBeDisabledAsync();
    public async Task VerifyCityIsEnabledAsync() => await Expect(CitySelect.Locator("input")).ToBeEnabledAsync();
    public async Task<string> GetSelectedCityText() => await CitySelect.Locator("[class*='singleValue']").TextContentAsync() ?? "";

    public async Task VerifyCityIsResetAsync()
    {
        string selectedText = await GetSelectedCityText();
        Assert.True(selectedText == "" || selectedText == "Select City", $"Expected city to be reset, but was '{selectedText}'");
    }

    public async Task VerifyModalVisibleAsync()
    {
        await Expect(ModalContent).ToBeVisibleAsync();
        await Expect(ModalTitle).ToHaveTextAsync("Thanks for submitting the form");
    }

    public ILocator GetModalValueByLabel(string label)
    {
        return ModalTable.Locator($"tr:has(td:nth-child(1):has-text('{label}')) td:nth-child(2)");
    }

    public ILocator GetModalValueByLabelAsync(string label) => GetModalValueByLabel(label);

    public async Task CloseModalAsync()
    {
        await CloseModalButton.ClickAsync();
        await Expect(ModalContent).ToBeHiddenAsync();
    }

    public async Task VerifyFieldColorAsync(ILocator locator, ValidationState state = ValidationState.Invalid)
    {
        ILocator checkLabels = locator.Locator(".form-check-label");
        if (await checkLabels.CountAsync() > 0)
            await Expect(checkLabels.First).ToHaveCSSAsync("color", state.ToRgb());
        else
            await Expect(locator).ToHaveCSSAsync("border-color", state.ToRgb());
    }

    public async Task VerifyGenderIsCheckedAsync(Gender gender)
    {
        ILocator radioInput = GenderWrapper.Locator($"input[value='{gender}']");
        await Expect(radioInput).ToBeCheckedAsync();
    }

    public async Task VerifyGenderIsNotCheckedAsync(Gender gender)
    {
        ILocator radioInput = GenderWrapper.Locator($"input[value='{gender}']");
        await Expect(radioInput).ToBeCheckedAsync(new() { Checked = false });
    }

    public async Task VerifyOnlyOneGenderCheckedAsync(Gender selectedGender)
    {
        List<Gender> allGenders = Enum.GetValues<Gender>().ToList();
        foreach (var gender in allGenders)
        {
            if (gender == selectedGender)
                await VerifyGenderIsCheckedAsync(gender);
            else
                await VerifyGenderIsNotCheckedAsync(gender);
        }
    }
}