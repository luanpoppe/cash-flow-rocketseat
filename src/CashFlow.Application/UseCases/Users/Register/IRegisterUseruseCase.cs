using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;

namespace CashFlow.Application.UseCases.Users.Register;

public interface IRegisterUseruseCase
{
  public Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request);
}
