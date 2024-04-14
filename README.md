## Calculator Application

This is a simple calculator application implemented in C# using the Gtk# library for the graphical user interface (GUI). The application allows users to perform basic arithmetic operations, view calculation history, delete specific calculations, and update existing calculations.

### Features:

- **Arithmetic Operations:** Perform addition, subtraction, multiplication, and division operations.
- **Special Operations:** Calculate square roots and squares of numbers.
- **Calculation History:** View a history of previous calculations.
- **Update and Delete:** Modify existing calculations by updating operands or operators, and delete specific calculations from the history.

### Technologies Used:
- **C#:** The primary programming language used for developing the application logic.
- **Gtk#:** The graphical user interface (GUI) library used for creating the cross-platform GUI.
- **.NET Core SDK:** Required for building and running the .NET Core application.
- **MonoDevelop:** Integrated development environment (IDE) for C# development, used for writing and managing the codebase.
- **MSSQL:** Microsoft SQL Server used as the backend database for storing calculation history.
- **Azure Data Studio:** A cross-platform database tool for MSSQL, used for managing and querying the database.
### Prerequisites:
- [.NET Core SDK](https://dotnet.microsoft.com/download)
- [Gtk#](https://www.mono-project.com/download/stable/#download-lin)
- **Database Setup:** Ensure you have MSSQL installed and set up. Additionally, there is a folder named "database" in the project directory containing an SQL file (`SQLQuery.sql`) for creating the required database and table. Execute this SQL script in your MSSQL environment to create the necessary database structure before running the application. The database should be named "Calculator" for smooth operation.
### Installation:

1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/Calculator_with_Database.git
   ```
2. Navigate to the project directory:
   ```bash
   cd Calculator_with_Database
   ```
3. Build the project:
   ```bash
   dotnet build
   ```
4. Run the application:
   ```bash
   dotnet run
   ```

### Usage:

- Launch the application to open the calculator interface.
- Use the buttons to input numbers and perform operations.
- Click "=" to calculate the result.
- View calculation history by clicking the "History" button.
- Update or delete specific calculations as needed.

### Screenshots:

![Calculator App](/Screenshots/Calculator.png)
![History](/Screenshots/History.png)
![Update](/Screenshots/update.png)
![Delete](/Screenshots/Delete.png)

## Working Video
[![Demo Video](/Screenshots/gifi.gif)](/Screenshots/video.mp4)

### Contributing:

Contributions are welcome! If you'd like to contribute to this project, please follow these steps:

1. Fork the repository.
2. Create a new branch (`git checkout -b feature/new-feature`).
3. Make your changes.
4. Commit your changes (`git commit -am 'Add new feature'`).
5. Push to the branch (`git push origin feature/new-feature`).
6. Create a new Pull Request.

### License:

This project is licensed under the [MIT License](/LICENSE).

### Acknowledgements:

This project was inspired by the need for a simple yet functional calculator application. Special thanks to the Gtk# community for providing a powerful toolkit for developing cross-platform GUI applications in C#.

### Contact:

For any inquiries or feedback, feel free to contact [Alikhizar142](mailto:p229269@pwr.nu.edu.pk).

**Happy calculating!**
