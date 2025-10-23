using CashFlow.Application.UseCases.Users.Register;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using Shouldly;

namespace Validator.Tests.Users.Register;

public class RegisterUserValidatorTest
{
  [Fact]
  public void Succes()
  {
    var validator = new RegisterUserValidator();
    var request = RequestRegisterUserJsonBuilder.Build();

    var result = validator.Validate(request);

    result.IsValid.ShouldBeTrue();
  }

  [Theory]
  [InlineData("")]
  [InlineData("         ")]
  [InlineData(null)]
  public void Error_Name_Empty(string name)
  {
    var validator = new RegisterUserValidator();
    var request = RequestRegisterUserJsonBuilder.Build();
    request.Name = name;

    var result = validator.Validate(request);

    result.IsValid.ShouldBeFalse();
    result.Errors.ShouldHaveSingleItem();
    result.Errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceErrorMessages.NAME_EMPTY));
  }

  [Theory]
  [InlineData("")]
  [InlineData("         ")]
  [InlineData(null)]
  public void Error_Email_Empty(string email)
  {
    var validator = new RegisterUserValidator();
    var request = RequestRegisterUserJsonBuilder.Build();
    request.Email = email;

    var result = validator.Validate(request);

    result.IsValid.ShouldBeFalse();
    result.Errors.ShouldHaveSingleItem();
    result.Errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceErrorMessages.EMAIL_EMPTY));
  }

  [Fact]
  public void Error_Email_Invalid()
  {
    var validator = new RegisterUserValidator();
    var request = RequestRegisterUserJsonBuilder.Build();
    request.Email = "welisson.com";

    var result = validator.Validate(request);

    result.IsValid.ShouldBeFalse();
    result.Errors.ShouldHaveSingleItem();
    result.Errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceErrorMessages.EMAIL_INVALID));
  }

  [Fact]
  public void Error_Password_Empty()
  {
    var validator = new RegisterUserValidator();
    var request = RequestRegisterUserJsonBuilder.Build();
    request.Password = string.Empty;

    var result = validator.Validate(request);

    result.IsValid.ShouldBeFalse();
    result.Errors.ShouldHaveSingleItem();
    result.Errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceErrorMessages.INVALID_PASSWORD));
  }
}
