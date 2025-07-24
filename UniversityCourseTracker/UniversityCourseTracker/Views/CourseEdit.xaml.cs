using C971App.Models;
using C971App.Services;

namespace C971App.Views;

public partial class CourseEdit : ContentPage
{

   
	public CourseEdit(Course selectedCourse)
	{
		InitializeComponent();

        //Populate Controls Next
        CourseId.Text = selectedCourse.Id.ToString();
        CourseName.Text = selectedCourse.Name;
        CourseColorPicker.SelectedItem = selectedCourse.Status;
        NotesEditor.Text = selectedCourse.Notes;
        StartDatePicker.Date = selectedCourse.StartDate;
        EndDatePicker.Date = selectedCourse.EndDate;
        Notification.IsToggled = selectedCourse.StartNotification;

        InstructorName.Text = selectedCourse.InstructorName;
        InstructorPhone.Text = selectedCourse.InstructorPhone;
        InstructorEmail.Text = selectedCourse.InstructorEmail;
        
        //Part C4
        PerformanceAssessmentName.Text = selectedCourse.PerformanceAssessmentName;
        PerformanceAssessmentStartDate.Date = selectedCourse.PerformanceAssessmentStartDate;
        PerformanceAssessmentDueDate.Date = selectedCourse.PerformanceAssessmentDueDate;
        PerformanceAssessmentNotification.IsToggled = selectedCourse.PerformanceAssessmentNotification;

        ObjectiveAssessmentName.Text = selectedCourse.ObjectiveAssessmentName;
        ObjectiveAssessmentStartDate.Date = selectedCourse.ObjectiveAssessmentStartDate;
        ObjectiveAssessmentDueDate.Date = selectedCourse.ObjectiveAssessmentDueDate;
        ObjectiveAssessmentNotification.IsToggled = selectedCourse.ObjectiveAssessmentNotification;

        //CourseInStock.Text = selectedCourse.InStock.ToString();
        //CoursePrice.Text = selectedCourse.Price.ToString();


    }

    private async void ShareUri_OnClicked(object sender, EventArgs e)
    {
        string uri = "https://learn.microsoft.com/en-us/dotnet/maui/platform-integration/data/share?view=net-maui-8.0&tabs=windows";

        await Share.RequestAsync(new ShareTextRequest
        {
            Uri = uri,
            Title = "Share Web Link"
        });
    }

    private async void ShareButton_OnClicked(object sender, EventArgs e)
    {
        var text = NotesEditor.Text;

        await Share.RequestAsync(new ShareTextRequest(text)
        {
            Text = text,
            Title = "Share Text"
        });
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

        if (CourseColorPicker.SelectedIndex == -1)
        {
            await DisplayAlert("Missing Status", "Please enter a Status", "OK");
            return;
        }


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



        await DatabaseService.UpdateCourse (
            Int32.Parse(CourseId.Text), 
            CourseName.Text, 
            CourseColorPicker.SelectedItem.ToString(),
            StartDatePicker.Date, 
            EndDatePicker.Date, 

            InstructorName.Text,
            InstructorPhone.Text,
            InstructorEmail.Text,

            Notification.IsToggled, 
            NotesEditor.Text,

            //Part C4
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
    }

    private async void CancelCourse_OnClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void DeleteCourse_OnClicked(object sender, EventArgs e)
    {
        var answer = await DisplayAlert("Delete Course?", "Delete this Course?", "Yes", "No");

        if (answer == true)

        {
            var id = int.Parse(CourseId.Text);

            await DatabaseService.RemoveCourse(id);
            
            await DisplayAlert("Course Deleted", "Course Deleted", "OK");
        }
        else
        {
            await DisplayAlert("Delete Canceled", "Nothing Deleted", "OK");
        }

        await Navigation.PopAsync();
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