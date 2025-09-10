using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories.Expenses;

public interface IExpensesWriteOnlyRepository
{
  Task Add(Expense expense);

  /// <summary>
  /// This functions returns TRUE if the deletion was successfull
  /// </summary>
  /// <param name="id"></param>
  /// <returns></returns>
  Task<bool> Delete(long id);
}
