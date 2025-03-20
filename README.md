# CLI Code Packaging Tool

## Overview
This project is a **Command-Line Interface (CLI)** tool developed in **.NET** for efficiently managing and packaging multiple code files. The tool provides a streamlined way to create commands and tokens for organizing, compressing, and deploying code files in a structured manner.

## Features
- **Command Creation**: Define and execute custom commands for managing code files.
- **Token Management**: Generate and utilize tokens for secure and efficient file packaging.
- **Multi-File Packaging**: Combine multiple code files into a single, deployable package.
- **Cross-Platform Compatibility**: Works seamlessly on Windows, macOS, and Linux.
- **Extensibility**: Easily extend the tool with additional commands or features.

## Installation
1. Clone the repository:
   ```bash
   git clone https://github.com/sari-Yona/CLIBundleFiles
   ```
2. Navigate to the project directory:
   ```bash
   cd cli-code-packaging-tool
   ```
3. Build the project using the .NET CLI:
   ```bash
   dotnet build
   ```

## Usage
1. Run the CLI tool:
   ```bash
   dotnet run -- [command] [options]
   ```
2. Example commands:
   - **Package files**:
     ```bash
     dotnet run -- package --input "src/" --output "dist/package.zip"
     ```
   - **Generate token**:
     ```bash
     dotnet run -- token --generate
     ```

## Commands
| Command       | Description                                   | Example Usage                              |
|---------------|-----------------------------------------------|-------------------------------------------|
| `package`     | Packages multiple code files into a single archive. | `dotnet run -- package --input "src/" --output "dist/package.zip"` |
| `token`       | Generates or validates tokens for packaging.  | `dotnet run -- token --generate`          |

## Project Structure
```
prj/
├── src/                # Source code files
├── dist/               # Output directory for packaged files
├── .gitignore          # Git ignore rules
├── README.md           # Project documentation
├── Program.cs          # Main entry point for the CLI tool
└── CLICommands/        # Directory for custom command implementations
```

## Contributing
Contributions are welcome! Please follow these steps:
1. Fork the repository.
2. Create a new branch for your feature or bug fix:
   ```bash
   git checkout -b feature-name
   ```
3. Commit your changes and push the branch:
   ```bash
   git push origin feature-name
   ```
4. Open a pull request.


## Acknowledgments
- Developed as part of the **2024 Practical Engineering Program**.
- Special thanks to the mentors and peers for their guidance and support.
