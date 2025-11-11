# 🎵 Music Library Search App

A cross-platform **.NET MAUI** app to manage your music library. View, search, add, edit, and delete songs stored in JSON.

## 🚀 Features

- View table: ID, Title, Artist, Album, Release Year, Genre
- Search by Title, Artist, or Genre
- Add, Edit, Delete songs by ID
- Load/Save library as JSON
- Reset form & navigate pages

**Prerequisites:** [.NET 7 SDK](https://dotnet.microsoft.com/download/dotnet/7.0), Visual Studio 2022+ with **.NET MAUI workload**

## 📁 JSON Structure

Example song: `{ "Id": 1, "Title": "Song Title", "Artist": "Artist Name", "Album": "Album Name", "ReleaseYear": "2023", "Genre": "Pop" }`

## 🛠 Technologies

- .NET MAUI
- C# & XAML
- JSON Serialization

## 📌 Notes

- Each song must have a unique ID
- Only JSON files are supported for import/export

## 📷 Screenshots

![MainPage](screenshots/main.png)
![Search](screenshots/search.png)
![Change](screenshots/change.png)
