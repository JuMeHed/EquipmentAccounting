using EquipmentAccounting.Classes;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Input;
namespace EquipmentAccounting.ViewModels
{
    internal class UserMenuViewModel : INotifyPropertyChanged
    {
        private Page _currentPage; // Текущая страница в меню
        private bool _isEquipmentBtnChecked; // Флаг для кнопки "Оборудование"
        private bool _isProfileBtnChecked; // Флаг для кнопки "Профиль"
        // Свойство для кнопки "Оборудование"
        public bool IsEquipmentBtnChecked
        {
            get => _isEquipmentBtnChecked;
            set
            {
                if (_isEquipmentBtnChecked != value)
                {
                    _isEquipmentBtnChecked = value; // Установка значения
                    OnPropertyChanged(); // Уведомление об изменении свойства
                    if (_isEquipmentBtnChecked) // Если кнопка выбрана
                    {
                        IsProfileBtnChecked = false; // Снять выбор с кнопки "Профиль"
                    }
                }
            }
        }
        // Свойство для кнопки "Профиль"
        public bool IsProfileBtnChecked
        {
            get => _isProfileBtnChecked;
            set
            {
                if (_isProfileBtnChecked != value)
                {
                    _isProfileBtnChecked = value; // Установка значения
                    OnPropertyChanged(); // Уведомление об изменении свойства
                    if (_isProfileBtnChecked) // Если кнопка выбрана
                    {
                        IsEquipmentBtnChecked = false; // Снять выбор с кнопки "Оборудование"
                    }
                }
            }
        }
        // Свойство для текущей страницы
        public Page CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value; // Установка текущей страницы
                OnPropertyChanged(); // Уведомление об изменении свойства
            }
        }
        // Команды для обработки нажатий на кнопки
        public ICommand EquipmentCLickCommand => new RelayCommand(EquipmentBtnCLick);
        public ICommand ProfileClickCommand => new RelayCommand(ProfileBtnClick);
        // Конструктор
        public UserMenuViewModel()
        {
            Manager.UserMenu = this; // Привязка текущего ViewModel к менеджеру
            Manager.MainViewModel.IsBorderVisible = true; // Установка видимости границы
        }
        // Метод обработки нажатия на кнопку "Оборудование"
        private void EquipmentBtnCLick()
        {
            // Логика для кнопки "Оборудование" может быть добавлена здесь
        }
        // Метод обработки нажатия на кнопку "Профиль"
        private void ProfileBtnClick()
        {
            Views.AdminViews.ProfileView profile = new Views.AdminViews.ProfileView(); // Создание нового представления профиля
            ProfileViewModel viewModel = new ProfileViewModel(); // Создание ViewModel для профиля
            profile.DataContext = viewModel; // Привязка DataContext
            Manager.UserMenu.CurrentPage = profile; // Установка текущей страницы на представление профиля
        }
        // Событие для уведомления об изменении свойств
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); // Уведомление об изменении свойства
        }
    }
}
