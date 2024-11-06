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
    internal class CPUAddEditViewModel : INotifyPropertyChanged
    {
        private Models.Component _currentComponent;
        private Models.ComponentCharacteristic _characteristic;

        private bool _isSaveDialogOpen;
        private bool _isExitDialogOpen;
        private bool _isEditing;
        private bool _isHasGraphics;

        private int _numberOfCores;
        private int _numberOfThreads;

        private string _frequency;
        private string _socket;
        private string _cash;
        private string _energyConsumption;
        public bool IsExitDialogOpen
        {
            get => _isExitDialogOpen;
            set
            {
                _isExitDialogOpen = value;
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
                        ComponentTypeId = 1
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

        public bool IsEditing
        {
            get => _isEditing;
            set
            {
                _isEditing = value;
                OnPropertyChanged();
            }
        }

        public string Frequency
        {
            get => _frequency;
            set
            {
                _frequency = value;
                OnPropertyChanged();
            }
        }

        public int NumberOfCores
        {
            get => _numberOfCores;
            set
            {
                _numberOfCores = value;
                OnPropertyChanged();
            }
        }
        public int NumberOfThreads
        {
            get => _numberOfThreads;
            set
            {
                _numberOfThreads = value;
                OnPropertyChanged();
            }
        }

        public string Socket
        {
            get => _socket;
            set
            {
                _socket = value;
                OnPropertyChanged();
            }
        }

        public string Cash
        {
            get => _cash;
            set
            {
                _cash = value;
                OnPropertyChanged();
            }
        }
        public string EnergyConsumption
        {
            get => _energyConsumption;
            set
            {
                _energyConsumption = value;
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
        public bool IsHasGraphics
        {
            get => _isHasGraphics;
            set
            {
                _isHasGraphics = value;
                OnPropertyChanged();
            }
        }

        public List<int> AvailableCores => ConstLists.NUMBER_OF_CORES;
        public List<string> Sockets => ConstLists.SOCKET_TYPES;
        public List<int> AvailableThreads => ConstLists.NUMBER_OF_THREADS;

        public ICommand GoBackCommand => new RelayCommand(GoBack);
        public ICommand ExitCommand => new RelayCommand(Exit);
        public ICommand CloseDialogCommand => new RelayCommand(CloseDialog);
        public ICommand SaveCommand => new RelayCommand(OnSaveChanges);
        public ICommand OpenSaveDialogCommand => new RelayCommand(OpenSaveDialog);
        public CPUAddEditViewModel()
        {

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
                }
            }
        }
        private void SetCharacteristics()
        {
            if (CurrentComponent != null)
            {
                if (IsEditing)
                {
                    ClearCharacteristics();
                }

                if (!string.IsNullOrWhiteSpace(Frequency))
                {
                    var frequencyCharacteristic = new Models.ComponentCharacteristic
                    {
                        ComponentTypeCharacteristicId = 1,
                        ComponentId = CurrentComponent.Id,
                        Value = Frequency
                    };
                    AddCharacteristic(frequencyCharacteristic);
                }

                if (NumberOfCores > 0)
                {
                    var coresCharacteristic = new Models.ComponentCharacteristic
                    {
                        ComponentTypeCharacteristicId = 2,
                        ComponentId = CurrentComponent.Id,
                        Value = NumberOfCores.ToString()
                    };
                    AddCharacteristic(coresCharacteristic);
                }

                if (!string.IsNullOrWhiteSpace(Socket))
                {
                    var socketCharacteristic = new Models.ComponentCharacteristic
                    {
                        ComponentTypeCharacteristicId = 3,
                        ComponentId = CurrentComponent.Id,
                        Value = Socket
                    };
                    AddCharacteristic(socketCharacteristic);
                }

                if (!string.IsNullOrWhiteSpace(Cash))
                {
                    var cashCharacteristic = new Models.ComponentCharacteristic
                    {
                        ComponentTypeCharacteristicId = 4,
                        ComponentId = CurrentComponent.Id,
                        Value = Cash
                    };
                    AddCharacteristic(cashCharacteristic);
                }

                if (!string.IsNullOrWhiteSpace(EnergyConsumption))
                {
                    var energyCharacteristic = new Models.ComponentCharacteristic
                    {
                        ComponentTypeCharacteristicId = 5,
                        ComponentId = CurrentComponent.Id,
                        Value = EnergyConsumption
                    };
                    AddCharacteristic(energyCharacteristic);
                }

                if (NumberOfThreads > 0)
                {
                    var threadsCharacteristic = new Models.ComponentCharacteristic
                    {
                        ComponentTypeCharacteristicId = 6,
                        ComponentId = CurrentComponent.Id,
                        Value = NumberOfThreads.ToString()
                    };
                    AddCharacteristic(threadsCharacteristic);
                }

                if (IsHasGraphics)
                {
                    var graphicsCharacteristic = new Models.ComponentCharacteristic
                    {
                        ComponentTypeCharacteristicId = 7,
                        ComponentId = CurrentComponent.Id,
                        Value = "Есть"
                    };
                    AddCharacteristic(graphicsCharacteristic);
                }
                else
                {
                    var graphicsCharacteristic = new Models.ComponentCharacteristic
                    {
                        ComponentTypeCharacteristicId = 7,
                        ComponentId = CurrentComponent.Id,
                        Value = "Нет"
                    };
                    AddCharacteristic(graphicsCharacteristic);
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
                    //MessageBox.Show(characteristic.ComponentTypeCharacteristicId + " " + characteristic.ComponentTypeCharacteristic.Characteristic.Designation);
                    switch (characteristic.ComponentTypeCharacteristicId)
                    {
                        case 1:
                            Frequency = characteristic.Value;
                            break;
                        case 2:
                            NumberOfCores = int.Parse(characteristic.Value);
                            break;
                        case 3:
                            Socket = characteristic.Value;
                            break;
                        case 4:
                            Cash = characteristic.Value;
                            break;
                        case 5:
                            EnergyConsumption = characteristic.Value;
                            break;
                        case 6:
                            NumberOfThreads = int.Parse(characteristic.Value);
                            break;
                        case 7:
                            IsHasGraphics = characteristic.Value.Equals("Есть");
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
