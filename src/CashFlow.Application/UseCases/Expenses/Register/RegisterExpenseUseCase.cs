using CashFlow.Communication.Enums;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;

namespace CashFlow.Application.UseCases.Expenses.Register;

public class RegisterExpenseUseCase
{
  public ResponseRegisterExpenseJson Execute(RequestRegisterExpenseJson request)
  {
    Validate(request);

    return new ResponseRegisterExpenseJson();
  }

  private void Validate(RequestRegisterExpenseJson request)
  {
    var isTitleEmpty = string.IsNullOrWhiteSpace(request.Title);
    if (isTitleEmpty) throw new ArgumentException("The title is required");

    if (request.Amount <= 0) throw new ArgumentException("The amount must be greater than 0");

    var result = DateTime.Compare(request.Date, DateTime.UtcNow);
    if (result > 0) throw new ArgumentException("Expenses cannot be for the future");

    var isPaymentTypeValid = Enum.IsDefined(typeof(PaymentType), request.PaymentType);
    if (!isPaymentTypeValid) throw new ArgumentException("Payment Type is not valid");
  }

}
