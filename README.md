# University Course Tracker 📚

## Overview

University Course Tracker is a cross-platform mobile application designed to help students efficiently manage their academic terms, courses, and assessments. Built with .NET MAUI, this app provides a seamless, intuitive experience across Android, iOS, Windows, and macOS.

## 🌟 Features

### Comprehensive Course Management
- Create, update, delete, and view academic terms and courses
- Manage multiple courses within each term
- Intuitive interface for tracking academic schedules

### Secure Authentication
- Secure login with hashed PIN protection
- Easy first-time user setup
- Robust credential validation

### Powerful Reporting
- Generate detailed reports on:
  - Course information
  - Term progress
  - Assessment dates
- Visualize academic performance at a glance

### Smart Search Capabilities
- Quick filtering of courses
- Search by name, status, or date range
- Instantly find the information you need

### Cross-Platform Compatibility
- Runs on Android, iOS, Windows, and macOS
- Consistent user experience across devices

## 🚀 Getting Started

### Prerequisites
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) with .NET MAUI workload
- Git
- Android emulator or physical device

### Installation

1. Clone the repository:
   ```bash
   git clone <YOUR_REPOSITORY_URL>
   cd UniversityCourseTracker
   ```

2. Restore dependencies:
   ```bash
   dotnet restore
   ```

3. Build the application:
   ```bash
   dotnet build
   ```

4. Deploy to Android:
   ```bash
   dotnet build -t:Run -f net8.0-android
   ```

## 🧪 Testing

The application includes comprehensive unit tests using xUnit, covering:
- User authentication
- Course management
- PIN validation

## 🤝 Contributing

Contributions are welcome! Please follow these steps:
1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Submit a pull request

Please ensure your code follows project guidelines and includes appropriate tests.

## 📄 License

This project is licensed under the MIT License. See the `LICENSE` file for details.

## 📞 Contact

For inquiries or support, please contact [Your Name] at [your.email@example.com].

## 🔗 Additional Resources
- [Project Documentation](link-to-docs)
- [Issue Tracker](link-to-issues)
