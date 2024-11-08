using EquipmentAccounting.Classes;
using EquipmentAccounting.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace EquipmentAccounting.ViewModels
{
    internal class SoundCardAddEditViewModel : INotifyPropertyChanged
    {
        private bool _isSaveDialogOpen;
        private bool _isExitDialogOpen;
        private bool _isEditing;

        private string _selectedTechnology;
        private string _sendingDataSpeed;
        private string _connectionInterface;

        private Models.Component _currentComponent;
        private Models.ComponentCharacteristic _characteristic;
        public List<string> AvailableTechnologies => ConstLists.SOUND_CARD_TECHNOLOGIES;
        public bool IsSaveDialogOpen
        {
            get => _isSaveDialogOpen;
            set
            {
                _isSaveDialogOpen = value;
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

        public bool IsEditing
        {
            get => _isEditing;
            set
            {
                _isEditing = value;
                OnPropertyChanged();
            }
        }

        public string SelectedTechnology
        {
            get => _selectedTechnology;
            set
            {
                _selectedTechnology = value;
                OnPropertyChanged();
            }
        }

        public string SendingDataSpeed
        {
            get => _sendingDataSpeed;
            set
            {
                _sendingDataSpeed = value;
                OnPropertyChanged();
            }
        }

        public string ConnectionInterface
        {
            get => _connectionInterface;
            set
            {
                _connectionInterface = value;
                OnPropertyChanged();
            }
        }
        public Models.Component CurrentComponent
        {
            get => _currentComponent;
            set
            {
                if (value == null)
                {
                    _currentComponent = new Models.Component
                    {
                        ComponentTypeId = 11
                    };
                }
                else
                {
                    _currentComponent = value;
                    IsEditing = true;
                    LoadCharacteristics();
                    OnPropertyChanged();
                }
            }
        }
        public Models.ComponentCharacteristic Characteristic
        {
            get => _characteristic;
            set
            {
                _characteristic = value;
                OnPropertyChanged();
            }
        }

        public ICommand GoBackCommand => new RelayCommand(GoBack);
        public ICommand ExitCommand => new RelayCommand(Exit);
        public ICommand CloseDialogCommand => new RelayCommand(CloseDialog);
        public ICommand SaveCommand => new RelayCommand(OnSaveChanges);
        public ICommand OpenSaveDialogCommand => new RelayCommand(OpenSaveDialog);

        public SoundCardAddEditViewModel()
        {

        }
        private void OnSaveChanges()
        {
            if (IsEditing)
            {
                SetCharacteristics();

                try
                {
                    EquipmentEntities.GetContext().SaveChanges();
                    Exit();
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                try
                {
                    EquipmentEntities.GetContext().Component.Add(CurrentComponent);
                    EquipmentEntities.GetContext().SaveChanges();

                    SetCharacteristics();
                    Exit();
                }
                catch (Exception ex)
                {
                }
            }
        }
        private void CloseDialog()
        {
            IsExitDialogOpen = false;
            IsSaveDialogOpen = false;
        }
        private void Exit()
        {
            _isExitDialogOpen = false;
            _isSaveDialogOpen = false;
            Views.AdminViews.ComponentsPage componentsPage = new Views.AdminViews.ComponentsPage();
            Classes.Manager.MenuPage.CurrentPage = componentsPage;
        }
        private void GoBack()
        {
            IsExitDialogOpen = true;
        }

        private void OpenSaveDialog()
        {
            IsSaveDialogOpen = true;
        }
        private void SetCharacteristics()
        {
            if (CurrentComponent != null)
            {
                if (IsEditing)
                {
                    ClearCharacteristics();
                }

                if (!string.IsNullOrWhiteSpace(ConnectionInterface))
                {
                    var connectionInterfaceCharacteristic = new Models.ComponentCharacteristic
                    {
                        ComponentTypeCharacteristicId = 61,
                        ComponentId = CurrentComponent.Id,
                        Value = ConnectionInterface
                    };
                    AddCharacteristic(connectionInterfaceCharacteristic);
                }

                if (!string.IsNullOrWhiteSpace(SendingDataSpeed))
                {
                    var speedCharacteristic = new Models.ComponentCharacteristic
                    {
                        ComponentTypeCharacteristicId = 62,
                        ComponentId = CurrentComponent.Id,
                        Value = SendingDataSpeed
                    };
                    AddCharacteristic(speedCharacteristic);
                }

                if (!string.IsNullOrWhiteSpace(SelectedTechnology))
                {
                    var technologyCharacteristic = new Models.ComponentCharacteristic
                    {
                        ComponentTypeCharacteristicId = 63,
                        ComponentId = CurrentComponent.Id,
                        Value = SelectedTechnology
                    };
                    AddCharacteristic(technologyCharacteristic);
                }
            }
        }
        private void AddCharacteristic(Models.ComponentCharacteristic characteristic)
        {
            try
            {
                EquipmentEntities.GetContext().ComponentCharacteristic.Add(characteristic);
                EquipmentEntities.GetContext().SaveChanges();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }
        private void LoadCharacteristics()
        {
            if (CurrentComponent != null)
            {
                var characteristics = EquipmentEntities.GetContext().ComponentCharacteristic
                    .Where(cc => cc.ComponentId == CurrentComponent.Id).ToList();

                foreach (var characteristic in characteristics)
                {
                    switch (characteristic.ComponentTypeCharacteristicId)
                    {
                        case 61:
                            ConnectionInterface = characteristic.Value;
                            break;
                        case 62:
                            SendingDataSpeed = characteristic.Value;
                            break;
                        case 63:
                            SelectedTechnology = characteristic.Value;
                            break;
                    }
                }
            }
        }

        private void ClearCharacteristics()
        {
            try
            {
                var existingCharacteristics = EquipmentEntities.GetContext().ComponentCharacteristic
                    .Where(cc => cc.ComponentId == CurrentComponent.Id).ToList();
                EquipmentEntities.GetContext().ComponentCharacteristic.RemoveRange(existingCharacteristics);
                EquipmentEntities.GetContext().SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
