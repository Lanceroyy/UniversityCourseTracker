using C971App.Services;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using System;

namespace C971App.Views
{
    public partial class LoginPage : ContentPage
    {
        private string enteredPin = string.Empty;

        public LoginPage()
        {
            InitializeComponent();
            CheckFirstLogin();
        }

        private async void CheckFirstLogin()
        {
            bool isFirstLogin = await DatabaseService.IsFirstLogin();

            if (isFirstLogin)
            {
                // Show setup PIN UI
                welcomeLabel.Text = "Welcome! Please set up a PIN to secure your application";
                loginButton.Text = "Set PIN";
                confirmPinEntry.IsVisible = true;
            }
            else
            {
                // Show login PIN UI
                welcomeLabel.Text = "Enter your PIN to access the application";
                loginButton.Text = "Login";
                confirmPinEntry.IsVisible = false;
            }
        }

        private async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            bool isFirstLogin = await DatabaseService.IsFirstLogin();

            if (isFirstLogin)
            {
                // PIN setup process
                string pin = pinEntry.Text;
                string confirmPin = confirmPinEntry.Text;

                if (string.IsNullOrEmpty(pin) || pin.Length < 4)
                {
                    await DisplayAlert("Error", "PIN must be at least 4 digits", "OK");
                    return;
                }

                if (pin != confirmPin)
                {
                    await DisplayAlert("Error", "PINs do not match", "OK");
                    return;
                }

                await DatabaseService.CreateUser(pin);
                await Navigation.PushAsync(new TermList());
            }
            else
            {
                // PIN validation process
                string pin = pinEntry.Text;

                if (string.IsNullOrEmpty(pin))
                {
                    await DisplayAlert("Error", "Please enter your PIN", "OK");
                    return;
                }

                bool isValid = await DatabaseService.ValidatePin(pin);

                if (isValid)
                {
                    await Navigation.PushAsync(new TermList());
                }
                else
                {
                    await DisplayAlert("Error", "Invalid PIN", "OK");
                    pinEntry.Text = string.Empty;
                }
            }
        }
    }
}