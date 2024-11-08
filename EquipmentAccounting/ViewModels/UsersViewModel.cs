using EquipmentAccounting.Classes;
using EquipmentAccounting.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
namespace EquipmentAccounting.ViewModels
{
    internal class UsersViewModel : INotifyPropertyChanged
    {
        // Коллекции для хранения пользователей, фильтрованных пользователей, уровней доступа и локаций
        private ObservableCollection<Models.User> _users;
        private ObservableCollection<Models.User> _filteredUsers;
        private ObservableCollection<AccessLevel> _accessLevels;
        private ObservableCollection<AccessLevel> _editAccessLevels;
        private ObservableCollection<Location> _locations;
        private ObservableCollection<Location> _usersLocations;
        // Выбранные пользователи и уровни доступа
        private Models.User _selectedUser;
        private AccessLevel _selectedAccessLevel;
        private AccessLevel _editSelectedLevel;
        private Location _selectedLocation;
        // Фильтры и заголовок диалогового окна
        private string _fullnameFilter;
        private string _dialogTitle;
        // Флаги для управления открытием диалоговых окон
        private bool _isDeleteDialogOpen;
        private bool _isEditDialogOpen;
        // Свойства для доступа к коллекциям пользователей и другим данным
        public ObservableCollection<Models.User> Users
        {
            get => _users;
            set
            {
                _users = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Models.User> FilteredUsers
        {
            get => _filteredUsers;
            set
            {
                _filteredUsers = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<AccessLevel> AccessLevels
        {
            get => _accessLevels;
            set
            {
                _accessLevels = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<AccessLevel> EditAccessLevels
        {
            get => _editAccessLevels;
            set
            {
                _editAccessLevels = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Location> Locations
        {
            get => _locations;
            set
            {
                _locations = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Location> UsersLocations
        {
            get => _usersLocations;
            set
            {
                _usersLocations = value;
                OnPropertyChanged();
            }
        }
        // Свойства для выбранного пользователя и уровня доступа
        public Models.User SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged();
            }
        }
        public AccessLevel SelectedAccessLevel
        {
            get => _selectedAccessLevel;
            set
            {
                _selectedAccessLevel = value;
                OnPropertyChanged();
                FilterUsers(); // Фильтруем пользователей при изменении уровня доступа
            }
        }
        public AccessLevel EditSelectedLevel
        {
            get => _editSelectedLevel;
            set
            {
                _editSelectedLevel = value;
                OnPropertyChanged();
            }
        }
        public Location SelectedLocation
        {
            get => _selectedLocation;
            set
            {
                _selectedLocation = value;
                OnPropertyChanged();
            }
        }
        // Свойство для фильтрации по полному имени
        public string FullNameFilter
        {
            get => _fullnameFilter;
            set
            {
                _fullnameFilter = value;
                OnPropertyChanged();
                FilterUsers(); // Фильтруем пользователей при изменении фильтра
            }
        }
        public string DialogTitle
        {
            get => _dialogTitle;
            set
            {
                _dialogTitle = value;
                OnPropertyChanged();
            }
        }
        // Флаги для управления диалогами
        public bool IsDeleteDialogOpen
        {
            get => _isDeleteDialogOpen;
            set
            {
                _isDeleteDialogOpen = value;
                OnPropertyChanged();
            }
        }
        public bool IsEditDialogOpen
        {
            get => _isEditDialogOpen;
            set
            {
                _isEditDialogOpen = value;
                OnPropertyChanged();
            }
        }
        // Команды для управления действиями в интерфейсе
        public ICommand DeleteDialogOpenCommand => new RelayCommand<Models.User>(DeleteDialogOpen);
        public ICommand DialogCloseCommand => new RelayCommand(DialogClose);
        public ICommand DeleteUserCommand => new RelayCommand(DeleteUser);
        public ICommand EditDialogOpenCommand => new RelayCommand<Models.User>(OpenEditDialog);
        public ICommand AddDialogOpenCommand => new RelayCommand(OpenAddDialog);
        public ICommand SaveChangesCommand => new RelayCommand(SaveChanges);
        public ICommand DeleteResponsobilityCommand => new RelayCommand<Location>(DeleteResponsibility);
        public ICommand AssignRoomCommand => new RelayCommand(AddResponsibility);
        // Конструктор, загружающий данные при создании ViewModel
        public UsersViewModel()
        {
            LoadUsers(); // Загружаем пользователей
            LoadAccessLevels(); // Загружаем уровни доступа
            FilterUsers(); // Применяем фильтр
            LoadLocations(); // Загружаем локации
        }
        // Метод для загрузки пользователей из базы данных
        private void LoadUsers()
        {
            var users = new ObservableCollection<Models.User>(EquipmentEntities.GetContext().User.ToList());
            Users = users;
        }
        // Метод для загрузки локаций из базы данных
        private void LoadLocations()
        {
            var locations = new ObservableCollection<Models.Location>(EquipmentEntities.GetContext().Location.ToList());
            Locations = locations;
        }
        // Метод для загрузки локаций пользователей
        private void LoadUsersLocations()
        {
            var usersLocations = Locations.Where(x => x.ResponsibleId == SelectedUser.Id);
            UsersLocations = new ObservableCollection<Location>(usersLocations);
        }
        // Метод для загрузки уровней доступа
        private void LoadAccessLevels()
        {
            var accessLevels = new ObservableCollection<AccessLevel>(EquipmentEntities.GetContext().AccessLevel.ToList());
            EditAccessLevels = new ObservableCollection<AccessLevel>(EquipmentEntities.GetContext().AccessLevel.ToList());
            accessLevels.Insert(0, new AccessLevel { Description = "Все типы" }); // Добавляем опцию "Все типы"
            AccessLevels = accessLevels;
            SelectedAccessLevel = AccessLevels[0]; // Устанавливаем выбранный уровень доступа по умолчанию
        }
        // Метод для удаления пользователя
        private void DeleteUser()
        {
            try
            {
                EquipmentEntities.GetContext().User.Remove(SelectedUser); // Удаляем выбранного пользователя
                EquipmentEntities.GetContext().SaveChanges(); // Сохраняем изменения
                DialogClose(); // Закрываем диалог
                LoadUsers(); // Перезагружаем пользователей
                FilterUsers(); // Применяем фильтр
            }
            catch
            {
                MessageBox.Show("Произошла непредвиденная ошибка."); // Обработка ошибок
            }
        }
        // Метод для открытия диалога удаления
        private void DeleteDialogOpen(Models.User user)
        {
            SelectedUser = user; // Устанавливаем выбранного пользователя
            IsDeleteDialogOpen = true; // Открываем диалог удаления
        }
        // Метод для закрытия диалогов
        private void DialogClose()
        {
            IsDeleteDialogOpen = false; // Закрываем диалог удаления
            IsEditDialogOpen = false; // Закрываем диалог редактирования
        }
        // Метод для открытия диалога редактирования
        private void OpenEditDialog(Models.User user)
        {
            SelectedUser = user; // Устанавливаем выбранного пользователя
            DialogTitle = "Редактирование пользователя"; // Устанавливаем заголовок
            LoadUsersLocations(); // Загружаем локации пользователя
            IsEditDialogOpen = true; // Открываем диалог редактирования
        }
        // Метод для открытия диалога добавления
        public void OpenAddDialog()
        {
            DialogTitle = "Добавление пользователя"; // Устанавливаем заголовок
            SelectedUser = new Models.User(); // Создаем нового пользователя
            LoadUsersLocations(); // Загружаем локации
            IsEditDialogOpen = true; // Открываем диалог добавления
        }
        // Метод для сохранения изменений пользователя
        private void SaveChanges()
        {
            try
            {
                if (DialogTitle != "Редактирование пользователя") // Проверяем, добавляем ли мы нового пользователя
                {
                    EquipmentEntities.GetContext().User.Add(SelectedUser); // Добавляем нового пользователя
                    EquipmentEntities.GetContext().SaveChanges(); // Сохраняем изменения
                    // Обновляем ответственные локации для нового пользователя
                    foreach (Location location in UsersLocations)
                    {
                        location.ResponsibleId = SelectedUser.Id;
                    }
                }
                EquipmentEntities.GetContext().SaveChanges(); // Сохраняем изменения
                LoadUsers(); // Перезагружаем пользователей
                FilterUsers(); // Применяем фильтр
                DialogClose(); // Закрываем диалог
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла непредвиденная ошибка." + "\n" + ex.Message); // Обработка ошибок
            }
        }
        // Метод для фильтрации пользователей
        private void FilterUsers()
        {
            try
            {
                if (Users == null || Users.Count == 0)
                {
                    MessageBox.Show("пусто"); // Проверка на пустую коллекцию
                    FilteredUsers = new ObservableCollection<Models.User>();
                    return;
                }
                var filteredUsers = Users.AsEnumerable(); // Начинаем фильтрацию
                // Фильтруем по полному имени
                if (!string.IsNullOrEmpty(FullNameFilter))
                {
                    filteredUsers = filteredUsers.Where(x => x.FullName.ToLower().Contains(FullNameFilter.ToLower()));
                }
                // Фильтруем по уровню доступа
                if (SelectedAccessLevel != null && SelectedAccessLevel != AccessLevels[0])
                {
                    filteredUsers = filteredUsers.Where(x => x.AccessLevel == SelectedAccessLevel);
                }
                FilteredUsers = new ObservableCollection<Models.User>(filteredUsers); // Обновляем отфильтрованных пользователей
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла непредвиденная ошибка." + "\n" + ex.Message); // Обработка ошибок
            }
        }
        // Метод для удаления ответственности
        private void DeleteResponsibility(Location location)
        {
            try
            {
                location.ResponsibleId = null; // Убираем ответственность
                EquipmentEntities.GetContext().SaveChanges(); // Сохраняем изменения
                LoadUsersLocations(); // Обновляем локации пользователей
            }
            catch
            {
                // Обработка ошибок (пустая)
            }
        }
        // Метод для добавления ответственности
        private void AddResponsibility()
        {
            if (SelectedUser.Id != 0) // Проверяем, что выбранный пользователь существует
            {
                if (SelectedLocation != null)
                {
                    SelectedLocation.ResponsibleId = SelectedUser.Id; // Устанавливаем пользователя ответственным
                    LoadUsersLocations(); // Обновляем локации
                }
            }
            else
            {
                UsersLocations.Add(SelectedLocation); // Добавляем локацию, если пользователь не выбран
            }
        }
        // Событие для уведомления об изменении свойств
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); // Уведомляем об изменении свойства
        }
    }
}
