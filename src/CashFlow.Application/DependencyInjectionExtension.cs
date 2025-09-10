using CashFlow.Application.AutoMapper;
using CashFlow.Application.UseCases.Expenses.Delete;
using CashFlow.Application.UseCases.Expenses.GetAll;
using CashFlow.Application.UseCases.Expenses.GetById;
using CashFlow.Application.UseCases.Expenses.Register;
using Microsoft.Extensions.DependencyInjection;

namespace CashFlow.Application;

public static class DependencyInjectionExtension
{
  public static void AddApplication(this IServiceCollection services)
  {
    AddUseCases(services);
    AddAutoMapper(services);
  }

  private static void AddAutoMapper(IServiceCollection services)
  {
    services.AddAutoMapper(cfg => cfg.AddProfile<AutoMapping>());
  }

  private static void AddUseCases(IServiceCollection services)
  {
    services.AddScoped<IRegisterExpenseUseCase, RegisterExpenseUseCase>();
    services.AddScoped<IGetAllExpensesUseCase, GetAllExpensesUseCase>();
    services.AddScoped<IGetExpenseByIdUseCase, GetExpenseByIdUseCase>();
    services.AddScoped<IDeleteExpenseUseCase, DeleteExpenseUseCase>();
  }
}
