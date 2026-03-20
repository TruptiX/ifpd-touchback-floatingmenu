# MCU Software - ESP32-S3 USB HID Touch Device

This firmware runs on an ESP32-S3 microcontroller, receiving touch data from an IFPD (Interactive Flat Panel Display) over UART and presenting it to a connected computer as a USB HID multi-touch digitizer.

## Architecture

IFPD (Laptop 1) ──UART 3Mbaud──> ESP32-S3 ──USB HID──> Target Computer (Laptop 2)
MCU

## Features

- **USB HID Digitizer**: 10-finger multi-touch support
- **High-speed UART**: 3 Mbaud input from IFPD
- **Low latency**: Optimized dual-task architecture
- **Standards compliant**: Windows Precision Touchpad compatible

## Documentation

📖 **[SETUP.md](SETUP.md)** - Development environment setup and build instructions

🚀 **[DEPLOYMENT.md](DEPLOYMENT.md)** - Flashing firmware and deployment guide

## Quick Start

### For Developers

1. Follow [SETUP.md](SETUP.md) to configure your environment
2. Build the project
3. Follow [DEPLOYMENT.md](DEPLOYMENT.md) to flash and test

### For Production

See the "Production Deployment" section in [DEPLOYMENT.md](DEPLOYMENT.md).

## Hardware Requirements

- ESP32-S3 development board (with USB-OTG support)
- USB cable (data-capable)
- UART connection from IFPD (3 Mbaud, 8N1)

## Project Structure

```
MCU/
├── main/
│ ├── app_main.c # Main application and UART handling
│ ├── hid_touch.c # HID touch descriptor and reporting
│ └── hid_touch.h
├── managed_components/ # ESP-IDF managed dependencies
├── CMakeLists.txt # Project CMake configuration
├── sdkconfig # ESP-IDF configuration
└── README.md # This file
```

## Technical Details

- **UART Protocol**: Custom CSV format with 13 fields per contact
- **HID Range**: 0-32767 logical units (auto-mapped by Windows)
- **CPU Affinity**: UART task on CPU1, USB task on CPU0
- **Queue Size**: 64 lines (handles burst traffic)

## Support

For issues related to:
- **Building**: See [SETUP.md](SETUP.md) troubleshooting
- **Deployment**: See [DEPLOYMENT.md](DEPLOYMENT.md) troubleshooting
- **Touch protocol**: Review `parse_touch()` in [main/app_main.c](main/app_main.c)
- **HID descriptor**: Review [main/hid_touch.c](main/hid_touch.c)

## License

Copyright (C) 2026 Intel Corporation  
SPDX-License-Identifier: Apache-2.0