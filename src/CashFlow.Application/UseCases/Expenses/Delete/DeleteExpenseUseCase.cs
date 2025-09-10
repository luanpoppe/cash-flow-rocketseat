
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.Delete;

public class DeleteExpenseUseCase : IDeleteExpenseUseCase
{
  private readonly IUnityOfWork _unityOfWork;
  private readonly IExpensesWriteOnlyRepository _repository;
  public DeleteExpenseUseCase(IExpensesWriteOnlyRepository repository, IUnityOfWork unityOfWork)
  {
    _repository = repository;
    _unityOfWork = unityOfWork;
  }
  public async Task Execute(long id)
  {
    var result = await _repository.Delete(id);

    if (result == false)
    {
      throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);
    }

    await _unityOfWork.Commit();
  }
}
