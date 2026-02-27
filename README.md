#  Location Heat Map App

A cross-platform mobile application built using **.NET MAUI** that
tracks user location over time and visualizes movement data as a
heat-style overlay on a map.

This project demonstrates real-time geolocation tracking, local database
storage using SQLite, and dynamic map rendering using .NET MAUI Maps.

------------------------------------------------------------------------

##  Features

-    Real-time GPS location tracking
-    Interactive map display using Google Maps (Android)
-    Heat-style visualization using layered map circles
-    Local storage using SQLite
-    Start / Stop tracking functionality
-    Clear saved location history
-    Runtime location permission handling
-    <img width="540" height="1060" alt="Screenshot 2026-02-26 223922" src="https://github.com/user-attachments/assets/d6ac6569-799b-4b63-97fd-c4ff3da2ad97" />
-    <img width="1671" height="1290" alt="Screenshot 2026-02-26 223954" src="https://github.com/user-attachments/assets/b3e4db44-d1db-4836-8854-480227a9ab41" />


------------------------------------------------------------------------

##  Technologies Used

-   .NET MAUI (Multi-platform App UI)
-   Microsoft.Maui.Controls.Maps
-   SQLite (sqlite-net-pcl)
-   Android Emulator (API 30+)
-   Visual Studio 2022

------------------------------------------------------------------------

##  Project Structure

LocationHeatMapApp/ │ ├── Models/ │ └── LocationPoint.cs │ ├── Services/
│ └── LocationDb.cs │ ├── MainPage.xaml ├── MainPage.xaml.cs ├──
AppShell.xaml ├── MauiProgram.cs └── LocationHeatMapApp.csproj

------------------------------------------------------------------------

## ⚙️ How It Works

1.  When the app starts, it requests location permission.
2.  Once granted, the user can press **Start** to begin tracking.
3.  Every 5 seconds:
    -   The device GPS location is captured.
    -   The coordinates are stored in a local SQLite database.
4.  The map refreshes dynamically.
5.  Each saved coordinate renders two semi-transparent circles:
    -   Outer red circle
    -   Inner blue circle
6.  Over time, overlapping circles simulate a heat map effect.

------------------------------------------------------------------------

##  Running the App

### Requirements

-   Visual Studio 2022 (with .NET MAUI workload installed)
-   Android SDK
-   Android Emulator

### Steps

1.  Clone the repository:

    git clone [https://github.com/sbhetwal11/MultiPlatform_Location_Tracker_Apk_With_NET_MAUI]  

2.  Open the solution in Visual Studio.

3.  Set startup target to: net8.0-android

4.  Select Android Emulator.

5.  Click Run 

------------------------------------------------------------------------

##  Permissions

Android location permission is requested at runtime:

Permissions.LocationWhenInUse

Required for tracking GPS coordinates.

------------------------------------------------------------------------

##  Database

The app uses SQLite to store:

-   Latitude
-   Longitude
-   Timestamp (UTC)

Stored locally inside the app's application data directory.

------------------------------------------------------------------------

##  Learning Objectives

This project demonstrates:

-   Dependency Injection in MAUI
-   Map rendering with MapElements
-   Async location tracking
-   SQLite integration
-   Android permission handling
-   Emulator debugging & crash resolution

------------------------------------------------------------------------

##  Author

Sagar Bhetwal\
Software Engineering and Multiplatform App Development (MSCS-533-A01) - First Bi-term  
Final Software Development Project: Build Multiplatform “Location Tracker" App with .NET MAUI  

------------------------------------------------------------------------

##  License

This project is for academic and educational purposes.
