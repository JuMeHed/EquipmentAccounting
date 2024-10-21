using EquipmentAccounting.Classes;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Input;

namespace EquipmentAccounting.ViewModels
{
    internal class MenuPageViewModel : INotifyPropertyChanged
    {
        private bool _isProfileBtnChecked;
        private bool _isEquipmentBtnChecked;
        private bool _isComponentBtnChecked;
        private bool _isUsersBtnChecked;
        private Page _currentPage;
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
                        IsUsersBtnChecked = false;
                        IsComponentBtnChecked = false;
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
                        IsUsersBtnChecked = false;
                        IsComponentBtnChecked = false;
                    }
                }
            }
        }

        public bool IsComponentBtnChecked
        {
            get => _isComponentBtnChecked;
            set
            {
                if (_isComponentBtnChecked != value)
                {
                    _isComponentBtnChecked = value;
                    OnPropertyChanged();
                    if (_isComponentBtnChecked)
                    {
                        IsEquipmentBtnChecked = false;
                        IsUsersBtnChecked = false;
                        IsProfileBtnChecked = false;
                    }
                }
            }
        }

        public bool IsUsersBtnChecked
        {
            get => _isUsersBtnChecked;
            set
            {
                if (_isUsersBtnChecked != value)
                {
                    _isUsersBtnChecked = value;
                    OnPropertyChanged();
                    if (_isUsersBtnChecked)
                    {
                        IsEquipmentBtnChecked = false;
                        IsProfileBtnChecked = false;
                        IsComponentBtnChecked = false;
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

        public ICommand EquipmentCLickCommand { get; set; }
        public ICommand ProfileClickCommand { get; set; }
        public ICommand UsersClickCommand { get; set; }
        public ICommand ComponentsClickCommand { get; set; }

        public MenuPageViewModel()
        {
            Manager.MainViewModel.IsBorderVisible = true;

            EquipmentCLickCommand = new RelayCommand(EquipmentBtnClick);
            ProfileClickCommand = new RelayCommand(ProfileBtnClick);
            UsersClickCommand = new RelayCommand(UsersBtnClick);
            ComponentsClickCommand = new RelayCommand(ComponentsBtnClick);

            Page components = new Views.AdminViews.ComponentsPage();
            IsComponentBtnChecked = true;
            CurrentPage = components;
        }

        private void EquipmentBtnClick()
        {

        }

        private void ProfileBtnClick()
        {

        }

        private void UsersBtnClick()
        {

        }

        private void ComponentsBtnClick()
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
