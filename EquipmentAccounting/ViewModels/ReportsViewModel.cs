using EquipmentAccounting.Models;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace EquipmentAccounting.ViewModels
{
    internal class ReportsViewModel : INotifyPropertyChanged
    {
        private double _percentInRepair;
        private double _percentNeedRepair;
        private double _percentGoodState;

        private int _countInRepair;
        private int _countNeedRepair;
        private int _countGoodState;

        public double PercentInRepair
        {
            get => _percentInRepair;
            set
            {
                _percentInRepair = value;
                OnPropertyChanged();
            }
        }

        public double PercentNeedRepair
        {
            get => _percentNeedRepair;
            set
            {
                _percentNeedRepair = value;
                OnPropertyChanged();
            }
        }

        public double PercentGoodState
        {
            get => _percentGoodState;
            set
            {
                _percentGoodState = value;
                OnPropertyChanged();
            }
        }

        public int CountInRepair
        {
            get => _countInRepair;
            set
            {
                _countInRepair = value;
                OnPropertyChanged();
            }
        }

        public int CountNeedRepair
        {
            get => _countNeedRepair;
            set
            {
                _countNeedRepair = value;
                OnPropertyChanged();
            }
        }
        
        public int CountGoodState 
        {
            get => _countGoodState;
            set
            {
                _countGoodState = value;
                OnPropertyChanged();
            }
        }
        public ReportsViewModel()
        {
            GetInfoAboutEquipment();
        }

        private void GetInfoAboutEquipment()
        {
            var countInRepair = EquipmentEntities.GetContext().Equipment
                                    .Where(x => x.StateId == 3).ToList();
            var countNeedRepair = EquipmentEntities.GetContext().Equipment
                                    .Where(x => x.StateId == 2).ToList();
            var countGoodState = EquipmentEntities.GetContext().Equipment
                                    .Where(x => x.StateId == 1).ToList();

            var quantityOfEquipment = EquipmentEntities.GetContext().Equipment;

            CountGoodState = countGoodState.Count();
            CountNeedRepair = countNeedRepair.Count();
            CountInRepair = countInRepair.Count();

            PercentInRepair = ((double)CountInRepair / (double)quantityOfEquipment.Count()) * 100;
            PercentNeedRepair = ((double)CountNeedRepair / (double)quantityOfEquipment.Count()) * 100;
            PercentGoodState = ((double)CountGoodState / (double)quantityOfEquipment.Count()) * 100;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
