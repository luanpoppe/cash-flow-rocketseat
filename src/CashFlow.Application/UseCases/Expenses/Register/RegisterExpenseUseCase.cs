using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Enums;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.Register;

public class RegisterExpenseUseCase : IRegisterExpenseUseCase
{
  private readonly IExpensesRepository _repository;
  private readonly IUnityOfWork _unityOfWork;

  public RegisterExpenseUseCase(IExpensesRepository repository, IUnityOfWork unityOfWork)
  {
    _repository = repository;
    _unityOfWork = unityOfWork;
  }

  public async Task<ResponseRegisterExpenseJson> Execute(RequestRegisterExpenseJson request)
  {
    Validate(request);

    var entity = new Expense
    {
      Amount = request.Amount,
      Date = request.Date,
      Description = request.Description,
      Title = request.Title,
      PaymentType = (PaymentType)request.PaymentType,
    };

    await _repository.Add(entity);
    await _unityOfWork.Commit();

    return new ResponseRegisterExpenseJson();
  }

  private void Validate(RequestRegisterExpenseJson request)
  {
    var validator = new RegisterExpenseValidator();
    var result = validator.Validate(request);

    if (!result.IsValid)
    {
      var errorMessages = result.Errors.Select(f => f.ErrorMessage).ToList();

      throw new ErrorOnValidationException(errorMessages);
    }
  }

}
