using EquipmentAccounting.ViewModels;
using System.Windows.Controls;

namespace EquipmentAccounting.Classes
{
    internal class Manager
    {
        public static MainWindowViewModel MainViewModel { get; set; }
        public static Page CurrentPage { get; set; }
    }
}
