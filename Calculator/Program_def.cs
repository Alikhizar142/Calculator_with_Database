using System;
using System.Data.SqlClient;
using Gtk;
namespace CalculatorApp
{
    internal partial class Program
    {
        private static Window updateWindow; // Declare updateWindow as a class-level variable
        private static Window deleteWindow;
        private static Window confirmationWindow;

        private enum Operation
        {
            Addition,
            Subtraction,
            Multiplication,
            Division,
            Square,
            SquareRoot,
            None
        }

        private static void SaveCalculations(double operand, double result, string operator1)
        {
            if (operator1 == null)
            {
                throw new ArgumentNullException(nameof(operator1));
            }

            @operator1 = @operator1.Trim();
            string tableName = "Calculations";
            if (tableName == null)
                return; // Table name not defined

            string connectionString = "Server=localhost;Database=Calculator;User Id=SA;Password=Alikhizar@143;"; // Replace with your SQL Server connection string
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query;
                SqlCommand command;

                if (string.IsNullOrEmpty(operator1))
                {
                    Console.WriteLine("Operator is null or empty.");
                    return;
                }

                if (lastOperation == Operation.Square || lastOperation == Operation.SquareRoot)
                {

                    query = $"INSERT INTO {tableName} (operand1,operator ,operand2, result) VALUES (@operand1,@operator, @operand2, @result)";
                    command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@operand1", operand);
                    command.Parameters.AddWithValue("@operator", operator1);
                    command.Parameters.AddWithValue("@operand2", 0);
                    command.Parameters.AddWithValue("@result", result);
                }
                else
                {
                    if (operands.Length != 2)
                    {
                        Console.WriteLine("Error: Incorrect number of operands");
                        return;
                    }

                    query = $"INSERT INTO {tableName} (operand1,operator ,operand2, result) VALUES (@operand1,@operator, @operand2, @result)";
                    command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@operand1", operands[0]);
                    command.Parameters.AddWithValue("@operator", operator1);
                    command.Parameters.AddWithValue("@operand2", operands[1]);
                    command.Parameters.AddWithValue("@result", result);
                }
                command.ExecuteNonQuery();
                Console.Write("Data Written SuucessFully on DataBase");
            }
        }

        private static double[] ExtractOperands(string expression)
        {
            string[] parts = expression.Split(new char[] { '+', '-', '*', '/' }, StringSplitOptions.RemoveEmptyEntries);

            double[] operands1 = new double[parts.Length];
            for (int i = 0; i < parts.Length; i++)
            {
                if (!double.TryParse(parts[i], out operands1[i]))
                {
                    Console.WriteLine("Error parsing operand: " + parts[i]);
                    operands1[i] = double.NaN;
                }
            }
            return operands1;
        }

        private static void ShowHistoryWindow()
        {
            var historyWindow = new Window("Calculation History");
            historyWindow.SetDefaultSize(600, 400);

            var table = new TreeView(); // Create a TreeView to display the table

            // Create columns for the table
            var idColumn = new TreeViewColumn { Title = "ID" };
            var operand1Column = new TreeViewColumn { Title = "Operand1" };
            var operatorColumn = new TreeViewColumn { Title = "Operator" };
            var operand2Column = new TreeViewColumn { Title = "Operand2" };
            var resultColumn = new TreeViewColumn { Title = "Result" };

            // Add columns to the TreeView
            table.AppendColumn(idColumn);
            table.AppendColumn(operand1Column);
            table.AppendColumn(operatorColumn);
            table.AppendColumn(operand2Column);
            table.AppendColumn(resultColumn);

            // Create CellRenderers for each column
            var idCell = new CellRendererText();
            var operand1Cell = new CellRendererText();
            var operatorCell = new CellRendererText();
            var operand2Cell = new CellRendererText();
            var resultCell = new CellRendererText();

            // Add CellRenderers to columns
            idColumn.PackStart(idCell, true);
            operand1Column.PackStart(operand1Cell, true);
            operatorColumn.PackStart(operatorCell, true);
            operand2Column.PackStart(operand2Cell, true);
            resultColumn.PackStart(resultCell, true);

            // Set attributes to map data from the ListStore to the CellRenderers
            idColumn.AddAttribute(idCell, "text", 0);
            operand1Column.AddAttribute(operand1Cell, "text", 1);
            operatorColumn.AddAttribute(operatorCell, "text", 2);
            operand2Column.AddAttribute(operand2Cell, "text", 3);
            resultColumn.AddAttribute(resultCell, "text", 4);

            // Create a ListStore to hold the data
            var store = new ListStore(typeof(string), typeof(string), typeof(string), typeof(string), typeof(string));

            // Retrieve and populate the data
            RetrieveData(store);

            // Set the data store for the table
            table.Model = store;

            // Add the table to the window
            historyWindow.Add(table);

            historyWindow.ShowAll();
        }


        private static void RetrieveData(ListStore store)
        {
            string connectionString = "Server=localhost;Database=Calculator;User Id=SA;Password=Alikhizar@143;";
            string tableName = "Calculations"; // Change this to your actual table name
            string query = $"SELECT * FROM {tableName} ORDER BY ID DESC";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                   // Console.Write("Here");s
                    string id = reader["Id"].ToString();
                    string operand1 = reader["Operand1"].ToString().Trim();
                    string operand2 = reader["Operand2"].ToString().Trim();
                    string @operator = reader["Operator"].ToString().Trim();
                    string result = reader["Result"].ToString();

                    // Add data to the ListStore
                    store.AppendValues(id, operand1, @operator, operand2, result);
                }
            }
        }


        private static void ConfirmDelete(string idString)
        {
            if (int.TryParse(idString, out int id))
            {
                // Execute a SQL SELECT statement to check if the ID exists in the table
                string tableName = "Calculations";
                string connectionString = "Server=localhost;Database=Calculator;User Id=SA;Password=Alikhizar@143;";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string selectQuery = $"SELECT COUNT(*) FROM {tableName} WHERE ID = @id";
                    SqlCommand selectCommand = new SqlCommand(selectQuery, connection);
                    selectCommand.Parameters.AddWithValue("@id", id);

                    int rowCount = (int)selectCommand.ExecuteScalar();

                    if (rowCount == 0)
                    {
                        ShowConfirmationMessage($"Calculation with ID {id} does not exist.");
                        //Console.WriteLine($"Calculation with ID {id} does not exist.");
                    }
                    else
                    {
                        // Execute a SQL DELETE statement to remove the row with the specified ID
                        string deleteQuery = $"DELETE FROM {tableName} WHERE ID = @id";
                        SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection);
                        deleteCommand.Parameters.AddWithValue("@id", id);
                        deleteCommand.ExecuteNonQuery();

                        string message = $"Calculation with ID {id} deleted successfully.";
                        ShowConfirmationMessage(message);
                    }
                }
                deleteWindow.Close();
            }
            else
            {
                Console.WriteLine("Invalid ID entered. Please enter a valid integer ID.");
            }
        }

        private static string PromptDialog(string promptMessage)
        {
            // This method prompts the user with a dialog box to input data
            Console.WriteLine(promptMessage);
            return Console.ReadLine();
        }

        private static void ShowConfirmationMessage(string message)
        {
            confirmationWindow = new Window("Confirmation");
            confirmationWindow.SetDefaultSize(300, 100);

            var label = new Label(message);
            confirmationWindow.Add(label);

            confirmationWindow.ShowAll();
        }

        // Updation part start here

        private static void UpdateCalculationWindow(Calculation calculation)
        {
            updateWindow = new Window("Update Calculation");
            updateWindow.SetDefaultSize(300, 200);

            var grid = new Grid
            {
                ColumnSpacing = 4,
                RowSpacing = 4,
                ColumnHomogeneous = true,
                RowHomogeneous = true
            };
            updateWindow.Add(grid);

            var operand1Label = new Label("Operand 1:");
            grid.Attach(operand1Label, 0, 0, 1, 1);

            var operand1Entry = new Entry
            {
                Text = calculation.Operand1.ToString() // Set default value to existing operand
            };
            grid.Attach(operand1Entry, 1, 0, 1, 1);

            var operatorLabel = new Label("Operator:");
            grid.Attach(operatorLabel, 0, 1, 1, 1);

            var operatorCombo = new ComboBoxText();
            operatorCombo.AppendText("+"); // Add operator options to the combo box
            operatorCombo.AppendText("-");
            operatorCombo.AppendText("*");
            operatorCombo.AppendText("/");
            operatorCombo.AppendText("√");
            operatorCombo.AppendText("x^2");

            // Set the default value to the existing operator
            // Set the default value to the existing operator
            // Set the default value to the existing operator
            string existingOperator = calculation.Operator;
            int index = 0;
            for (int i = 0; i < operatorCombo.Model.IterNChildren(); i++)
            {
                string value = operatorCombo.ActiveText;
                if (value == existingOperator)
                {
                    index = i;
                    break;
                }
            }
            operatorCombo.Active = index;



            grid.Attach(operatorCombo, 1, 1, 1, 1);

            var operand2Label = new Label("Operand 2:");
            grid.Attach(operand2Label, 0, 2, 1, 1);

            var operand2Entry = new Entry
            {
                Text = calculation.Operand2.ToString() // Set default value to existing operand
            };
            grid.Attach(operand2Entry, 1, 2, 1, 1);

            var confirmButton = new Button("Update");
            confirmButton.Clicked += (s, args) => ConfirmUpdate(calculation.ID, operand1Entry.Text, operatorCombo.ActiveText, operand2Entry.Text);
            grid.Attach(confirmButton, 0, 3, 2, 1);

            updateWindow.ShowAll();
        }

        private static void ConfirmUpdate(int id, string operand1, string @operator, string operand2)
        {
            @operator = @operator.Trim();

            if (double.TryParse(operand1, out double operand1Value) &&
                double.TryParse(operand2, out double operand2Value))
            {
                // Execute an SQL UPDATE statement to update the data in the database
                string tableName = "Calculations";
                string connectionString = "Server=localhost;Database=Calculator;User Id=SA;Password=Alikhizar@143;";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    double result = CalculateResult(operand1Value, @operator, operand2Value);

                    connection.Open();

                    string updateQuery = $"UPDATE {tableName} SET Operand1 = @operand1, Operator = @operator, Operand2 = @operand2 ,result= @result WHERE ID = @id";
                    SqlCommand updateCommand = new SqlCommand(updateQuery, connection);
                    updateCommand.Parameters.AddWithValue("@operand1", operand1Value);
                    updateCommand.Parameters.AddWithValue("@operator", @operator);
                    updateCommand.Parameters.AddWithValue("@operand2", operand2Value);
                    updateCommand.Parameters.AddWithValue("@result", result);
                    updateCommand.Parameters.AddWithValue("@id", id);
                    updateCommand.ExecuteNonQuery();
                    ShowConfirmationMessage($"Calculation with ID {id} updated successfully. Recalculated result: {result}");
                }

                updateWindow.Close();//or updateWindow.Visible = false;
            }
            else
            {
                ShowConfirmationMessage("Invalid input for operands. Please enter valid numeric values.");
            }
        }

        private static double CalculateResult(double operand1, string @operator, double operand2)
        {
            switch (@operator)
            {
                case "+":
                    return operand1 + operand2;
                case "-":
                    return operand1 - operand2;
                case "*":
                    return operand1 * operand2;
                case "/":
                    return operand1 / operand2;
                case "x^2":
                    return operand1 * operand1;
                case "√":
                    return Math.Sqrt(operand1);
                default:
                    throw new ArgumentException($"Invalid operator: '{@operator}'");
            }
        }

        private static Calculation GetCalculationByID(int id)
        {
            // Initialize a Calculation object
            Calculation calculation = null;

            // Define your SQL query
            string tableName = "Calculations";
            string connectionString = "Server=localhost;Database=Calculator;User Id=SA;Password=Alikhizar@143;";
            string query = $"SELECT * FROM {tableName} WHERE ID = @id";

            // Create a connection to the database and execute the query
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Create a command with parameters to prevent SQL injection
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);

                // Execute the query and read the results
                SqlDataReader reader = command.ExecuteReader();

                // Check if the query returned any results
                if (reader.Read())
                {
                    // Construct a Calculation object from the retrieved data
                    calculation = new Calculation
                    {
                        ID = (int)reader["ID"],
                        Operand1 = Convert.ToDouble(reader["Operand1"]),
                        Operator = (string)reader["Operator"],
                        Operand2 = Convert.ToDouble(reader["Operand2"]),
                        Result = Convert.ToDouble(reader["Result"])
                    };
                }
            }

            // Return the constructed Calculation object
            return calculation;
        }

        private class Calculation
        {
            // Define properties for a Calculation object
            public int ID { get; set; }
            public double Operand1 { get; set; }
            public string Operator { get; set; }
            public double Operand2 { get; set; }
            public double Result { get; set; }
        }

    }
}