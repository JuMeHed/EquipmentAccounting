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
    internal class CaseAddEditViewModel : INotifyPropertyChanged
    {
        private bool _isSaveDialogOpen;
        private bool _isExitDialogOpen;
        private bool _isEditing;

        private string _selectedFormFactor;
        private string _size;
        private int _countOfFan;
        private string _connectors;

        private Models.Component _currentComponent;
        private Models.ComponentCharacteristic _characteristic;

        public List<string> AvailableFormFactors => ConstLists.CASE_FORM_FACTORS;
        public List<int> AvailableCountOfFans => ConstLists.NUMBER_OF_SATA;
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

        public string SelectedFormFactor
        {
            get => _selectedFormFactor;
            set
            {
                _selectedFormFactor = value;
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

        public int CountOfFan
        {
            get => _countOfFan;
            set
            {
                _countOfFan = value;
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
        public Models.Component CurrentComponent
        {
            get => _currentComponent;
            set
            {
                if (value == null)
                {
                    _currentComponent = new Models.Component
                    {
                        ComponentTypeId = 4
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
        public CaseAddEditViewModel()
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
                SetCharacteristics();

                try
                {
                    EquipmentEntities.GetContext().Component.Add(CurrentComponent);
                    EquipmentEntities.GetContext().SaveChanges();
                    Exit();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка. Проверьте правильность заполнения полей.");
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
            CloseDialog();
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

                if (!string.IsNullOrWhiteSpace(SelectedFormFactor))
                {
                    var selectedFormFactorCharacteristic = new Models.ComponentCharacteristic
                    {
                        ComponentTypeCharacteristicId = 23,
                        ComponentId = CurrentComponent.Id,
                        Value = SelectedFormFactor
                    };
                    AddCharacteristic(selectedFormFactorCharacteristic);
                }

                if (CountOfFan > -1)
                {
                    var countOfFanCharacteristic = new Models.ComponentCharacteristic
                    {
                        ComponentTypeCharacteristicId = 25,
                        ComponentId = CurrentComponent.Id,
                        Value = CountOfFan.ToString()
                    };
                    AddCharacteristic(countOfFanCharacteristic);
                }

                if (!string.IsNullOrWhiteSpace(Size))
                {
                    var sizeCharacteristic = new Models.ComponentCharacteristic
                    {
                        ComponentTypeCharacteristicId = 24,
                        ComponentId = CurrentComponent.Id,
                        Value = Size
                    };
                    AddCharacteristic(sizeCharacteristic);
                }

                if (!string.IsNullOrWhiteSpace(Connectors))
                {
                    var connectorsCharacteristic = new Models.ComponentCharacteristic
                    {
                        ComponentTypeCharacteristicId = 26,
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
                        case 23:
                            SelectedFormFactor = characteristic.Value;
                            break;
                        case 24:
                            Size = characteristic.Value;
                            break;
                        case 25:
                            CountOfFan = int.Parse(characteristic.Value);
                            break;
                        case 26:
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
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
