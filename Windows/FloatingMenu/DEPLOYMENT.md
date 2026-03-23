# Floating Menu Application - Deployment Guide

This guide covers building, publishing, and deploying the Floating Menu Application for production use on interactive flat panel displays (IFPD) and Windows systems.

## 📋 Table of Contents

- [Prerequisites](#prerequisites)
- [Building the Application](#building-the-application)
- [Publishing Options](#publishing-options)
- [Manual Deployment](#manual-deployment)
- [Testing the Deployment](#testing-the-deployment)
- [Startup Configuration](#startup-configuration)
- [Screen Annotation Integration](#screen-annotation-integration)
- [Updates and Maintenance](#updates-and-maintenance)
- [Uninstalling](#uninstalling)

## ✅ Prerequisites

### Development Machine

- **Windows 10/11** with latest updates
- **.NET 10 SDK** installed
- **Git** for source control
- **Visual Studio 2022** or **Visual Studio 2026** (recommended)
- **OpenCvSharp4** NuGet packages available

### Target Machine

- **Windows 10** (build 19041+) or **Windows 11**
- **.NET 10 Runtime** (for framework-dependent deployment) or **.NET 10 Desktop Runtime**
- **DirectShow-compatible camera** (USB webcam, document camera, etc.)
- **Display**: Interactive flat panel display or standard monitor
- **RAM**: Minimum 4 GB (8 GB recommended for HD camera feeds)
- **Disk Space**: 200 MB (for application and dependencies)
- **Camera Access**: Windows Camera privacy settings must allow desktop apps

## 🔨 Building the Application

### 1. Clone the Repository
   
   - Clone from GitHub
	   ```
	   git clone https://github.com/intel-sandbox/ifpd-touchback-floatingmenu.git
	   cd ifpd-touchback-floatingmenu
	   ```
   - Or navigate to existing clone
	
	  ```
	  cd C:\<clone-directory>\ifpd-touchback-floatingmenu
	  ```

### 2. Restore Dependencies

 - Restore NuGet packages
	
	```
	dotnet restore
	```
 - Verify restoration
	
	```
	dotnet list package
	```
  - Expected packages:
	- OpenCvSharp4
	- OpenCvSharp4.WpfExtensions
	- System.Drawing.Common

### 3. Build Release Version

 - Build in Release configuration
	```
	dotnet build -c Release
	```
 - Build output location: `bin\Release\net10.0-windows\`

### 4. Verify Build

 - Navigate to output directory
	```
	cd bin\Release\net10.0-windows
	```

 - List files
	```
	dir
	```
- Expected files:
	- FloatingMenu.exe
	- FloatingMenu.dll
	- OpenCvSharp*.dll (multiple OpenCV libraries)
	- FloatingMenu.runtimeconfig.json
	- FloatingMenu.deps.json
	- Controls, Helpers, Styles folders (if not embedded)

## 📦 Publishing Options

### Option 1: Framework-Dependent Deployment (FDD)

**Smaller size, requires .NET Desktop Runtime on target machine**

Publish for any Windows x64 machine:
```
dotnet publish -c Release -r win-x64 --no-self-contained
```
Output location: `bin\Release\net10.0-windows\win-x64\publish\`

### Option 2: Self-Contained Deployment (SCD)

**Larger size, no runtime required on target machine**

Publish with bundled runtime:
```
dotnet publish -c Release -r win-x64 --self-contained true
```
Output location: `bin\Release\net10.0-windows\win-x64\publish\`

### Option 3: Single File Executable

**All-in-one executable file (recommended for easy deployment)**

Publish as single executable:
```
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true
```
Optional: Enable compression:
```
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:EnableCompressionInSingleFile=true
```
**Note**: `IncludeNativeLibrariesForSelfExtract=true` is important for OpenCvSharp native libraries.

Output: Single FloatingMenu.exe file

### Option 4: Trimmed Deployment

**Reduced size by removing unused code**

Publish with trimming:
```
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishTrimmed=true -p:TrimMode=partial
```
## 🚀 Manual Deployment

### Step 1: Prepare Deployment Package

* Navigate to publish directory:
	```
	cd bin\Release\net10.0-windows\win-x64\publish
	```
* Create deployment folder:
	```
	$deployPath = "C:\Temp\FloatingMenu_Deployment" New-Item -Path $deployPath -ItemType Directory -Force
	```
* Copy all files:
	```
	Copy-Item -Path * -Destination $deployPath -Recurse
	```
* Optional: Create ZIP for distribution:
	```
	Compress-Archive -Path "$deployPath*" -DestinationPath "$deployPath..\FloatingMenu.zip"
	```
### Step 2: Transfer to Target Machine

Choose your transfer method:

* **Option A: USB Drive**

	Copy to USB drive:
	```
	Copy-Item -Path $deployPath -Destination "E:\FloatingMenu" -Recurse
	```
* **Option B: Email/Cloud**

	Use the ZIP file created earlier. Upload to OneDrive, Google Drive, or email.

### Step 3: Install on Target Machine

* Create installation directory:
	```
	$installPath = "C:\Program Files\FloatingMenu" New-Item -Path $installPath -ItemType Directory -Force
	```
* Copy files from deployment package:
	```	
	Copy-Item -Path "E:\FloatingMenu*" -Destination $installPath -Recurse
	```
* Or extract from ZIP:
	```
	Expand-Archive -Path "C:\Downloads\FloatingMenu.zip" -DestinationPath $installPath
	```
### Step 4: Install .NET Runtime (if using FDD)

If you published as framework-dependent, install .NET 10 Desktop Runtime:

* **Option 1: Direct download**
	```
	Start-Process "https://dotnet.microsoft.com/download/dotnet/10.0"
	```
* **Option 2: Using winget**
	```
	winget install Microsoft.DotNet.DesktopRuntime.10
	```
* **Verify installation:**
	```
	dotnet --list-runtimes
	```
	Should show: `Microsoft.WindowsDesktop.App 10.x.x`

### Step 5: Configure Camera Permissions

Enable camera access for desktop apps:

1. Open **Settings** → **Privacy & Security** → **Camera**
2. Enable **"Let desktop apps access your camera"**
3. Verify camera is working in Device Manager

### Step 6: Verify Installation

Navigate to install directory:
```
cd "C:\Program Files\FloatingMenu"
```
Test run:
```
.\FloatingMenu.exe
```
Expected behavior:
- Application starts with edge handle on right side of screen
- No error messages
- UI is responsive

## ✅ Testing the Deployment

### Test 1: Basic Functionality

* Run the application:  

* Verify:
	- ✅ Edge handle appears on right side of screen
	- ✅ Handle is draggable vertically
	- ✅ Clicking handle expands the menu
	- ✅ Menu items are visible and clickable

### Test 2: Camera Detection

1. **Ensure camera is connected** (USB or integrated)
2. **Click edge handle** to expand menu
3. **Click "Signal Source"** menu item
4. **Verify cameras are listed** (PC1, PC2, etc.)

* Check if cameras are detected:

* Test camera detection via PnP
	```
	pnputil /enum-devices /class Camera /connected
	```

### Test 3: Camera Preview

1. **Select a camera** from Signal Source list
2. **Verify**:
   - ✅ Full-screen camera window opens
   - ✅ Live camera feed displays (not black screen)
   - ✅ Frame rate is smooth (~60 FPS)
   - ✅ Resolution matches screen
   - ✅ No lag or freezing

3. **Close camera window**
4. **Verify**:
   - ✅ Camera releases properly
   - ✅ Menu returns to Signal Source view
   - ✅ Camera status changes appropriately

### Test 4: Menu Navigation

Test each menu item:
- ✅ **Home** - Collapses menu to edge handle
- ✅ **Exit** - Closes application completely
- ✅ **Signal Source** - Opens camera selection panel
- ✅ **Annotation** - Launches annotation tool (if installed)
- ✅ **Settings** - (Reserved for future use)

### Test 5: Multi-Monitor Setup (if applicable)

1. Connect multiple monitors
2. Launch FloatingMenu
3. Verify:
   - ✅ Menu docks to primary monitor
   - ✅ Camera window uses primary monitor resolution
   - ✅ Menu doesn't disappear on secondary monitor

### Test 6: Camera Switching

1. Connect **multiple cameras**
2. Select **first camera** → verify preview works
3. Close camera window
4. Select **second camera** → verify preview works
5. Verify:
   - ✅ Only one camera active at a time
   - ✅ Previous camera released properly
   - ✅ No resource leaks

## 🎯 Startup Configuration

### Option 1: Desktop Shortcut

Create a shortcut for manual startup:

1. **Right-click Desktop** → New → Shortcut
2. **Location:** 
	```
	C:\Program Files\FloatingMenu\FloatingMenu.exe
	```
3. **Name:** `Floating Menu`
4. **Optional: Change icon**
	- Right-click shortcut → Properties → Change Icon
	- Browse to FloatingMenu.exe (if it has embedded icon)

### Option 2: Windows Startup Folder

Auto-start FloatingMenu when Windows starts:

1. Press `Win + R`
2. Type `shell:startup` and press Enter
3. Create shortcut to FloatingMenu.exe in this folder