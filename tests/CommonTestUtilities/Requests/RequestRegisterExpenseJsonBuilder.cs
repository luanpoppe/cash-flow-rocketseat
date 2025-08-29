
using Bogus;
using CashFlow.Communication.Enums;
using CashFlow.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestRegisterExpenseJsonBuilder
{
  public RequestRegisterExpenseJson Build()
  {
    return new Faker<RequestRegisterExpenseJson>()
      .RuleFor(r => r.Title, f => f.Commerce.ProductName())
      .RuleFor(r => r.Description, f => f.Commerce.ProductDescription())
      .RuleFor(r => r.Date, f => f.Date.Past())
      .RuleFor(r => r.PaymentType, f => f.PickRandom<PaymentType>())
      .RuleFor(r => r.Amount, f => f.Random.Decimal());
  }
}
