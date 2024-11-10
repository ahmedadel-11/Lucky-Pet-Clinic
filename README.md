# Lucky Pet Clinic 🐾

Lucky Pet Clinic is a comprehensive veterinary clinic management application designed to streamline the management of pet information, client data, examinations, surgeries, and vaccinations. With a user-friendly interface and robust database integration, it helps veterinary professionals organize and access essential records efficiently.


## Features

- **Client Management** 🧑‍🤝‍🧑: Add, update, and remove client information to keep client data organized and accessible.
- **Pet Management** 🐕🐈: Manage pet profiles to track pets associated with the clinic.
- **Examination Management** 🩺: Record and retrieve examination details to maintain comprehensive medical records.
- **Surgery Management** 🏥: Log surgical details, enabling easy access to a complete surgical history.
- **Vaccination Management** 💉: Record vaccination details to keep vaccination history up-to-date.
- **File Management** 📂: Open and manage files related to pets and examinations for organized documentation.
- **Database Integration** 🗄️: Leverages SQL Server for secure and efficient data storage.
- **Error Handling** ⚠️: Comprehensive error handling with logging for prompt issue identification.
- **Dark & Light Theme Support** 🌗: Switch between dark and light themes for optimal comfort in different lighting environments.

![Dashboard Screenshot](https://github.com/user-attachments/assets/bdeab1cb-f474-43c1-8a7a-804a306713f9)

---

## Getting Started

Follow these steps to set up and run the Lucky Pet Clinic application.

### Prerequisites

- [.NET 5.0](https://dotnet.microsoft.com/download/dotnet/5.0) or later
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

### Installation

1. **Clone the repository**:
   ```bash
   git clone https://github.com/your-repo/lucky-pet-clinic.git
   ```
2. **Navigate to the project directory**:
   ```bash
   cd lucky-pet-clinic
   ```
3. **Restore dependencies**:
   ```bash
   dotnet restore
   ```

### Configuration

1. **Update the SQL connection string** in `AppDbContextFactory.cs` to match your SQL Server configuration:
   ```csharp
   var connectionString = ConfigurationManager.ConnectionStrings["RemoteCS"].ConnectionString;
   ```

### Running the Application

1. **Build the project**:
   ```bash
   dotnet build
   ```
2. **Run the project**:
   ```bash
   dotnet run
   ```


---

## Usage

Here’s a quick guide to navigating the main features of Lucky Pet Clinic:

- **Add Client**: Go to **Add Client**, enter client details, and save.  
  ![Add Client Screenshot](https://github.com/user-attachments/assets/34d205c7-b610-45d6-ad54-fdbd8d7df5a0)

- **Add Pet**: Navigate to **Add Pet**, fill in the pet details, and save.  
  ![Add Pet Screenshot](https://github.com/user-attachments/assets/29525c03-58fa-4fe9-8f99-9b8025586d2f)

- **Pets**: Navigate to **Pets**, all pets data.  
  ![Pets Screenshot](https://github.com/user-attachments/assets/2e23f69c-ff3c-48b4-9f0a-47a6ca825ff7)

- **Examination**: Go to **Examinations**, enter details, and save the record.  
  ![Examination Page Screenshot](https://github.com/user-attachments/assets/dde373cc-bd30-4c59-82a1-c742f6fe4adc)

- **Surgery**: In **Surgeries**, fill in surgery information and save.  
  ![Surgery Page Screenshot](https://github.com/user-attachments/assets/43a77539-52c9-4bed-97aa-1f8dc9cd680b)

- **Vaccination**: Go to **Vaccinations**, enter details, and save.  
  ![Vaccination Page Screenshot](https://github.com/user-attachments/assets/d77deaee-97e6-4c4a-ad97-63b609f1e67b)


---

## Database Schema

Include a visual schema for the database structure to provide insight into the data relationships within the application.

![Database Schema](https://github.com/user-attachments/assets/f8e70bd6-ebbc-4c30-8b06-8ffee1addea2)

---

## Contributing

We welcome contributions! To contribute:

1. Fork the repository.
2. Create a new branch for your feature (`git checkout -b feature-name`).
3. Commit your changes (`git commit -m 'Add new feature'`).
4. Push to the branch (`git push origin feature-name`).
5. Open a pull request.

---

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

