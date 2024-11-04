using EquipmentAccounting.ViewModels;
using System.Windows.Controls;

namespace EquipmentAccounting.Classes
{
    internal class Manager
    {
        public static MainWindowViewModel MainViewModel { get; set; }
        public static Page CurrentPage { get; set; }
        public static MenuPageViewModel MenuPage { get; set; }
        public static UserMenuViewModel UserMenu {  get; set; }
    }
}
