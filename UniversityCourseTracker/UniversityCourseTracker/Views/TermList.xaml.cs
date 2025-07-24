using C971App.Models;
using C971App.Services;
using Plugin.LocalNotification;


namespace C971App.Views;

public partial class TermList : ContentPage
{
	public TermList()
	{
		InitializeComponent();
	}

	private async void AddTerm_OnClicked(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new TermAdd());
	}

    //TODO: Do not show these buttons to the evaluator in the final version
    private async void ClearDatabase_OnClicked(object sender, EventArgs e)
    {
		//TODO: Implement ClearDatabase_OnClicked
		await DatabaseService.ClearSampleData();

		await RefreshTermCollectionView();
    }

	//TODO: Do not show these buttons to the evaluator in the final version
    private async void LoadSampleData_OnClicked(object sender, EventArgs e)
    {
        //Clear the database before loading
        await DatabaseService.ClearSampleData();


        try
        {
            if (Settings.FirstRun) //To Prevent Sample data from loading more than once.
            {
                await DatabaseService.LoadSampleData();

                await DisplayAlert("Sample Data Loaded", "Sample Data Loaded", "OK");

            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }


        await RefreshTermCollectionView();

    }
    
    private async void TermCollectionView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		if (e.CurrentSelection != null) 
		{
			Term term = (Term)e.CurrentSelection.FirstOrDefault();

			await Navigation.PushAsync(new TermEdit(term));
		}
	}

	private async Task RefreshTermCollectionView()
	{
		//Thread.Sleep(0);

        TermCollectionView.ItemsSource = await DatabaseService.GetTerms();
	}

	protected override async void OnAppearing()
    {
		base.OnAppearing();

		if (Services.Settings.FirstRun)
		{
			await DatabaseService.LoadSampleData();

            Services.Settings.FirstRun = false;

			await RefreshTermCollectionView();
		}

		await RefreshTermCollectionView();

		ShowCourseNotifications();
    }

    private async Task ShowCourseNotifications()
    {
        // Check if notifications are enabled
        if (await LocalNotificationCenter.Current.AreNotificationsEnabled() == false)
        {
            await LocalNotificationCenter.Current.RequestNotificationPermission();
        }

        var courseList = (await DatabaseService.GetCourses()).ToList();
        var preferences = Preferences.Default;
        var lastNotificationTime = preferences.Get("LastNotificationTime", DateTime.MinValue);

        // Only proceed if more than 30 minutes have passed since last notification
        if (DateTime.Now.Subtract(lastNotificationTime).TotalHours < 12)
        {
            return;
        }

        // Build a consolidated list of all notification messages
        List<string> notificationMessages = new List<string>();

        foreach (Course courseRecord in courseList)
        {
            // Course Start
            if (courseRecord.StartNotification &&
                courseRecord.StartDate.Date >= DateTime.Today.AddDays(-8))
            {
                notificationMessages.Add($"• {courseRecord.Name} starts soon");
            }

            // Course End
            if (courseRecord.StartNotification &&
                courseRecord.EndDate.Date >= DateTime.Today.AddDays(-8))
            {
                notificationMessages.Add($"• {courseRecord.Name} ends soon");
            }

            // Performance Assessment Start
            if (courseRecord.PerformanceAssessmentNotification &&
                courseRecord.PerformanceAssessmentStartDate.Date >= DateTime.Today.AddDays(-8))
            {
                notificationMessages.Add($"• {courseRecord.PerformanceAssessmentName} assessment starts soon");
            }

            // Performance Assessment Due
            if (courseRecord.PerformanceAssessmentNotification &&
                courseRecord.PerformanceAssessmentDueDate.Date >= DateTime.Today.AddDays(-8))
            {
                notificationMessages.Add($"• {courseRecord.PerformanceAssessmentName} assessment is due soon");
            }

            // Objective Assessment Start
            if (courseRecord.ObjectiveAssessmentNotification &&
                courseRecord.ObjectiveAssessmentStartDate.Date >= DateTime.Today.AddDays(-8))
            {
                notificationMessages.Add($"• {courseRecord.ObjectiveAssessmentName} assessment starts soon");
            }

            // Objective Assessment Due
            if (courseRecord.ObjectiveAssessmentNotification &&
                courseRecord.ObjectiveAssessmentDueDate.Date >= DateTime.Today.AddDays(-8))
            {
                notificationMessages.Add($"• {courseRecord.ObjectiveAssessmentName} assessment is due soon");
            }
        }

        // Only send a notification if we have messages
        if (notificationMessages.Count > 0)
        {
            // Create a nicely formatted description with all events
            string title = notificationMessages.Count == 1
                ? "Upcoming Event"
                : "Upcoming Events";

            string subtitle = notificationMessages.Count == 1
                ? "You have 1 upcoming deadline"
                : $"You have {notificationMessages.Count} upcoming deadlines";

            string description = string.Join("\n", notificationMessages);

            // Create and send the consolidated notification
            var notification = new NotificationRequest
            {
                NotificationId = new Random().Next(1000),
                Title = title,
                Subtitle = subtitle,
                Description = description,
                ReturningData = "Consolidated course notification",
                BadgeNumber = notificationMessages.Count,
                Schedule = new NotificationRequestSchedule()
                {
                    NotifyTime = DateTime.Now.AddSeconds(2)
                }
            };

            await LocalNotificationCenter.Current.Show(notification);

            // Update the last notification time
            preferences.Set("LastNotificationTime", DateTime.Now);
        }
    }

    private (bool shouldNotify, string title, string subtitle, string description, DateTime? dateToCheck)
        GetNotificationParameters(string notificationType, Course courseRecord)
    {
        switch (notificationType)
        {
            case "CourseStart":
                return (
                    courseRecord.StartNotification,
                    "Course Notification",
                    "Starts Soon.",
                    $"{courseRecord.Name} starts soon!",
                    courseRecord.StartDate
                );

            case "CourseEnd":
                return (
                    courseRecord.StartNotification,
                    "Course Notification",
                    "Ends Soon.",
                    $"{courseRecord.Name} ends soon!",
                    courseRecord.EndDate
                );

            case "PerformanceStart":
                return (
                    courseRecord.PerformanceAssessmentNotification,
                    "Performance Assessment Notification",
                    "Start Date.",
                    $"{courseRecord.PerformanceAssessmentName} starts soon!",
                    courseRecord.PerformanceAssessmentStartDate
                );

            case "PerformanceDue":
                return (
                    courseRecord.PerformanceAssessmentNotification,
                    "Performance Assessment Notification",
                    "Due Date",
                    $"{courseRecord.PerformanceAssessmentName} is due soon!",
                    courseRecord.PerformanceAssessmentDueDate
                );

            case "ObjectiveStart":
                return (
                    courseRecord.ObjectiveAssessmentNotification,
                    "Objective Assessment Notification",
                    "Start Date.",
                    $"{courseRecord.ObjectiveAssessmentName} starts soon!",
                    courseRecord.ObjectiveAssessmentStartDate
                );

            case "ObjectiveDue":
                return (
                    courseRecord.ObjectiveAssessmentNotification,
                    "Objective Assessment Notification",
                    "Due Date.",
                    $"{courseRecord.ObjectiveAssessmentName} is due soon!",
                    courseRecord.ObjectiveAssessmentDueDate
                );

            default:
                return (false, string.Empty, string.Empty, string.Empty, null);
        }
    }

    private int GetDelayForType(string notificationType)
    {
        // Stagger notifications by type to avoid them all appearing simultaneously
        return notificationType switch
        {
            "CourseStart" => 2,
            "CourseEnd" => 4,
            "PerformanceStart" => 6,
            "PerformanceDue" => 8,
            "ObjectiveStart" => 10,
            "ObjectiveDue" => 12,
            _ => 2
        };
    }



    //TODO : Do not show these buttons to the evaluator in the final version
    private async void TestButton_OnClicked(object sender, EventArgs e)
    {
        await DatabaseService.TestMethod(this);
    }

    private async void ReportPage_OnClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ReportPage());

    }
}