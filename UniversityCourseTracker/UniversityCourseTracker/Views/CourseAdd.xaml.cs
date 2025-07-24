using C971App.Services;

namespace C971App.Views;

public partial class CourseAdd : ContentPage
{
    private readonly int _selectedTermId;

	public CourseAdd()
	{
		InitializeComponent();
	}

    public CourseAdd(int TermId)
    {
        InitializeComponent();

        _selectedTermId = TermId;
    }

    private async void CancelCourse_OnClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void SaveCourse_OnClicked(object sender, EventArgs e)
    {

        decimal tossedDecimal;
        int tossedInt;
        long tossedLong;

        if (string.IsNullOrWhiteSpace(CourseName.Text))
        {
            await DisplayAlert("Missing Name", "Please enter a name", "OK");
            return;
        }

        if (StatusPicker.SelectedIndex == -1)
        {
            await DisplayAlert("Missing Status", "Please enter a Status", "OK");
            return;
        }

        /*
        if (!Int32.TryParse(CourseInStock.Text, out tossedInt))
        {
            await DisplayAlert("Incorrect Inventory Value", "Please enter a whole number", "OK");
            return;
        }

        if (!Decimal.TryParse(CoursePrice.Text, out tossedDecimal))
        {
            await DisplayAlert("Incorrect Price Value", "Please enter a whole number", "OK");
            return;
        }
        */

        if (StartDatePicker.Date > EndDatePicker.Date)
        {
            await DisplayAlert("Incorrect Date Range", "Start Date must be before End Date", "OK");
            return;
        }


        if (string.IsNullOrWhiteSpace(InstructorName.Text))
        {
            await DisplayAlert("Missing Instructor Name", "Please enter a name", "OK");
            return;
        }

        if (string.IsNullOrEmpty(InstructorPhone.Text))
        {
            await DisplayAlert("Missing Instructor Phone", "Please enter a phone number", "OK");
            return;
        }

        if (InstructorPhone.Text.Length != 12 && !InstructorPhone.Text.Contains("-"))
        {
            await DisplayAlert("Incorrect Phone Number", "Please enter a 10 digit number with hyphens Ex: '123-456-7890'", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(InstructorEmail.Text))
        {
            await DisplayAlert("Missing Instructor Email", "Please enter an email", "OK");
            return;
        }

        if (!InstructorEmail.Text.Contains("@") || !InstructorEmail.Text.Contains("."))
        {
            await DisplayAlert("Incorrect Email", "Please enter a valid email", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(PerformanceAssessmentName.Text))
        {
            await DisplayAlert("Missing Performance Assessment Name", "Please enter a name", "OK");
            return;
        }

        if (PerformanceAssessmentStartDate.Date > PerformanceAssessmentDueDate.Date)
        {
            await DisplayAlert("Incorrect Date Range", "Start Date must be before End Date", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(ObjectiveAssessmentName.Text))
        {
            await DisplayAlert("Missing Objective Assessment Name", "Please enter a name", "OK");
            return;
        }

        if (ObjectiveAssessmentStartDate.Date > ObjectiveAssessmentDueDate.Date)
        {
            await DisplayAlert("Incorrect Date Range", "Start Date must be before End Date", "OK");
            return;
        }


        await DatabaseService.AddCourse(
            _selectedTermId, 
            CourseName.Text, 
            StatusPicker.SelectedItem.ToString(),

            StartDatePicker.Date, 
            EndDatePicker.Date, 

            InstructorName.Text, 
            (InstructorPhone.Text), 
            InstructorEmail.Text,

            Notification.IsToggled, 
            NotesEditor.Text,

            PerformanceAssessmentName.Text, 
            PerformanceAssessmentStartDate.Date, 
            PerformanceAssessmentDueDate.Date,
            PerformanceAssessmentNotification.IsToggled,

            ObjectiveAssessmentName.Text, 
            ObjectiveAssessmentStartDate.Date, 
            ObjectiveAssessmentDueDate.Date,
            ObjectiveAssessmentNotification.IsToggled

            //Int32.Parse(CourseInStock.Text), 
            //Decimal.Parse(CoursePrice.Text), 

        );

        await Navigation.PopAsync();
    }

    private async void Home_OnClicked(object sender, EventArgs e)
    {
        await Navigation.PopToRootAsync();

    }


    private async void PhoneEntry_OnTextChanged(object sender, EventArgs e)
    {
        Color orange = new Color(255, 165, 0);
        Color white = new Color(255, 255, 255);

        if (InstructorPhone.Text.Length < 12)
        {
            InstructorPhone.BackgroundColor = orange;
        }
        else
        {
            InstructorPhone.BackgroundColor = white;
        }
    }
}