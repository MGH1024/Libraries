using Application.Features.Libraries.Commands.CreateLibrary;
using Domain.Entities.Libraries.Constant;


namespace LibraryMicroservice.Test.Builders;

public class CreateLibraryCommandBuilder
{
    string _name;
    string _code;
    string _location;
    District _district;
    DateTime _registrationDate;

    public CreateLibraryCommandBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public CreateLibraryCommandBuilder WithCode(string code)
    {
        _code = code;
        return this;
    }

    public CreateLibraryCommandBuilder WithLocation(string location)
    {
        _location = location;
        return this;
    }

    public CreateLibraryCommandBuilder WithDistrict(District district)
    {
        _district = district;
        return this;
    }

    public CreateLibraryCommandBuilder WithRegistrationDate(DateTime registrationDate)
    {
        _registrationDate = registrationDate;
        return this;
    }

    public CreateLibraryCommand Build()
    {
        return new CreateLibraryCommand
        {
            Name = _name,
            District = _district,
            RegistrationDate = _registrationDate,
            Code = _code,
            Location = _location
        };
    }
}