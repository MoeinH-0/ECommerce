using AccountManagement.Application.Contracts.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Pages;

public class AccountModel : PageModel
{
    private readonly IAccountApplication _accountApplication;

    public AccountModel(IAccountApplication accountApplication)
    {
        _accountApplication = accountApplication;
    }

    public string? LoginMessage { get; set; }
    public string? RegisterMessage { get; set; }

    [BindProperty] 
    public Login LoginCommand { get; set; }

    [BindProperty] 
    public RegisterAccount RegisterCommand { get; set; }

    public void OnGet()
    {
    }

    public IActionResult OnPostLogin()
    {
        var result = _accountApplication.Login(LoginCommand);
        if (result.IsSucceeded)
            return RedirectToPage("/Index");

        LoginMessage = result.Message;
        return Page();
    }

    public IActionResult OnGetLogout()
    {
        _accountApplication.Logout();
        return RedirectToPage("/Index");
    }

    public IActionResult OnPostRegister()
    {
        var result = _accountApplication.Register(RegisterCommand);
        if (result.IsSucceeded)
            return RedirectToPage("/Index");
            
        RegisterMessage = result.Message;
        return Page();
    }
}