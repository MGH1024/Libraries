using System.Collections;
using Application.Features.Libraries.Commands.CreateLibrary;
using Domain.Entities.Libraries.Constant;
using FluentValidation.TestHelper;
using LibraryMicroservice.Test.Base.Builders;

namespace LibraryMicroservice.Test.ValidatorTests;

public class CreateLibraryCommandValidatorTest
{
    [Theory]
    [InlineData("")]
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa" +
                "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa" +
                "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa" +
                "aaaaaaaaaaaaaaaaaaa")]
    public void GivenLibraryName_WhenInvalidData_ThenWillInvalid(string libraryName)
    {
        var dto = new CreateLibraryCommandBuilder().WithName(libraryName).Build();

        var validator = new CreateLibraryCommandValidator();
        var result = validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }


    [Theory]
    [InlineData("")]
    [InlineData("aaaa")]
    public void GivenLibraryCode_WhenInvalidData_ThenWillInvalid(string libraryCode)
    {
        var dto = new CreateLibraryCommandBuilder().WithCode(libraryCode).Build();

        var validator = new CreateLibraryCommandValidator();
        var result = validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Code);
    }

    [Theory]
    [InlineData("")]
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa" +
                "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa" +
                "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa" +
                "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa" +
                "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa" +
                "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
    public void GivenLibraryLocation_WhenInvalidData_ThenWillInvalid(string libraryLocation)
    {
        var dto = new CreateLibraryCommandBuilder().WithLocation(libraryLocation).Build();

        var validator = new CreateLibraryCommandValidator();
        var result = validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Location);
    }

    [Fact]
    public void GivenLibraryRegistrationDate_WhenInvalidData_ThenWillInvalid_WithFact()
    {
        var date = new DateTime();
        var dto = new CreateLibraryCommandBuilder().WithRegistrationDate(date).Build();
        var validator = new CreateLibraryCommandValidator();
        var result = validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.RegistrationDate);
    }


    [Theory]
    [ClassData(typeof(RegistrationDateData))]
    public void GivenLibraryRegistrationDate_WhenInvalidData_ThenWillInvalid(DateTime registrationDate)
    {
        var dto = new CreateLibraryCommandBuilder().WithRegistrationDate(registrationDate).Build();

        var validator = new CreateLibraryCommandValidator();
        var result = validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.RegistrationDate);
    }


    [Theory]
    [MemberData(nameof(Data))]
    public void GivenLibraryRegistrationDate_WhenInvalidData_ThenWillInvalid2(DateTime registrationDate)
    {
        var dto = new CreateLibraryCommandBuilder().WithRegistrationDate(registrationDate).Build();

        var validator = new CreateLibraryCommandValidator();
        var result = validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.RegistrationDate);
    }

    public static IEnumerable<object[]> Data =>
        new List<object[]>
        {
            new object[] { new DateTime() },
        };

    [Theory]
    [MemberData(nameof(DataForDistrict))]
    public void GivenLibraryDistrict_WhenInvalidData_ThenWillInvalid(District district)
    {
        var dto = new CreateLibraryCommandBuilder().WithDistrict(district).Build();
        var validator = new CreateLibraryCommandValidator();
        var result = validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.District);
    }

    public static IEnumerable<object[]> DataForDistrict =>
        new List<object[]>
        {
            new object[] { (District)4 },
        };
}

public class RegistrationDateData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { new DateTime() };
        //yield return new object[] { new DateTime(2022,1,1) };
        //yield return new object[] { new DateTime(2023,1,1) };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}