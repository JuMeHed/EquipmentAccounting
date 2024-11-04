using EquipmentAccounting.Classes;
using EquipmentAccounting.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace EquipmentAccounting.ViewModels
{
    internal class ProfileViewModel : INotifyPropertyChanged
    {
        private string _introduceName;

        private Models.User _currentUser;
        private ObservableCollection<Location> _usersLocations;

        private bool _isExitDialogOpen;

        public string IntroduceName
        {
            get => _introduceName;
            set
            {
                _introduceName = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Location> UsersLocations
        {
            get => _usersLocations;
            set
            {
                _usersLocations = value;
            }
        }

        public Models.User CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                OnPropertyChanged();
            }
        }

        public bool IsExitDialogOpen
        {
            get => _isExitDialogOpen;
            set
            {
                _isExitDialogOpen = value;
                OnPropertyChanged();
            }
        }

        public ICommand ExitDialogOpenCommand => new RelayCommand(ExitDialogOpen);
        public ICommand ExitDialogCloseCommand => new RelayCommand(ExitDialogClose);
        public ICommand ExitCommand => new RelayCommand(Exit);

        public ProfileViewModel()
        {
            CurrentUser = Classes.User.CurrentUser;
            IntroduceName = "Вы зашли под логином: " + CurrentUser.Login;
            LoadUsersLocations();
        }

        private void ExitDialogOpen()
        {
            IsExitDialogOpen = true;
        }

        private void ExitDialogClose()
        {
            IsExitDialogOpen = false;
        }
        private void Exit()
        {
            Classes.User.CurrentUser = null;
            Properties.Settings.Default.CurrentUser = string.Empty; 
            Properties.Settings.Default.Save();

            Manager.CurrentPage = null;
            Manager.UserMenu = null;
            Manager.MenuPage = null;
            Views.LoginPage loginPage = new Views.LoginPage();
            Manager.MainViewModel.CurrentPage = loginPage;
        }

        private void LoadUsersLocations()
        {
            var locations = new ObservableCollection<Location>(EquipmentEntities.GetContext().Location.Where(x => x.ResponsibleId == CurrentUser.Id));
            UsersLocations = locations;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
