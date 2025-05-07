using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;

namespace ChatbotApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string userMessage = UserInput.Text.Trim(); // Access the TextBox by its Name
            if (string.IsNullOrEmpty(userMessage)) return;

            ChatBox.Items.Add("You: " + userMessage); // Access the ListBox by its Name
            string botReply = RunPythonChatbot(userMessage);
            ChatBox.Items.Add("Bot: " + botReply);

            UserInput.Clear(); // Clear the user input TextBox after sending
        }

        private string RunPythonChatbot(string message)
        {
            var psi = new ProcessStartInfo
            {
                FileName = "python", // Path to Python
                Arguments = $"chat_once.py \"{message}\"", // Python script args
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = @"C:\Users\tech\Pictures\Deepseek" // Path to where your Python script is
            };

            try
            {
                var process = Process.Start(psi);
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                if (!string.IsNullOrWhiteSpace(error))
                    return "Python Error: " + error;

                return output.Trim();
            }
            catch (Exception ex)
            {
                return "C# Error: " + ex.Message;
            }
        }
    }
}
