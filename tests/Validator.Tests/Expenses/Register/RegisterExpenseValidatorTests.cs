using CashFlow.Application.UseCases.Expenses.Register;
using CommonTestUtilities.Requests;
using Shouldly;

namespace Validator.Tests.Expenses.Register;

public class RegisterExpenseValidatorTests
{
  [Fact]
  public void Success()
  {
    var validator = new RegisterExpenseValidator();
    var request = RequestRegisterExpenseJsonBuilder.Build();

    var result = validator.Validate(request);

    result.IsValid.ShouldBeTrue();
  }
}
