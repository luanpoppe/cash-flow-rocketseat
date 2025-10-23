using CashFlow.Application.UseCases.Users;
using CashFlow.Communication.Requests;
using FluentValidation;
using Shouldly;

namespace Validator.Tests.Users;

public class PasswordValidatorTest
{
  [Theory]
  [InlineData("")]
  [InlineData("         ")]
  [InlineData(null)]
  [InlineData("a")]
  [InlineData("aa")]
  [InlineData("aaa")]
  [InlineData("aaaa")]
  [InlineData("aaaaa")]
  [InlineData("aaaaaa")]
  [InlineData("aaaaaaa")]
  [InlineData("aaaaaaaa")]
  [InlineData("AAAAAAAA")]
  [InlineData("Aaaaaaaa")]
  [InlineData("Aaaaaaa1")]
  public void Error_Password_Invalid(string password)
  {
    var validator = new PasswordValidator<RequestRegisterUserJson>();

    var context = new ValidationContext<RequestRegisterUserJson>(new RequestRegisterUserJson());
    var result = validator.IsValid(context, password);

    result.ShouldBeFalse();
    // result.Errors.ShouldHaveSingleItem();
    // result.Errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceErrorMessages.EMAIL_EMPTY));
  }
}
