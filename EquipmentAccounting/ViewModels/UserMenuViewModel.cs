using EquipmentAccounting.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace EquipmentAccounting.ViewModels
{
    internal class UserMenuViewModel : INotifyPropertyChanged
    {
        private Page _currentPage;

        private bool _isEquipmentBtnChecked;
        private bool _isProfileBtnChecked;

        public bool IsEquipmentBtnChecked
        {
            get => _isEquipmentBtnChecked;
            set
            {
                if (_isEquipmentBtnChecked != value)
                {
                    _isEquipmentBtnChecked = value;
                    OnPropertyChanged();
                    if (_isEquipmentBtnChecked)
                    {
                        IsProfileBtnChecked = false;
                    }
                }
            }
        }

        public bool IsProfileBtnChecked 
        {
            get => _isProfileBtnChecked;
            set
            {
                if (_isProfileBtnChecked != value) 
                {
                    _isProfileBtnChecked = value;
                    OnPropertyChanged();
                    if (_isProfileBtnChecked)
                    {
                        IsEquipmentBtnChecked = false;
                    }
                }
            }
        }

        public Page CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                OnPropertyChanged();
            }
        }
        public ICommand EquipmentCLickCommand => new RelayCommand(EquipmentBtnCLick);
        public ICommand ProfileClickCommand => new RelayCommand(ProfileBtnClick);

        public UserMenuViewModel()
        {
            Manager.UserMenu = this;
            Manager.MainViewModel.IsBorderVisible = true;
        }

        private void EquipmentBtnCLick()
        {

        }

        private void ProfileBtnClick()
        {
            Views.AdminViews.ProfileView profile = new Views.AdminViews.ProfileView();
            ProfileViewModel viewModel = new ProfileViewModel();
            profile.DataContext = viewModel;
            Manager.UserMenu.CurrentPage = profile;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
