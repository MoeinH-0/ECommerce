using _0_Framework.Application;
using AccountManagement.Application.Contracts.Account;
using AccountManagement.Domain.AccountAgg;
using AccountManagement.Domain.RoleAgg;

namespace AccountManagement.Application;

public class AccountApplication : IAccountApplication
{
    private readonly IAccountRepository _accountRepository;
    private readonly IAuthHelper _authHelper;
    private readonly IFileUploader _fileUploader;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IRoleRepository _roleRepository;

    public AccountApplication(IAccountRepository accountRepository, IPasswordHasher passwordHasher,
        IFileUploader fileUploader, IAuthHelper authHelper, IRoleRepository roleRepository)
    {
        _accountRepository = accountRepository;
        _passwordHasher = passwordHasher;
        _fileUploader = fileUploader;
        _authHelper = authHelper;
        _roleRepository = roleRepository;
    }

    public AccountViewModel GetAccountBy(long id)
    {
        var account = _accountRepository.Get(id);
        return new AccountViewModel
        {
            FullName = account.FullName,
            Mobile = account.Mobile
        };
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

        var permissions = _roleRepository.Get(account.RoleId)!
            .Permissions
            .Select(x => x.Code)
            .ToList();

        var authViewModel = new AuthViewModel(account.Id, account.RoleId,
            account.FullName, account.Username, account.Mobile, permissions);

        _authHelper.Signin(authViewModel);

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

    public List<AccountViewModel> GetAccounts()
    {
        return _accountRepository.GetAccounts();
    }

    public List<AccountViewModel> Search(AccountSearchModel searchModel)
    {
        return _accountRepository.Search(searchModel);
    }
}