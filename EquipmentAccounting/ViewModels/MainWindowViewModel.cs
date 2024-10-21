using EquipmentAccounting.Classes;
using EquipmentAccounting.Views;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
namespace EquipmentAccounting.ViewModels
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        public Page MainPage;
        private Page _currentPage;
        private bool _isBorderVisible;
        public Page CurrentPage
        {
            get { return _currentPage; }
            set
            {
                _currentPage = value;
                OnPropertyChanged(nameof(CurrentPage));
            }
        }
        public bool IsBorderVisible 
        {
            get => _isBorderVisible;
            set
            {
                _isBorderVisible = value;
                OnPropertyChanged();
            }
        }
        public ICommand CloseCommand { get; set; }
        public ICommand DragMoveCommand { get; set; }

        public MainWindowViewModel()
        {
            Manager.MainViewModel = this;
            IsBorderVisible = false;
            Page loginPage = new LoginPage();

            CurrentPage = loginPage;
            Manager.CurrentPage = CurrentPage;

            CloseCommand = new RelayCommand(Close);
            DragMoveCommand = new RelayCommand(DragMove);
        }

        private void Close()
        {
            Application.Current.MainWindow.Close();
        }

        private void DragMove()
        {
            try
            {
                Application.Current.MainWindow.DragMove();
            }
            catch { }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
