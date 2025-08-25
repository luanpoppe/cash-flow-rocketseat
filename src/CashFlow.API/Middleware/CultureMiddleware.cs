using System.Globalization;

namespace CashFlow.API.Middleware;

public class CultureMiddleware
{
  private readonly RequestDelegate _next;

  public CultureMiddleware(RequestDelegate next)
  {
    _next = next;
  }

  public async Task Invoke(HttpContext context)
  {
    var supportedLanguages = CultureInfo.GetCultures(CultureTypes.AllCultures).ToList();

    var requestedCulture = context.Request.Headers.AcceptLanguage.FirstOrDefault();

    var cultureInfo = new CultureInfo("en");

    var isValidString = !string.IsNullOrWhiteSpace(requestedCulture);
    var isSupportedLanguage = supportedLanguages.Exists(l => l.Name.Equals(requestedCulture));

    if (isValidString && isSupportedLanguage)
    {
      cultureInfo = new CultureInfo(requestedCulture);
    }

    CultureInfo.CurrentCulture = cultureInfo;
    CultureInfo.CurrentUICulture = cultureInfo;

    await _next(context);
  }
}
