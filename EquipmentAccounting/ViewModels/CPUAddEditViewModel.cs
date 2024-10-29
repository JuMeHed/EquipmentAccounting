using EquipmentAccounting.Classes;
using EquipmentAccounting.Models;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace EquipmentAccounting.ViewModels
{
    internal class CPUAddEditViewModel : INotifyPropertyChanged
    {
        private static readonly List<int> NUMBER_OF_CORES = new List<int>() { 2, 4, 6, 8, 10, 12, 16, 64 };
        private static readonly List<string> SOCKET_TYPES = new List<string>
        {
            "AM5",
            "AM4",
            "LGA1700",
            "LGA1200",
            "LGA1151-v2",
            "LGA1151",
            "AM3+",
            "sWRX8",
            "TR4",
            "FM2+"
        };

        private Models.Component _currentComponent;
        private Models.ComponentCharacteristic _characteristic;
        private bool _isDialogOpen;
        private bool _isEditing;
        private string _frequency;
        private int _numberOfCores;
        private string _socket;
        private string _cash;
        private string _energyConsumption;
        public bool IsDialogOpen
        {
            get => _isDialogOpen;
            set
            {
                _isDialogOpen = value;
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
                    LoadCharacteristics();
                    IsEditing = true;
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

        public List<int> AvailableCores => NUMBER_OF_CORES;
        public List<string> Sockets => SOCKET_TYPES;

        public ICommand GoBackCommand => new RelayCommand(GoBack);
        public ICommand ExitCommand => new RelayCommand(Exit);
        public ICommand CloseDialogCommand => new RelayCommand(CloseDialog);
        public ICommand SaveCommand => new RelayCommand(OnSaveChanges); 
        public CPUAddEditViewModel()
        {
            
        }

        private void CloseDialog()
        {
            IsDialogOpen = false;
        }
        private void Exit()
        {
            _isDialogOpen = false;
            Views.AdminViews.ComponentsPage componentsPage = new Views.AdminViews.ComponentsPage();
            Classes.Manager.MenuPage.CurrentPage = componentsPage;
        }
        private void GoBack()
        {
            IsDialogOpen = true;
        }

        private void OnSaveChanges()
        {
            if (IsEditing)
            {
                ClearCharacteristics();
                SetCharacteristics();

                try
                {
                    EquipmentEntities.GetContext().SaveChanges();
                    Exit();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "сохранение");
                }
            } else
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
                    MessageBox.Show(ex.Message + "сохранение");
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
                MessageBox.Show(ex.Message);
            }
        }
        private void LoadCharacteristics()
        {
            //MessageBox.Show("ZZ");
            //MessageBox.Show($"{CurrentComponent != null}");
            if (CurrentComponent != null)
            {
                var characteristics = EquipmentEntities.GetContext().ComponentCharacteristic
                    .Where(cc => cc.ComponentId == CurrentComponent.Id).ToList();
                MessageBox.Show($"{CurrentComponent.Id}");

                foreach (var characteristic in characteristics)
                {
                    MessageBox.Show($"characteristic.ComponentTypeCharacteristic");
                    switch (characteristic.ComponentTypeCharacteristicId)
                    {
                        case 1: 
                            Frequency = characteristic.Value;
                            //MessageBox.Show($"Frequency: {Frequency} value {characteristic.Value}");
                            break;
                        case 2: 
                            NumberOfCores = int.Parse(characteristic.Value);
                            MessageBox.Show($"Frequency: {NumberOfCores} value {characteristic.Value}");
                            break;
                        case 3: 
                            Socket = characteristic.Value;
                            MessageBox.Show($"Frequency: {Socket} value {characteristic.Value}");
                            break;
                        case 4:
                            Cash = characteristic.Value;
                            break;
                        case 5: 
                            EnergyConsumption = characteristic.Value;
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
                    .Where(cc => cc.ComponentId != CurrentComponent.Id).ToList();
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
