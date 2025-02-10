Here is the documentation for the Unity Wallpaper Manager project:

# 📱 Unity Wallpaper Manager

## 🚀 Overview
The Unity Wallpaper Manager allows users to select, preview, and set wallpapers on their devices. It supports both online and offline wallpapers, with additional features like full-screen previews and preference saving.

🔗 Video Trailer

https://youtube.com/shorts/5LZUTMMCAg0?si=tun4acErvUPg5gAu

## 🎯 Features
- 🌐 Online & Offline Wallpapers
- 🖼️ Full-Screen Preview
- 🛠️ User Preferences (Fixed/Rotational Wallpapers)
- 📂 Save and Apply Wallpapers
- 🎭 Android & Windows Support

## 🏗️ Components
### **1. OfflineModeManager.cs**
🔹 Checks if the device is offline and restricts online-dependent actions.

### **2. PageContent.cs**
📜 Scriptable object for storing page content such as titles and descriptions.

### **3. PexelsImageFetcher.cs**
📸 Fetches curated images from the Pexels API and downloads them for use as wallpapers.

### **4. SettingsManager.cs**
⚙️ Manages user preferences for wallpaper types and saves settings using PlayerPrefs.

### **5. SplashScreenManager.cs**
⏳ Handles the splash screen and transitions to the home page.

### **6. WallpaperManager.cs**
🖥️ Manages the wallpaper selection, preview, and setting functionalities. Handles Android-specific permissions.

### **7. WallpaperDownload.cs**
💾 Saves and sets wallpapers for both Android and Windows platforms. Uses system APIs for desktop wallpaper changes.

### **8. WallpaperSelectionManager.cs**
📂 Dynamically loads wallpapers from local storage and online sources, displaying them in the UI.

## 🔧 Installation & Usage
1. 📥 **Import into Unity**: Add scripts to an existing Unity project.
2. 🎨 **Setup UI**: Ensure UI components like buttons and image panels are linked.
3. 🔑 **API Key**: If using online wallpapers, insert your Pexels API key.
4. ▶️ **Run the Project**: Test functionalities and tweak UI as needed.

## 🛠️ Future Enhancements
- 🌙 Dark mode UI
- 📶 Improved offline handling
- 🎞️ Live wallpaper support

Enjoy customizing your wallpapers with Unity Wallpaper Manager! 🚀

