﻿using System.Collections.Generic;

namespace EquipmentAccounting.Classes
{
    internal static class ConstLists
    {
        public static readonly List<int> NUMBER_OF_THREADS = new List<int>() { 2, 4, 6, 8, 12, 16, 20, 24, 28, 32, 36, 48, 64, 128 };
        public static readonly List<int> NUMBER_OF_CORES = new List<int>() { 2, 4, 6, 8, 10, 12, 16, 64 };
        public static readonly List<int> NUMBER_OF_M2 = new List<int>() { 0, 1, 2, 3, 4, 5, 7 };
        public static readonly List<int> NUMBER_OF_SATA = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8 };
        public static readonly List<int> NUMBER_OF_RAM = new List<int>() { 1, 2, 4, 8 };
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
    }
}