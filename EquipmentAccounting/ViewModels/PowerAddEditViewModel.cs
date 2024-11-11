using EquipmentAccounting.Classes;
using EquipmentAccounting.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace EquipmentAccounting.ViewModels
{
    internal class PowerAddEditViewModel : INotifyPropertyChanged
    {
        private bool _isSaveDialogOpen;
        private bool _isExitDialogOpen;
        private bool _isEditing;
        private bool _isMessageOpen;

        private string _power;
        private string _selectedFormFactor;
        private string _selectedCertification;
        private string _connectors;
        private string _size;
        private string _message;

        private Models.Component _currentComponent;
        private Models.ComponentCharacteristic _characteristic;

        public List<string> AvailableFormFactors => ConstLists.POWER_SUPPLY_FORM_FACTORS;
        public List<string> AvailableCertifications => ConstLists.POWER_SUPPLY_CERTIFICATIONS;
        public bool IsMessageOpen
        {
            get => _isMessageOpen;
            set
            {
                _isMessageOpen = value;
                OnPropertyChanged();
            }
        }
        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged();
            }
        }
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

        public string Power
        {
            get => _power;
            set
            {
                _power = value;
                OnPropertyChanged();
            }
        }

        public string SelectedFormFactor
        {
            get => _selectedFormFactor;
            set
            {
                _selectedFormFactor = value;
                OnPropertyChanged();
            }
        }

        public string SelectedCertification
        {
            get => _selectedCertification;
            set
            {
                _selectedCertification = value;
                OnPropertyChanged();
            }
        }

        public string Connectors
        {
            get => _connectors;
            set
            {
                _connectors = value;
                OnPropertyChanged();
            }
        }

        public string Size
        {
            get => _size;
            set
            {
                _size = value;
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
                        ComponentTypeId = 9
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
        public PowerAddEditViewModel()
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
                    CloseDialog();
                }
                catch 
                {
                    DisplayMessage("Не удалось сохранить. Проверьте правильность заполнения полей.");
                    return;
                }
            }
            else
            {
                SetCharacteristics();

                try
                {
                    EquipmentEntities.GetContext().Component.Add(CurrentComponent);
                    EquipmentEntities.GetContext().SaveChanges();
                    CloseDialog();
                }
                catch 
                {
                    DisplayMessage("Не удалось сохранить. Проверьте правильность заполнения полей.");
                    return;
                }
            }
            DisplayMessage("Данные сохранены.");
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

                if (!string.IsNullOrWhiteSpace(Power))
                {
                    var powerCharacteristic = new Models.ComponentCharacteristic
                    {
                        ComponentTypeCharacteristicId = 52,
                        ComponentId = CurrentComponent.Id,
                        Value = Power
                    };
                    AddCharacteristic(powerCharacteristic);
                }

                if (!string.IsNullOrWhiteSpace(Size))
                {
                    var sizeCharacteristic = new Models.ComponentCharacteristic
                    {
                        ComponentTypeCharacteristicId = 54,
                        ComponentId = CurrentComponent.Id,
                        Value = Size
                    };
                    AddCharacteristic(sizeCharacteristic);
                }

                if (!string.IsNullOrWhiteSpace(SelectedFormFactor))
                {
                    var formFactorCharacteristic = new Models.ComponentCharacteristic
                    {
                        ComponentTypeCharacteristicId = 55,
                        ComponentId = CurrentComponent.Id,
                        Value = SelectedFormFactor
                    };
                    AddCharacteristic(formFactorCharacteristic);
                }

                if (!string.IsNullOrWhiteSpace(SelectedCertification))
                {
                    var certificationCharacteristic = new Models.ComponentCharacteristic
                    {
                        ComponentTypeCharacteristicId = 56,
                        ComponentId = CurrentComponent.Id,
                        Value = SelectedCertification
                    };
                    AddCharacteristic(certificationCharacteristic);
                }

                if (!string.IsNullOrWhiteSpace(Connectors))
                {
                    var connectorsCharacteristic = new Models.ComponentCharacteristic
                    {
                        ComponentTypeCharacteristicId = 57,
                        ComponentId = CurrentComponent.Id,
                        Value = Connectors
                    };
                    AddCharacteristic(connectorsCharacteristic);
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
                        case 52:
                            Power = characteristic.Value;
                            break;
                        case 54:
                            Size = characteristic.Value;
                            break;
                        case 55:
                            SelectedFormFactor = characteristic.Value;
                            break;
                        case 56:
                            SelectedCertification = characteristic.Value;
                            break;
                        case 57:
                            Connectors = characteristic.Value;
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
            catch
            {
            }
        }

        private async Task DisplayMessage(string message)
        {
            Message = message;
            IsMessageOpen = true;

            await Task.Delay(3000);


            IsMessageOpen = false;
            Message = "";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
