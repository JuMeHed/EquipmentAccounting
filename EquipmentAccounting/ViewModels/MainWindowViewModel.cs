using EquipmentAccounting.Classes;
using EquipmentAccounting.Models;
using EquipmentAccounting.Views;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
namespace EquipmentAccounting.ViewModels
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
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

            //Manager.CurrentPage = CurrentPage;

            string savedLogin = Properties.Settings.Default.CurrentUser;
            if (!string.IsNullOrEmpty(savedLogin))
            {
                var user = EquipmentEntities.GetContext().User.FirstOrDefault(x => x.Login == savedLogin);
                if (user != null)
                {
                    Classes.User.CurrentUser = user;

                    if (user.AccessLevelId == 1)
                        Manager.MainViewModel.CurrentPage = new Views.MenuPage();
                    else
                        Manager.MainViewModel.CurrentPage = new Views.UserViews.UserMenu();
                }
            }
            else
            {
                Manager.MainViewModel.CurrentPage = new Views.LoginPage();
            }

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
