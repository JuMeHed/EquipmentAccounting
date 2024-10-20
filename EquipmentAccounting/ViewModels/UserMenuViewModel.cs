using EquipmentAccounting.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace EquipmentAccounting.ViewModels
{
    internal class UserMenuViewModel : INotifyPropertyChanged
    {
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

        public ICommand EquipmentCLickCommand { get; set; }
        public ICommand ProfileClickCommand { get; set; }

        public UserMenuViewModel()
        {

            EquipmentCLickCommand = new RelayCommand(EquipmentBtnCLick);
            ProfileClickCommand = new RelayCommand(ProfileBtnClick);
        }

        private void EquipmentBtnCLick()
        {

        }

        private void ProfileBtnClick()
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
