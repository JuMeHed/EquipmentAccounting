using EquipmentAccounting.Classes;
using EquipmentAccounting.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace EquipmentAccounting.ViewModels
{
    internal class UsersViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Models.User> _users;
        private ObservableCollection<Models.User> _filteredUsers;
        private ObservableCollection<AccessLevel> _accessLevels;
        private ObservableCollection<AccessLevel> _editAccessLevels;
        private ObservableCollection<Location> _locations;
        private ObservableCollection<Location> _usersLocations;

        private Models.User _selectedUser;
        private AccessLevel _selectedAccessLevel;
        private AccessLevel _editSelectedLevel;
        private Location _selectedLocation;

        private string _fullnameFilter;
        private string _dialogTitle;

        private bool _isDeleteDialogOpen;
        private bool _isEditDialogOpen;
        public ObservableCollection<Models.User> Users
        {
            get => _users;
            set
            {
                _users = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Models.User> FilteredUsers
        {
            get => _filteredUsers;
            set
            {
                _filteredUsers = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<AccessLevel> AccessLevels
        {
            get => _accessLevels;
            set
            {
                _accessLevels = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<AccessLevel> EditAccessLevels
        {
            get => _editAccessLevels;
            set
            {
                _editAccessLevels = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Location> Locations
        {
            get => _locations;
            set
            {
                _locations = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Location> UsersLocations
        {
            get => _usersLocations;
            set
            {
                _usersLocations = value;
                OnPropertyChanged();
            }
        }
        public Models.User SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged();
            }
        }

        public AccessLevel SelectedAccessLevel
        {
            get => _selectedAccessLevel;
            set
            {
                _selectedAccessLevel = value;
                OnPropertyChanged();
                FilterUsers();
            }
        }

        public AccessLevel EditSelectedLevel
        {
            get => _editSelectedLevel;
            set
            {
                _editSelectedLevel = value;
                OnPropertyChanged();
            }
        }

        public Location SelectedLocation
        {
            get => _selectedLocation;
            set
            {
                _selectedLocation = value;
                OnPropertyChanged();
            }
        }
        public string FullNameFilter
        {
            get => _fullnameFilter;
            set
            {
                _fullnameFilter = value;
                OnPropertyChanged();
                FilterUsers();
            }
        }

        public string DialogTitle
        {
            get => _dialogTitle;
            set
            {
                _dialogTitle = value;
                OnPropertyChanged();
            }
        }
        public bool IsDeleteDialogOpen
        {
            get => _isDeleteDialogOpen;
            set
            {
                _isDeleteDialogOpen = value;
                OnPropertyChanged();
            }
        }

        public bool IsEditDialogOpen
        {
            get => _isEditDialogOpen;
            set
            {
                _isEditDialogOpen = value;
                OnPropertyChanged();
            }
        }

        public ICommand DeleteDialogOpenCommand => new RelayCommand<Models.User>(DeleteDialogOpen);
        public ICommand DialogCloseCommand => new RelayCommand(DialogClose);
        public ICommand DeleteUserCommand => new RelayCommand(DeleteUser);
        public ICommand EditDialogOpenCommand => new RelayCommand<Models.User>(OpenEditDialog);
        public ICommand AddDialogOpenCommand => new RelayCommand(OpenAddDialog);
        public ICommand SaveChangesCommand => new RelayCommand(SaveChanges);
        public ICommand DeleteResponsobilityCommand => new RelayCommand<Location>(DeleteResponsibility);
        public ICommand AssignRoomCommand => new RelayCommand(AddResponsibility);
        public UsersViewModel()
        {
            LoadUsers();
            LoadAccessLevels();
            FilterUsers();
            LoadLocations();
        }

        private void LoadUsers()
        {
            var users = new ObservableCollection<Models.User>(EquipmentEntities.GetContext().User.ToList());
            Users = users;
        }

        private void LoadLocations()
        {
            var locations = new ObservableCollection<Models.Location>(EquipmentEntities.GetContext().Location.ToList());
            Locations = locations;
        }

        private void LoadUsersLocations()
        {
            var usersLocations = Locations.Where(x => x.ResponsibleId == SelectedUser.Id);
            UsersLocations = new ObservableCollection<Location>(usersLocations);
        }
        private void LoadAccessLevels()
        {
            var accessLevels = new ObservableCollection<AccessLevel>(EquipmentEntities.GetContext().AccessLevel.ToList());
            EditAccessLevels = new ObservableCollection<AccessLevel>(EquipmentEntities.GetContext().AccessLevel.ToList());
            accessLevels.Insert(0, new AccessLevel { Description = "Все типы" });
            AccessLevels = accessLevels;
            SelectedAccessLevel = AccessLevels[0];
        }

        private void DeleteUser()
        {
            try
            {
                EquipmentEntities.GetContext().User.Remove(SelectedUser);
                EquipmentEntities.GetContext().SaveChanges();
                DialogClose();
                LoadUsers();
                FilterUsers();
            }
            catch
            {
                MessageBox.Show("Произошла непредвиденная ошибка.");
            }
        }

        private void DeleteDialogOpen(Models.User user)
        {
            SelectedUser = user;
            IsDeleteDialogOpen = true;
        }

        private void DialogClose()
        {
            IsDeleteDialogOpen = false;
            IsEditDialogOpen = false;
        }

        private void OpenEditDialog(Models.User user)
        {
            SelectedUser = user;
            DialogTitle = "Редактирование пользователя";
            LoadUsersLocations();
            IsEditDialogOpen = true;
        }

        public void OpenAddDialog()
        {
            DialogTitle = "Добавление пользователя";
            SelectedUser = new Models.User();
            LoadUsersLocations();
            IsEditDialogOpen = true;
        }

        private void SaveChanges()
        {
            try
            {
                if (DialogTitle != "Редактирование пользователя")
                {
                    EquipmentEntities.GetContext().User.Add(SelectedUser);
                    EquipmentEntities.GetContext().SaveChanges();

                    foreach (Location location in UsersLocations)
                    {
                        location.ResponsibleId = SelectedUser.Id;
                    }
                }

                EquipmentEntities.GetContext().SaveChanges();
                LoadUsers();
                FilterUsers();
                DialogClose();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла непредвиденная ошибка." + "\n" + ex.Message);
            }
        }

        private void FilterUsers()
        {
            try
            {
                if (Users == null || Users.Count == 0)
                {
                    MessageBox.Show("пусто");
                    FilteredUsers = new ObservableCollection<Models.User>();
                    return;
                }

                var filteredUsers = Users.AsEnumerable();

                if (!string.IsNullOrEmpty(FullNameFilter))
                {
                    filteredUsers = filteredUsers.Where(x => x.FullName.ToLower().Contains(FullNameFilter.ToLower()));
                }

                if (SelectedAccessLevel != null && SelectedAccessLevel != AccessLevels[0])
                {
                    filteredUsers = filteredUsers.Where(x => x.AccessLevel == SelectedAccessLevel);
                }

                FilteredUsers = new ObservableCollection<Models.User>(filteredUsers);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла непредвиденная ошибка." + "\n" + ex.Message);
            }
        }

        private void DeleteResponsibility(Location location)
        {
            try
            {
                location.ResponsibleId = null;
                EquipmentEntities.GetContext().SaveChanges();
                LoadUsersLocations();
            }
            catch
            {

            }
        }

        private void AddResponsibility()
        {
            if (SelectedUser.Id != 0)
            {
                if (SelectedLocation != null)
                {
                    SelectedLocation.ResponsibleId = SelectedUser.Id;
                    LoadUsersLocations();
                }
            } else
            {
                UsersLocations.Add(SelectedLocation);
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
