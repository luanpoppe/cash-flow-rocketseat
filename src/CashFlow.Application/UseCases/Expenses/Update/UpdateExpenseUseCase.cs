using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.Update;

public class UpdateExpenseUseCase : IUpdateExpenseUseCase
{
  private readonly IMapper _mapper;
  private readonly IUnityOfWork _unityOfWord;
  private readonly IExpensesUpdateOnlyRepository _repository;

  public UpdateExpenseUseCase(IMapper mapper, IUnityOfWork unityOfWork, IExpensesUpdateOnlyRepository repository)
  {
    _mapper = mapper;
    _unityOfWord = unityOfWork;
    _repository = repository;
  }

  public async Task Execute(long id, RequestExpenseJson request)
  {
    Validate(request);

    var expense = await _repository.GetById(id);
    if (expense is null)
    {
      throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);
    }

    _mapper.Map(request, expense);

    _repository.Update(expense);

    await _unityOfWord.Commit();
  }

  private void Validate(RequestExpenseJson request)
  {
    var validator = new ExpenseValidator();
    var result = validator.Validate(request);

    if (result.IsValid == false)
    {
      var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
      throw new ErrorOnValidationException(errorMessages);
    }
  }
}
