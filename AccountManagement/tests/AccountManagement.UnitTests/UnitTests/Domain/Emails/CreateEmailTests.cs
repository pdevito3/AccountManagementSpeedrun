namespace AccountManagement.UnitTests.UnitTests.Domain.Emails;

using AccountManagement.Domain.Emails;
using Bogus;
using FluentAssertions;
using NUnit.Framework;

[Parallelizable]
public class CreateEmailTests
{
    private readonly Faker _faker;

    public CreateEmailTests()
    {
        _faker = new Faker();
    }
    
    [Test]
    public void can_create_valid_email()
    {
        var validEmail = _faker.Person.Email;
        var emailVo = new Email(validEmail);
        emailVo.Value.Should().Be(validEmail);
    }

    [Test]
    public void can_not_add_invalid_email()
    {
        var validEmail = _faker.Lorem.Word();
        var act = () => new Email(validEmail);
        act.Should().Throw<FluentValidation.ValidationException>();
    }

    [Test]
    public void email_can_be_null()
    {
        var emailVo = new Email(null);
        emailVo.Value.Should().BeNull();
    }
}