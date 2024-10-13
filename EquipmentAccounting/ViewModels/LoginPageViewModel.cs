using EquipmentAccounting.Classes;
using EquipmentAccounting.Models;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace EquipmentAccounting.ViewModels
{
    internal class LoginPageViewModel : INotifyPropertyChanged
    {
        private string _password;
        private string _login;
        private bool _isLoginExists;
        private bool _isPasswordValid;

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public string Login
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged();
                IsLoginExists = !CheckUserExisting(_login);
            }
        }

        public bool IsLoginExists
        {
            get => _isLoginExists;
            set
            {
                _isLoginExists = value;
                OnPropertyChanged();
            }
        }

        public bool IsPasswordValid
        {
            get => _isPasswordValid;
            set
            {
                _isPasswordValid = !value;
                OnPropertyChanged();
            }
        }

        public ICommand LoginCommand {get;set;}
        public LoginPageViewModel()
        {
            LoginCommand = new RelayCommand(LogIn);
        }

        public void LogIn()
        {
            if (CheckUserExisting(Login))
            {
                var user = EquipmentEntities.GetContext().User.FirstOrDefault(x => x.Login == Login);

                if (Password != user.Password)
                {
                    IsPasswordValid = false;
                    return;
                } else
                {
                    Classes.User.CurrentUser = user;
                    MessageBox.Show("чики");
                }
            }
        }

        private static bool CheckUserExisting(string login)
        {
            var user = EquipmentEntities.GetContext().User.FirstOrDefault(x => x.Login == login);
            return user != null;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
