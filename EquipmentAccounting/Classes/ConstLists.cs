using System.Collections.Generic;

namespace EquipmentAccounting.Classes
{
    internal static class ConstLists
    {
        public static readonly List<string> TYPES_OF_QR = new List<string>() { "Сгенерировать по серийному номеру", "Сгенерировать по инвентарному номеру" };
        public static readonly List<int> NUMBER_OF_THREADS = new List<int>() { 2, 4, 6, 8, 12, 16, 20, 24, 28, 32, 36, 48, 64, 128 };
        public static readonly List<int> NUMBER_OF_CORES = new List<int>() { 2, 4, 6, 8, 10, 12, 16, 64 };
        public static readonly List<int> NUMBER_OF_M2 = new List<int>() { 0, 1, 2, 3, 4, 5, 7 };
        public static readonly List<int> NUMBER_OF_SATA = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8 };
        public static readonly List<int> NUMBER_OF_RAM = new List<int>() { 1, 2, 4, 8 };
        public static readonly List<int> RAM_CAPACITIES = new List<int>() { 2, 4, 8, 16, 24, 32, 48, 64, 96, 128 };
        public static readonly List<string> SOCKET_TYPES = new List<string>
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

        public static readonly List<string> CHIPSETS = new List<string>
        {
            "Z790", // 13th Gen
            "Z690", // 12th Gen
            "B660", // 12th Gen
            "H670", // 12th Gen
            "H610", // 12th Gen
            "Z590", // 11th Gen
            "B560", // 11th Gen
            "H510", // 11th Gen
            "Z490", // 10th Gen
            "B460", // 10th Gen
            "H470", // 10th Gen
            "H410", // 10th Gen
            "X299", // High-end desktop
            "C621", // Server
            "C622", // Server
            "C632", // Server
            "Z370", // 8th Gen
            "B360", // 8th Gen
            "H370", // 8th Gen
            "H310", // 8th Gen
            "Q370", // Business
            "H410", // 10th Gen
            "B365", // 9th Gen
        
            // AMD Chipsets
            "X670E", // 700 Series
            "X670",  // 700 Series
            "B650E", // 600 Series
            "B650",  // 600 Series
            "A620",  // 600 Series
            "X570",  // 500 Series
            "B550",  // 500 Series
            "A520",  // 500 Series
            "X470",  // 400 Series
            "B450",  // 400 Series
            "A320",  // 400 Series
            "TRX40", // Threadripper
            "WRX80", // Threadripper Pro
            "X399",  // Threadripper
            "B350",  // 300 Series
            "A300",  // 300 Series
        
            // Other Chipsets
            "H370",  // Intel 8th/9th Gen
            "B360",  // Intel 8th/9th Gen
            "H310",  // Intel 8th/9th Gen
            "Z370",  // Intel 8th Gen
            "C246",  // Intel Xeon
            "C232",  // Intel Xeon
            "C621",  // Intel Xeon
            "C622",  // Intel Xeon
            "C686",  // Intel Xeon
        
            // Legacy Chipsets
            "X58",   // Intel
            "P67",   // Intel
            "H67",   // Intel
            "Q67",   // Intel
            "P55",   // Intel
            "H55",   // Intel
            "X48",   // Intel
            "P45",   // Intel
            "G41",   // Intel
            "G43",   // Intel
            "P35",   // Intel
            "P965",  // Intel
            "865",   // Intel
            "775",   // Intel
        };

        public static readonly List<string> MOTHERBOARDS_FORM_FACTORS = new List<string>
        {
            "ATX",               // Advanced Technology eXtended
            "Micro ATX",        // Smaller version of ATX
            "Mini ATX",         // Even smaller than Micro ATX
            "Mini ITX",         // Compact form factor for small builds
            "Nano ITX",         // Smaller than Mini ITX
            "Pico ITX",         // Very small form factor
            "Flex ATX",         // Flexible version of ATX
            "E-ATX",            // Extended ATX for more features
            "XL-ATX",           // Extra Large ATX for high-end systems
            "BTX",              // Balanced Technology eXtended
            "LPX",              // Low Profile eXtended
            "DTX",              // Double-sided Mini ITX
            "HPTX",             // High Performance ATX
            "Micro BTX",        // Smaller version of BTX
            "Mini DTX",         // Compact version of DTX
            "Server ATX",       // Specialized for server use
            "Custom",           // Custom form factors for specific builds
        };

        public static readonly List<string> CASE_FORM_FACTORS = new List<string>
        {
            "ATX",               // Standard size for most desktop cases
            "Micro ATX",        // Smaller than ATX, supports fewer components
            "Mini ITX",         // Compact size for small builds
            "E-ATX",            // Extended ATX for additional features
            "XL-ATX",           // Extra large ATX for high-end systems
            "BTX",              // Balanced Technology eXtended
            "Mini DTX",         // Compact version of DTX, supports fewer components
            "Pico ITX",         // Very small form factor, typically for specialized builds
            "Flex ATX",         // Flexible version of ATX, often used in compact systems
            "HTPC",             // Home Theater PC form factor, designed for media centers
            "Cube",             // Cube-shaped cases, often used for compact builds
            "Rackmount",        // Designed for mounting in racks, typically for servers
            "Custom",           // Custom form factors for specific builds
            "Server",           // Specialized for server use, may vary in size
        };

        public readonly static List<string> RAM_FORM_FACTORS = new List<string>
        {
            "DIMM",
            "LRDIMM",
            "RDIMM"
        };

        public readonly static List<string> RAM_TYPES = new List<string>
        {
            "DDR2",
            "DDR3",
            "DDR4",
            "DDR5"
        };

        public static readonly List<string> COOLING_TYPES = new List<string>
        {
            "Воздушное",             // Air cooling
            "Жидкостное",            // Liquid cooling
            "Гибридное",             // Hybrid cooling (combination of air and liquid)
            "Пассивное",             // Passive cooling (no fans)
            "Термоэлектрическое",    // Thermoelectric cooling (Peltier)
            "Водяное охлаждение",    // Water cooling
            "Альтернативное",        // Alternative cooling methods (e.g., phase change)
        };

        public static readonly List<string> BASE_MATERIALS = new List<string>
        {
            "Медь",                  // Copper
            "Алюминий",             // Aluminum
            "Сталь",                // Steel
            "Комбинированный",      // Combined materials
            "Нержавеющая сталь",    // Stainless steel
        };

        public static readonly List<string> RADIATOR_MATERIALS = new List<string>
        {
            "Медь",                  // Copper
            "Алюминий",             // Aluminum
            "Сталь",                // Steel
            "Пластик",              // Plastic
            "Комбинированный",      // Combined materials
        };

        public static readonly List<string> STORAGE_CAPACITY = new List<string>
        {
            "128 ГБ",
            "256 ГБ",
            "512 ГБ",
            "1 ТБ",
            "2 ТБ",
            "4 ТБ",
            "8 ТБ",
            "16 ТБ",
            "32 ТБ",
            "64 ТБ",
            "128 ТБ",
            "256 ТБ",
            "512 ТБ",
            "1 ПБ",
            "2 ПБ",
            "4 ПБ",
            "8 ПБ",
            "16 ПБ",
            "32 ПБ",
            "64 ПБ"
        };

        public static readonly List<string> MEMORY_BUSES = new List<string>
        {
            "64-bit",
            "128-bit",
            "192-bit",
            "256-bit",
            "384-bit"
        };

        public static readonly List<string> VIDEO_MEMORY_TYPES = new List<string>
        {
            "GDDR6",
            "GDDR5",
            "GDDR5X",
            "HBM2",
            "GDDR4"
        };

        public static readonly List<string> VIDEO_MEMORY_CAPACITIES = new List<string>
        {
            "2GB",
            "4GB",
            "6GB",
            "8GB",
            "10GB",
            "12GB",
            "16GB"
        };

        public static readonly List<string> MAX_RESOLUTIONS = new List<string>
        {
            "1920x1080",
            "2560x1440",
            "3840x2160",
            "7680x4320",
            "1280x720",
            "1600x900"
        };

        public static readonly List<string> TECHNOLOGIES = new List<string>
        {
            "NVIDIA G-Sync",
            "AMD FreeSync"
        };

        public static readonly List<string> POWER_SUPPLY_FORM_FACTORS = new List<string>
        {
            "ATX",               // Стандартный форм-фактор для большинства блоков питания
            "SFX",               // Компактный форм-фактор для малых систем
            "SFX-L",             // Увеличенная версия SFX с большей мощностью
            "TFX",               // Форм-фактор для тонких и компактных систем
            "Flex ATX",         // Гибкий форм-фактор, часто используемый в малых системах
            "Micro ATX",        // Меньший форм-фактор, подходит для компактных корпусов
            "PS2",               // Старый форм-фактор, использовавшийся в некоторых системах
            "Custom",            // Пользовательские форм-факторы для специфических сборок
        };
                public static readonly List<string> POWER_SUPPLY_CERTIFICATIONS = new List<string>
        {
            "80 PLUS",                   // Базовая сертификация для эффективных блоков питания
            "80 PLUS Bronze",            // Сертификация для блоков питания с эффективностью от 82% до 85%
            "80 PLUS Silver",            // Сертификация для блоков питания с эффективностью от 85% до 88%
            "80 PLUS Gold",              // Сертификация для блоков питания с эффективностью от 87% до 90%
            "80 PLUS Platinum",          // Сертификация для блоков питания с эффективностью от 90% до 92%
            "80 PLUS Titanium",          // Сертификация для блоков питания с эффективностью от 90% до 94%
            "Energy Star",               // Сертификация для устройств с низким энергопотреблением
            "CE",                        // Европейская сертификация безопасности
            "UL",                        // Сертификация безопасности от Underwriters Laboratories
            "FCC",                       // Сертификация от Федеральной комиссии связи США
        };

        // Список интерфейсов подключения
        public static readonly List<string> CONNECTION_INTERFACES = new List<string>
        {
            "Ethernet",          // Стандартный интерфейс для проводных сетей
            "Wi-Fi",             // Беспроводной интерфейс для сетей
            "Bluetooth",         // Беспроводной интерфейс для коротких расстояний
            "Fiber Optic",       // Интерфейс для оптоволоконных соединений
            "USB",               // Универсальная последовательная шина для подключения устройств
            "Serial",            // Последовательный интерфейс для подключения устройств
            "Parallel",          // Параллельный интерфейс для подключения устройств
            "Thunderbolt"        // Высокоскоростной интерфейс для подключения периферии
        };

        // Список типов интерфейсов
        public static readonly List<string> INTERFACE_TYPES = new List<string>
        {
            "PCIe",              // Высокоскоростной интерфейс для подключения карт
            "PCI",               // Стандартный интерфейс для подключения карт
            "M.2",               // Компактный интерфейс для SSD и других устройств
            "PCMCIA",            // Интерфейс для подключения карт расширения
            "ExpressCard"        // Современный интерфейс для подключения карт расширения
        };

        public static readonly List<string> SOUND_CARD_TECHNOLOGIES = new List<string>
        {
            "Stereo",                // Базовая технология для двухканального звука
            "Surround Sound",        // Технология для многоканального звука (например, 5.1, 7.1)
            "3D Audio",             // Технология для создания трехмерного звука
            "Dolby Digital",         // Технология сжатия звука для многоканального аудио
            "DTS",                   // Альтернатива Dolby Digital для многоканального звука
            "ASIO",                  // Технология для низкой задержки звука, используемая в профессиональной аудиозаписи
            "WASAPI",                // Windows Audio Session API для работы с низкой задержкой
            "EAX",                   // Environmental Audio Extensions для улучшения 3D-звука в играх
            "OpenAL",                // Открытый стандарт для 3D-звука и аудиоприложений
            "Hi-Res Audio",         // Поддержка высококачественного аудио с высокой частотой дискретизации
            "MIDI",                  // Технология для работы с музыкальными инструментами и синтезаторами
            "Virtual Surround",       // Эмуляция многоканального звука через стереонаушники
        };
    }
}
