using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using EquipmentAccounting.Classes;
using EquipmentAccounting.Models;

namespace EquipmentAccounting.ViewModels
{
    internal class UsersViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Models.User> _users;
        private ObservableCollection<AccessLevel> _accessLevels;
        private Models.User _selectedUser;
        private AccessLevel _selectedAccessLevel;
        private string _fullnameFilter;

        private bool _isDeleteDialogOpen;
        
        public ObservableCollection<Models.User> Users
        {
            get => _users;
            set
            {
                _users = value;
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
            }
        }
        public string FullNameFilter
        {
            get => _fullnameFilter;
            set
            {
                _fullnameFilter = value;
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

        public ICommand DeleteDialogOpenCommand => new RelayCommand(DeleteDialogOpen);
        public ICommand DeleteDialogCloseCommand => new RelayCommand(DeleteDialogClose);
        public ICommand DeleteUserCommand => new RelayCommand(DeleteUser);
        public UsersViewModel()
        {
            LoadUsers();
            LoadAccessLevels();
        }

        private void LoadUsers()
        {
            var users = new ObservableCollection<Models.User>(EquipmentEntities.GetContext().User.ToList());
            Users = users;
        }

        private void LoadAccessLevels()
        {
            var accessLevels = new ObservableCollection<AccessLevel>(EquipmentEntities.GetContext().AccessLevel.ToList());
            accessLevels.Insert(0, new AccessLevel { Description = "Все типы" });
            AccessLevels = accessLevels;
        }

        private void DeleteUser()
        {
            if (SelectedUser != null)
            {
                try
                {
                    EquipmentEntities.GetContext().User.Remove(SelectedUser);
                    EquipmentEntities.GetContext().SaveChanges();
                } catch (Exception ex)
                {

                }
            }
        }

        private void DeleteDialogOpen()
        {
            MessageBox.Show("Але");
            IsDeleteDialogOpen = true;
        }

        private void DeleteDialogClose()
        {
            IsDeleteDialogOpen = false;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
