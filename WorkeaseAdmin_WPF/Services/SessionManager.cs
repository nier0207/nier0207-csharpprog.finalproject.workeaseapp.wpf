using WorkeaseAdmin_WPF.Models;

namespace WorkeaseAdmin_WPF.Services
{
    public class SessionManager
    {
        private string _token = string.Empty;
        private string _userName = string.Empty;
        private string _userType = string.Empty;
        private int _userId = 0;
        private UserProfile? _profile = null; // ✅ store full profile

        public bool IsLoggedIn => !string.IsNullOrEmpty(_token);

        public void SaveSession(LoginResponse response)
        {
            _token = response.Token;
            _userName = response.UserName;
            _userType = response.UserType;
            _userId = response.UserId;
        }

        // ✅ Save full profile after fetching
        public void SaveProfile(UserProfile profile) =>
            _profile = profile;

        public UserProfile? GetProfile() => _profile;
        public string GetToken() => _token;
        public string GetUserName() => _userName;
        public string GetUserType() => _userType;
        public int GetUserId() => _userId;

        public void ClearSession()
        {
            _token = string.Empty;
            _userName = string.Empty;
            _userType = string.Empty;
            _userId = 0;
            _profile = null;
        }
    }
}