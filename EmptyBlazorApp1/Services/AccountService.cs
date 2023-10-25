using EmptyBlazorApp1.Entities;

namespace EmptyBlazorApp1.Services;

public class AccountService {
    public User? User {
        get {
            if (_user is not null) {
                return _user;
            }

            if (_authenticationService.IsAuthorized()) {
                _user = _authenticationService.GetUserWithProfile();
                return _user;
            }

            return null;
        }
    }

    private User?                 _user = null;
    private AuthenticationService _authenticationService;
    private DbService             _dbService;

    public AccountService(AuthenticationService authenticationService, DbService dbService) {
        _authenticationService = authenticationService;
        _dbService             = dbService;
    }
    
    public List<Community> GetCommunities(User user) {
        _dbService.LoadCommunities(user);
        return user.Communities;
    }
}