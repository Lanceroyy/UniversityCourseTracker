using C971App.Services;
using C971App.Models;

namespace C971App.Views;

public partial class TermEdit : ContentPage
{
    private readonly int _selectedTermId; //Used in OnAppearing() below

	public TermEdit(Term term)
	{
		InitializeComponent();

        _selectedTermId = term.Id;

        TermId.Text = term.Id.ToString();
        TermName.Text = term.Name;
        
        TermSemesterPicker.SelectedItem = term.Semester;
        //TermsInStock.Text = term.InStock.ToString();
        //TermPrice.Text = term.Price.ToString();
        StartDatePicker.Date = term.StartDate;
        EndDatePicker.Date = term.EndDate;

	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        //TODO: Return count from a table async
        //NOTE: the await unwraps a Task<T> to a T value (T maybe a string, int, etc. In this case an int)
        //Stackoverflow: how-does-taskint-become an int-
        //Stackoverflow: 13159176

        int countCourses = await DatabaseService.GetCourseCountAsync(_selectedTermId);

        CountLabel.Text = "*You have " + countCourses.ToString() + " courses this term.*";

        CourseCollectionView.ItemsSource = await DatabaseService.GetCourses(_selectedTermId); //Retrieve Courses for a specific term based on the GadgeID
    }

    #region Term Methods
    private async void SaveTerm_OnClicked(object sender, EventArgs e)
    {
        //TODO: Not Verified?

        decimal tossedDecimal;
        int tossedInt;

        if (string.IsNullOrWhiteSpace(TermName.Text))
        {
            await DisplayAlert("Missing Name", "Please enter a name", "OK");
            return;
        }


        if (string.IsNullOrWhiteSpace(TermSemesterPicker.SelectedItem.ToString()))
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

        await DatabaseService.UpdateTerm(
            Int32.Parse(TermId.Text),
            TermName.Text,
            TermSemesterPicker.SelectedItem.ToString(),
           // Int32.Parse(TermsInStock.Text),
            //Decimal.Parse(TermPrice.Text),
            DateTime.Parse(StartDatePicker.Date.ToString()),
            DateTime.Parse(EndDatePicker.Date.ToString())
            
        );
        


        await Navigation.PopAsync();

    }

    private async void CancelTerm_OnClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void DeleteTerm_OnClicked(object sender, EventArgs e)
    {
        var answer = await DisplayAlert("Delete Term and Related Courses?", "Delete this Term?", "Yes", "No");

        if (answer == true)
        {
            var id = int.Parse(TermId.Text);

            await DatabaseService.RemoveTerm(id);

            await DisplayAlert("Term Deleted", "Term Deleted", "OK");
        }
        else
        {
            await DisplayAlert("Delete Cancelled", "Nothing Deleted", "OK");
        }
        
        await Navigation.PopAsync();
    }
    #endregion

    #region Course Methods
    private async void CourseCollectionView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var Course = (Course)e.CurrentSelection.FirstOrDefault();

        if (e.CurrentSelection != null)
        {
            await Navigation.PushAsync(new CourseEdit(Course));

        }
    }


    private async void AddCourse_OnClicked(object sender, EventArgs e)
    {

        int countCourses = await DatabaseService.GetCourseCountAsync(_selectedTermId);

        CountLabel.Text = "*You have " + countCourses.ToString() + " courses this term.*";

        //CourseCollectionView.ItemsSource = await DatabaseService.GetCourses(_selectedTermId);

        //Ensures a term can only have 6 courses
        //PART C2
        if (countCourses >= 6)
        {
            DisplayAlert("Too Many Courses", "You can only have 6 courses per term.", "OK");
            return;
        }

        
        var termId = Int32.Parse(TermId.Text);

        await Navigation.PushAsync(new CourseAdd(termId));


    }
    #endregion


}