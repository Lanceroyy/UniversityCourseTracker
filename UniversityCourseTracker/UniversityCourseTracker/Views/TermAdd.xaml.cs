using C971App.Services;

namespace C971App.Views;

public partial class TermAdd : ContentPage
{
	public TermAdd()
	{
		InitializeComponent();
	}

    private async void SaveTerm_OnClicked(object sender, EventArgs e)
    {
        decimal tossedDecimal;
        int tossedInt;

        if (string.IsNullOrWhiteSpace(TermName.Text))
        {
            await DisplayAlert("Missing Name", "Please enter a name", "OK");
            return;
        }
        
        if (TermSemesterPicker.SelectedIndex == -1)
        {
            await DisplayAlert("Missing Status", "Please enter a Status", "OK");
            return;
        }

        /*
        if (!Int32.TryParse(TermsInStock.Text, out tossedInt))
        {
            await DisplayAlert("Incorrect Inventory Value", "Please enter a whole number", "OK");
            return;
        }

        if (!Decimal.TryParse(TermPrice.Text, out tossedDecimal))
        {
            await DisplayAlert("Incorrect Price Value", "Please enter a whole number", "OK");
            return;
        }

        */

        if (StartDatePicker.Date > EndDatePicker.Date)
        {
            await DisplayAlert("Incorrect Dates", "Start date must be before end date", "OK");
            return;
        }


        await DatabaseService.AddTerm (
            TermName.Text, 
            TermSemesterPicker.SelectedItem.ToString(), 
            //Int32.Parse(TermsInStock.Text), 
           // Decimal.Parse(TermPrice.Text), 
            StartDatePicker.Date, 
            EndDatePicker.Date
        );

        await Navigation.PushAsync(new TermList());

    }

    private async void CancelTerm_OnClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}