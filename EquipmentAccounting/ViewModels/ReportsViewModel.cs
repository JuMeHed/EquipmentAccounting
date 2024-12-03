using EquipmentAccounting.Models;
using LiveCharts;
using LiveCharts.Wpf;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using LiveCharts.Defaults;


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

        private List<string> _componentTypes;
        public ChartValues<double> AppliedValues { get; set; }
        public ChartValues<double> NotAppliedValues { get; set; }
        public SeriesCollection SeriesCollection { get; set; }
        public List<string> ComponentTypes
        {
            get => _componentTypes;
            set
            {
                _componentTypes = value;
                OnPropertyChanged();
            }
        }
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
            SetChart();
        }

        private void SetChart()
        {
            AppliedValues = new ChartValues<double>();
            NotAppliedValues = new ChartValues<double>();
            ComponentTypes = EquipmentEntities.GetContext().ComponentType.Select(x => x.Title).ToList();

            foreach (string component in ComponentTypes)
            {
                var items = EquipmentEntities.GetContext().Component
                    .Where(x => x.ComponentType.Title.Equals(component))
                    .ToList();

                int appliedCount = 0;
                int notAppliedCount = 0;

                foreach (var item in items)
                {
                    var componentInEquipment = EquipmentEntities.GetContext().EquipmentComponent
                                               .Where(x => x.ComponentId == item.Id)
                                               .OrderByDescending(x => x.Id)
                                               .FirstOrDefault();

                    if (componentInEquipment != null && componentInEquipment.IsActual)
                    {
                        appliedCount++;
                    }
                    else
                    {
                        notAppliedCount++;
                    }
                }

                AppliedValues.Add(appliedCount);
                NotAppliedValues.Add(notAppliedCount);
            }

            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Примененные",
                    Values = AppliedValues
                },
                new ColumnSeries
                {
                    Title = "Непримененные",
                    Values = NotAppliedValues
                }
            };

            OnPropertyChanged(nameof(SeriesCollection));
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
