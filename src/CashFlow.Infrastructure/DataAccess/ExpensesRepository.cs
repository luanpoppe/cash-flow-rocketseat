using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;

namespace CashFlow.Infrastructure.DataAccess;

internal class ExpensesRepository : IExpensesRepository
{
  public void Add(Expense expense)
  {
    var dbContext = new CashFlowDbContext();

    dbContext.Expenses.Add(expense);
  }
}
