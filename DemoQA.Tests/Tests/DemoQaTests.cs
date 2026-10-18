using DemoQA.Tests.Pages;
using DemoQA.Tests.Tests.Base;
using DemoQA.Tests.Enums;
using Microsoft.Playwright.Xunit;
using Xunit;

namespace DemoQA.Tests.Tests;

public class DemoQaTests : BaseTest
{
    [Fact]
    public async Task OpenDemoQaPage_Success()
    {
        var demoQaPage = new DemoQaPage(Page);
        await demoQaPage.NavigateAsync();
        await Expect(demoQaPage.MainHeader).ToHaveTextAsync("Practice Form");
    }

    [Fact]
    public async Task AllFormsFilledCorrectly_Success(){
        var demoQaPage = new DemoQaPage(Page);
        await demoQaPage.NavigateAsync();
        await Expect(demoQaPage.StudentRegistrationForm).ToHaveTextAsync("Student Registration Form");
        await demoQaPage.FillFirstNameAsync("User");
        await demoQaPage.FillLastNameAsync("Test");
        await demoQaPage.FillEmailAsync("test.email@test.com");
        await demoQaPage.SelectGenderAsync(Gender.Male);
        await demoQaPage.FillMobileNumberAsync("1234567890");
        await demoQaPage.FillDateOfBirthAsync("01 Jan 1990");
        await demoQaPage.FillSubjectsAsync(["Maths", "Computer"]);
        await demoQaPage.SelectHobbiesAsync([Hobby.Sports, Hobby.Reading, Hobby.Music]);
        await demoQaPage.UploadPictureAsync("Assets/manul.jpg");
        await demoQaPage.FillCurrentAddressAsync("Street");
        await demoQaPage.SelectStateAsync("NCR");
        await demoQaPage.SelectCityAsync("Delhi");
        await demoQaPage.SubmitAsync();
        await demoQaPage.VerifyModalVisibleAsync();
        await Expect(demoQaPage.GetModalValueByLabelAsync("Student Name")).ToHaveTextAsync("User Test");
        await Expect(demoQaPage.GetModalValueByLabelAsync("Student Email")).ToHaveTextAsync("test.email@test.com");
        await Expect(demoQaPage.GetModalValueByLabelAsync("Gender")).ToHaveTextAsync("Male");
        await Expect(demoQaPage.GetModalValueByLabelAsync("Mobile")).ToHaveTextAsync("1234567890");
        await Expect(demoQaPage.GetModalValueByLabelAsync("Date of Birth")).ToHaveTextAsync("01 January,1990");
        await Expect(demoQaPage.GetModalValueByLabelAsync("Subjects")).ToHaveTextAsync("Maths, Computer Science");
        await Expect(demoQaPage.GetModalValueByLabelAsync("Hobbies")).ToHaveTextAsync("Sports, Reading, Music");
        await Expect(demoQaPage.GetModalValueByLabelAsync("Picture")).ToHaveTextAsync("manul.jpg");
        await Expect(demoQaPage.GetModalValueByLabelAsync("Address")).ToHaveTextAsync("Street");
        await Expect(demoQaPage.GetModalValueByLabelAsync("State and City")).ToHaveTextAsync("NCR Delhi");
        await demoQaPage.CloseModalAsync();
    }
}