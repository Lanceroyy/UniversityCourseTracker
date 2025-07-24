using System;
using System.Collections.Generic;
using System.Linq;
using C971App.Models;
using C971App.Services;
using Microsoft.Maui.Controls.Xaml;

namespace C971App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReportPage : ContentPage
    {
        private List<CourseDisplayModel> allCourses;
        private List<CourseDisplayModel> filteredCourses;

        public ReportPage()
        {
            InitializeComponent();

            // Initialize date pickers with reasonable defaults
            var today = DateTime.Today;
            StartDatePicker.Date = today.AddMonths(-3);  // 3 months ago
            EndDatePicker.Date = today.AddMonths(6);     // 6 months in future

            LoadData();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            // Refresh data when page appears
            LoadData();
        }

        private async void LoadData()
        {
            try
            {
                // Get all courses and terms
                var courses = (await DatabaseService.GetCourses()).ToList();
                var terms = (await DatabaseService.GetTerms()).ToList();

                // Create a dictionary for quick term lookups
                var termDict = terms.ToDictionary(t => t.Id, t => t.Name);

                // Create display models with term names
                allCourses = courses.Select(c => new CourseDisplayModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Status = c.Status,
                    StartDate = c.StartDate,
                    EndDate = c.EndDate,
                    TermId = c.TermId,
                    TermName = termDict.TryGetValue(c.TermId, out var termName) ? termName : "Unassigned"
                }).ToList();

                FilterCourses();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load courses: {ex.Message}", "OK");
            }
        }

        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            FilterCourses();
        }

        private void OnDateSelected(object sender, DateChangedEventArgs e)
        {
            // Ensure end date is not before start date
            if (EndDatePicker.Date < StartDatePicker.Date)
            {
                EndDatePicker.Date = StartDatePicker.Date;
            }

            FilterCourses();
        }

        private void FilterCourses()
        {
            if (allCourses == null) return;

            string searchText = SearchBar.Text?.ToLower() ?? "";
            DateTime startDate = StartDatePicker.Date;
            DateTime endDate = EndDatePicker.Date.AddDays(1).AddSeconds(-1); // End of the selected day

            filteredCourses = allCourses
                .Where(c => c.Name.ToLower().Contains(searchText) &&
                           (c.StartDate >= startDate && c.StartDate <= endDate ||
                            c.EndDate >= startDate && c.EndDate <= endDate ||
                            c.StartDate <= startDate && c.EndDate >= endDate))
                .ToList();

            ReportCollectionView.ItemsSource = filteredCourses;

            // Update count label similar to your TermEdit page
            CountLabel.Text = filteredCourses.Count == 1
                ? "Showing 1 course"
                : $"Showing {filteredCourses.Count} courses";
        }

        private async void ReportCollectionView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is CourseDisplayModel selectedCourse)
            {
                // Clear selection
                ReportCollectionView.SelectedItem = null;

                // Navigate to course details or show details
                // For example:
                // await Navigation.PushAsync(new CourseDetailPage(selectedCourse.Id));

                // Or display course details in an alert
                await DisplayAlert(selectedCourse.Name,
                    $"Status: {selectedCourse.Status}\n" +
                    $"Term: {selectedCourse.TermName}\n" +
                    $"Start Date: {selectedCourse.StartDate:MM/dd/yyyy}\n" +
                    $"End Date: {selectedCourse.EndDate:MM/dd/yyyy}\n",
                    "Close");
            }
        }
    }

    // Display model to hold the term name as a string
    public class CourseDisplayModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TermId { get; set; }
        public string TermName { get; set; } // Added property for term name
    }
}