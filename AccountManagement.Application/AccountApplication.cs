using _0_Framework.Application;
using AccountManagement.Application.Contracts.Account;
using AccountManagement.Domain.AccountAgg;

namespace AccountManagement.Application;

public class AccountApplication : IAccountApplication
{
    private readonly IAccountRepository _accountRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IFileUploader _fileUploader;
    private readonly IAuthHelper _authHelper;

    public AccountApplication(IAccountRepository accountRepository, IPasswordHasher passwordHasher,
        IFileUploader fileUploader, IAuthHelper authHelper)
    {
        _accountRepository = accountRepository;
        _passwordHasher = passwordHasher;
        _fileUploader = fileUploader;
        _authHelper = authHelper;
    }

    public OperationResult Register(RegisterAccount command)
    {
        var operation = new OperationResult();

        if (_accountRepository.Exists(x => x.Username == command.UserName
                                           || x.Mobile == command.Mobile))
            return operation.Failed(ApplicationMessages.DuplicatedRecord);

        var passwordHash = _passwordHasher.Hash(command.Password);
        var path = "profilePhotos";
        var picturePath = _fileUploader.Upload(command.ProfilePhoto, path);

        var account = new Account(command.FullName, command.UserName,
            passwordHash, command.Mobile, command.RoleId, picturePath);

        _accountRepository.Create(account);
        _accountRepository.SaveChanges();

        return operation.Succeeded();
    }

    public OperationResult Edit(EditAccount command)
    {
        var operation = new OperationResult();
        var account = _accountRepository.Get(command.Id);

        if (account == null)
            return operation.Failed(ApplicationMessages.RecordNotFound);

        if (_accountRepository.Exists(x => (x.Username == command.UserName
                                            || x.Mobile == command.Mobile) && x.Id != command.Id))
            return operation.Failed(ApplicationMessages.DuplicatedRecord);

        var path = "profilePhotos";
        var picturePath = _fileUploader.Upload(command.ProfilePhoto, path);

        account.Edit(command.FullName, command.UserName,
            command.Mobile, command.RoleId, picturePath);

        _accountRepository.SaveChanges();

        return operation.Succeeded();
    }

    public OperationResult ChangePassword(ChangePassword command)
    {
        var operation = new OperationResult();
        var account = _accountRepository.Get(command.Id);

        if (account == null)
            return operation.Failed(ApplicationMessages.RecordNotFound);

        if (command.Password != command.RePassword)
            return operation.Failed(ApplicationMessages.PasswordsNotMatch);

        var passwordHash = _passwordHasher.Hash(command.Password);
        account.ChangePassword(passwordHash);

        var authViewModel = new AuthViewModel(account.Id, account.RoleId
            , account.FullName, account.Username, account.Mobile, []);

        _authHelper.Signin(authViewModel);
        
        _accountRepository.SaveChanges();

        return operation.Succeeded();
    }

    public OperationResult Login(Login command)
    {
        var operation = new OperationResult();
        var account = _accountRepository.GetByUsername(command.Username);

        if (account == null)
            return operation.Failed(ApplicationMessages.WrongUsername);

        if (!_passwordHasher.Check(account.Password, command.Password).Verified)
            return operation.Failed(ApplicationMessages.WrongUserPass);

        return operation.Succeeded();
    }

    public void Logout()
    {
        _authHelper.SignOut();
    }

    public EditAccount? GetDetails(long id)
    {
        return _accountRepository.GetDetails(id);
    }

    public List<AccountViewModel> Search(AccountSearchModel searchModel)
    {
        return _accountRepository.Search(searchModel);
    }
}