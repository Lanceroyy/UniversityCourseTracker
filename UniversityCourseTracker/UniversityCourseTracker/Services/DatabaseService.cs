using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SQLite;
using C971App.Models;
using Microsoft.Maui.Controls;



namespace C971App.Services
{
    public class DatabaseService
    {
        private static SQLiteAsyncConnection _db;

        private static string databasePath = Path.Combine(FileSystem.AppDataDirectory, "MAUI_Terms.db");

        static async Task Init()
        {
            //If the database already exists return
            if (_db != null)
            {
                return;
            }
            //Get absolute path to the database file
           //var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "terms.db");

            //Declare the database with the absolute path
           // var databasePath = Path.Combine(FileSystem.AppDataDirectory, "MAUI_Terms.db");

            Console.WriteLine(databasePath); //TESTING

            //Create the database
            _db = new SQLiteAsyncConnection(databasePath);

            //Create 3 tables //TODO We need to ensure that these tables are being created.

            await _db.CreateTableAsync<Term>();
            await _db.CreateTableAsync<Course>();
            await _db.CreateTableAsync<User>();


        }

            //Added below for login with User. Related to Capstone Project
            //await _db.CreateTableAsync<User>(); //TODO
        

        #region Terms methods
        //terms-------------------------------------------------------
        public static async Task AddTerm(string name, string semester, DateTime startDate, DateTime endDate)
        {
            await Init();
            var Term = new Term()
            {
                Name = name,
                Semester = semester,
                //InStock = inStock,
                //Price = price,
                StartDate = startDate,
                EndDate = endDate
            };
            
            await _db.InsertAsync(Term);

            var id = Term.Id; //Returns the term ID

        }

        public static async Task RemoveTerm(int id)
        {

            await Init();

            await _db.DeleteAsync<Term>(id);

        }

        public static async Task<IEnumerable<Term>> GetTerms()
        {
            await Init();

            var Terms = await _db.Table<Term>().ToListAsync();
            return Terms;
        }

        public static async Task UpdateTerm(int id, string name, string semester, DateTime startDate, DateTime endDate)
        {
            await Init();

            var termQuery = await _db.Table<Term>()
                .Where(i => i.Id == id)
                .FirstOrDefaultAsync();
            
            if (termQuery != null)
            {
                termQuery.Name = name;
                termQuery.Semester = semester;
                //termQuery.InStock = inStock;
                //termQuery.Price = price;
                termQuery.StartDate = startDate;
                termQuery.EndDate = endDate;

                await _db.UpdateAsync(termQuery);
            }

            
        }

        #endregion

        #region Count Determinations
        public static async Task<int> GetTermCountAsync()
        {
            await Init();
            
            //TODO getting a course from a table
            //stackoverflow "how-doestaskinit-become-an-int"

            //Some form of the SQL queries below might be useful in getting the count of courses
            //and assesments for limiting the number of courses per term or the number of assesments per course.

          //int courseCount = await _db.ExecuteScalarAsync<int>("Select Count(*) From course where TermId = '" + selectedTermId + "'");
          //int courseCount = await _db.ExecuteScalarAsync<int>($"Select Count(*) From course where TermId = '{ selectedTermId }'");
          //int courseCount = await _db.ExecuteScalarAsync<int>($"Select Count(*) From course where TermId = ?", { selectedTermId });
          int courseCount = await _db.ExecuteScalarAsync<int>("Select Count(*) from course");
          //var objectiveCount = await _conn.QueryAsync<Assessment>($"Select Type From Assessments Where course = '{_course.Id}' And Type = 'Objective'");
          //var performanceCount = await _conn.QueryAsync<Assessment>($"Select Type From Assessments Where course = '{_course.Id}' And Type = 'Performance'");

            return courseCount;

        }

        public static async Task<int> GetCourseCountAsync(int selectedTermId)
        {
            await Init();

            //TODO getting a course from a table
            //stackoverflow "how-doestaskinit-become-an-int"

            //Some form of the SQL queries below might be useful in getting the count of courses
            //and assesments for limiting the number of courses per term or the number of assesments per course.

            //int courseCount = await _db.ExecuteScalarAsync<int>("Select Count(*) From course where TermId = '" + selectedTermId + "'");
            //int courseCount = await _db.ExecuteScalarAsync<int>($"Select Count(*) From course where TermId = '{ selectedTermId }'");
            int courseCount = await _db.ExecuteScalarAsync<int>($"Select Count(*) From course where TermId = ?",  selectedTermId );
            //int courseCount = await _db.ExecuteScalarAsync<int>("Select Count(*) from course");
            //var objectiveCount = await _conn.QueryAsync<Assessment>($"Select Type From Assessments Where course = '{_course.Id}' And Type = 'Objective'");
            //var performanceCount = await _conn.QueryAsync<Assessment>($"Select Type From Assessments Where course = '{_course.Id}' And Type = 'Performance'");

            return courseCount;

        }
        #endregion

        #region Courses methods
        //courses-------------------------------------------------------
        public static async Task AddCourse(int termId, string name, string status, DateTime startDate, DateTime endDate, 
            string instructorName, string instructorPhone, string instructorEmail, bool notificationStart, string notes, 
            string performanceAssessmentName, DateTime performanceAssessmentStartDate, DateTime performanceAssessmentDueDate, bool performanceAssessmentNotification, 
            string objectiveAssessmentName, DateTime objectiveAssessmentStartDate, DateTime objectiveAssessmentDueDate, bool objectiveAssessmentNotification)
        {
            await Init();

            var course = new Course
            {
                TermId = termId,
                Name = name,
                Status = status,
                StartDate = startDate,
                EndDate = endDate,

                InstructorName = instructorName,
                InstructorPhone = instructorPhone,
                InstructorEmail = instructorEmail,

                StartNotification = notificationStart,
                Notes = notes,

                PerformanceAssessmentName = performanceAssessmentName,
                PerformanceAssessmentStartDate = performanceAssessmentStartDate,
                PerformanceAssessmentDueDate = performanceAssessmentDueDate,
                PerformanceAssessmentNotification = performanceAssessmentNotification,

                ObjectiveAssessmentName = objectiveAssessmentName,
                ObjectiveAssessmentStartDate = objectiveAssessmentStartDate,
                ObjectiveAssessmentDueDate = objectiveAssessmentDueDate,
                ObjectiveAssessmentNotification = objectiveAssessmentNotification

                //InStock = inStock,
                //Price = price,
            };

            await _db.InsertAsync(course);

            var id = course.Id; //Returns the course ID
        }

        public static async Task RemoveCourse(int id)
        {
            await Init();

            await _db.DeleteAsync<Course>(id);
        }

        public static async Task <IEnumerable<Course>> GetCourses(int TermID)
        {
            await Init();

            var course = await _db.Table<Course>().Where(i => i.TermId == TermID).ToListAsync();

            return course;
        }

        //Overload Method
        //Used with notifications
        //Using the OnAppearing method is a key.
        public static async Task<IEnumerable<Course>> GetCourses()
        {
            await Init();

            var course = await _db.Table<Course>().ToListAsync();

            return course;
        }

        public static async Task UpdateCourse(int id, string name, string status, DateTime startDate, DateTime endDate, 
            string instructorName, string instructorPhone, string instructorEmail, bool notificationStart, string notes, 
            string performanceAssessmentName, DateTime performanceAssessmentStartDate, DateTime performanceAssessmentDueDate, bool performanceAssessmentNotification,
            string objectiveAssessmentName, DateTime objectiveAssessmentStartDate, DateTime objectiveAssessmentDueDate, bool objectiveAssessmentNotification)
        {
            //No need to deal with TermId here because it wont change when we update a course
            await Init();

            var courseQuery = await _db.Table<Course>()
                .Where(i => i.Id == id)
                .FirstOrDefaultAsync();

            if (courseQuery != null)
            {
                courseQuery.Name = name;
                courseQuery.Status = status;
                courseQuery.StartNotification = notificationStart;
                courseQuery.Notes = notes;
                courseQuery.StartDate = startDate;
                courseQuery.EndDate = endDate;

                courseQuery.InstructorName = instructorName;
                courseQuery.InstructorPhone = instructorPhone;
                courseQuery.InstructorEmail = instructorEmail;

                courseQuery.PerformanceAssessmentName = performanceAssessmentName;
                courseQuery.PerformanceAssessmentStartDate = performanceAssessmentStartDate;
                courseQuery.PerformanceAssessmentDueDate = performanceAssessmentDueDate;
                courseQuery.PerformanceAssessmentNotification = performanceAssessmentNotification;

                courseQuery.ObjectiveAssessmentName = objectiveAssessmentName;
                courseQuery.ObjectiveAssessmentStartDate = objectiveAssessmentStartDate;
                courseQuery.ObjectiveAssessmentDueDate = objectiveAssessmentDueDate;
                courseQuery.ObjectiveAssessmentNotification = objectiveAssessmentNotification;

                //courseQuery.InStock = inStock;
                //courseQuery.Price = price;

                await _db.UpdateAsync(courseQuery);
            }
        }

        #endregion

        #region Demo Data Methods

        //DemoData -------------------------------------------------------

        //LoadSample() Data was originally void in the demo tutorial.
        //Changed to task since await cannot be used with void.
        public static async Task LoadSampleData()
        {
            try
            {
                // Initialize database
                await Init();

                // Verify database connection
                if (_db == null)
                {
                    throw new Exception("Database connection failed to initialize");
                }

                // Create and insert first term
                Term term = new Term
                {
                    Name = "Term 1",
                    Semester = "Spring",
                    StartDate = DateTime.Today.Date,
                    EndDate = DateTime.Today.Date + TimeSpan.FromDays(180)

                    //InStock = 255,
                    //Price = 25m,

                };

                var termResult = await _db.InsertAsync(term);
                if (termResult <= 0)
                {
                    throw new Exception($"Failed to insert Term 1. Result: {termResult}");
                }

                // Create and insert first set of courses
                Course course = new Course
                {
                    Name = "Course 1",
                    Status = "In Progress",
                    StartDate = DateTime.Now.Date,
                    EndDate = DateTime.Now.Date + TimeSpan.FromDays(180),
                    StartNotification = true,
                    Notes = "This is a note",
                    TermId = term.Id,

                    InstructorName = "Anika Patel",
                    InstructorPhone = "555-123-4567", 
                    InstructorEmail = "anika.patel@strimeuniversity.edu",

                    PerformanceAssessmentName = "Performance Assessment 1",
                    PerformanceAssessmentStartDate = DateTime.Now.Date,
                    PerformanceAssessmentDueDate = DateTime.Now.Date + TimeSpan.FromDays(5),
                    PerformanceAssessmentNotification = true,

                    ObjectiveAssessmentName = "Objective Assessment 1",
                    ObjectiveAssessmentStartDate = DateTime.Now.Date,
                    ObjectiveAssessmentDueDate = DateTime.Now.Date + TimeSpan.FromDays(5),
                    ObjectiveAssessmentNotification = true

                    //InStock = 25,
                    //Price = 22.59m,

                };

                var courseResult = await _db.InsertAsync(course);
                if (courseResult <= 0)
                {
                    throw new Exception($"Failed to insert course 1. Result: {courseResult}");
                }

                /*
                Course course2 = new Course
                {
                    Name = "Course 2",
                    Status = "Completed",
                    StartDate = DateTime.Now.Date,
                    EndDate = DateTime.Now.Date + TimeSpan.FromDays(180),
                    StartNotification = true,
                    TermId = term.Id,

                    InstructorName = "Batman",
                    InstructorPhone = 1234567890,
                    InstructorEmail = "H2E0M@example.com",

                    PerformanceAssessmentName = "Performance Assessment 2",
                    PerformanceAssessmentStartDate = DateTime.Now.Date,
                    PerformanceAssessmentDueDate = DateTime.Now.Date + TimeSpan.FromDays(7),

                    ObjectiveAssessmentName = "Objective Assessment 2",
                    ObjectiveAssessmentStartDate = DateTime.Now.Date,
                    ObjectiveAssessmentDueDate = DateTime.Now.Date + TimeSpan.FromDays(7),
                };

                courseResult = await _db.InsertAsync(course2);
                if (courseResult <= 0)
                {
                    throw new Exception($"Failed to insert course 2. Result: {courseResult}");
                }

                // Create and insert second term
                Term term2 = new Term
                {
                    Name = "Term 2",
                    Semester = "Fall",
                    //InStock = 255,
                    //Price = 25m,
                    StartDate = DateTime.Today.Date,
                    EndDate = DateTime.Today.Date + TimeSpan.FromDays(180)
                };

                termResult = await _db.InsertAsync(term2);
                if (termResult <= 0)
                {
                    throw new Exception($"Failed to insert Term 2. Result: {termResult}");
                }

                // Create and insert remaining courses
                var remainingCourses = new[]
                {
                new Course
                {
                    Name = "Course 3",
                    Status = "In Progress",
                    //InStock = 25,
                    //Price = 25.59m,
                    StartDate = DateTime.Now.Date,
                    EndDate = DateTime.Now.Date + TimeSpan.FromDays(180),
                    StartNotification = true,
                    TermId = term2.Id
                },
                new Course
                {
                    Name = "Course 4",
                    Status = "Completed",
                    //InStock = 55,
                    //Price = 2.59m,
                    StartDate = DateTime.Now.Date,
                    EndDate = DateTime.Now.Date + TimeSpan.FromDays(180),
                    StartNotification = true,
                    TermId = term2.Id
                },
                new Course
                {
                    Name = "Course 5",
                    Status = "Dropped",
                    //InStock = 55,
                    //Price = 2.59m,
                    StartDate = DateTime.Now.Date,
                    EndDate = DateTime.Now.Date + TimeSpan.FromDays(180),
                    StartNotification = false,
                    TermId = term2.Id
                }
            };

                foreach (var currentCourse in remainingCourses)
                {
                    courseResult = await _db.InsertAsync(currentCourse);
                    if (courseResult <= 0)
                    {
                        throw new Exception($"Failed to insert {currentCourse.Name}. Result: {courseResult}");
                    }
                }
                */

                // Verify data was inserted
                var terms = await GetTerms();
                var courses = await GetCourses();

                if (terms.Count() == 0 || courses.Count() == 0)
                {
                    throw new Exception($"Data verification failed. Terms: {terms.Count()}, course: {courses.Count()}");
                }
            }
            catch (Exception ex)
            {

                // You might want to show this to the user or log it
                System.Diagnostics.Debug.WriteLine($"Error in LoadSampleData: {ex.Message}");
                throw; // Re-throw the exception to be handled by the caller

            }
        }


        public static async Task ClearSampleData()
        {
            await Init();

            await _db.DropTableAsync<Course>();
            await _db.DropTableAsync<Term>();
            
            //await _db.DropTableAsync<User>(); //TODO

            _db = null;

            Settings.ClearSettings();
        }

        //Might not be useful to the project
        //On SQLLite Video
        public static async void LoadSampleDataSql()
        {
            await Init();
        }

        public static async Task TestMethod(Page page)
        {
            if (File.Exists(databasePath))
            {
                await page.DisplayAlert("Database exists", "Database exists", "OK");
            }
            else
            {
                await page.DisplayAlert("Error", "Database does not exist", "OK");
            }

            // Check the database tables

            //Check if Term Table Exists
            var tableInfo = await _db.GetTableInfoAsync("Term");
            if (tableInfo.Count() > 0)
            {
                await page.DisplayAlert("Table Info", "Term table exists", "OK");
            }
            else
            {
                await page.DisplayAlert("Table Info", "Term table does not exist", "OK");
            }

            //Check if course Table Exists
            tableInfo = await _db.GetTableInfoAsync("course");
            if (tableInfo.Count() > 0)
            {
                await page.DisplayAlert("Table Info", "course table exists", "OK");
            }
            else
            {
                await page.DisplayAlert("Table Info", "course table does not exist", "OK");
            }

            // Check for data in the Term Table
            var terms = await GetTerms();
            if (terms.Count() > 0)
            {
                await page.DisplayAlert("Table Data", "Term table has data", "OK");
            }
            else
            {
                await page.DisplayAlert("Table Data", "Term table is empty", "OK");
            }


            // Check for data in the course Table
            var courses = await GetCourses();
            if (courses.Count() > 0)
            {
                await page.DisplayAlert("Table Data", "course table has data", "OK");
            }
            else
            {
                await page.DisplayAlert("Table Data", "course table is empty", "OK");
            }
        }


        #endregion

        #region User Security Methods
        public static async Task<bool> IsFirstLogin()
        {
            await Init();

            var users = await _db.Table<User>().ToListAsync();
            return users.Count() == 0;
        }

        public static async Task CreateUser(string pin)
        {
            await Init();

            // Hash the PIN instead of storing it in plain text
            string hashedPin = BCrypt.Net.BCrypt.HashPassword(pin);

            var user = new User
            {
                Pin = hashedPin,
                IsFirstLogin = false
            };

            await _db.InsertAsync(user);
        }

        public static async Task<bool> ValidatePin(string pin)
        {
            await Init();

            var user = await _db.Table<User>().FirstOrDefaultAsync();

            if (user == null)
                return false;

            // Verify the PIN against the stored hash
            return BCrypt.Net.BCrypt.Verify(pin, user.Pin);
        }

        public static async Task UpdatePin(string newPin)
        {
            await Init();

            var user = await _db.Table<User>().FirstOrDefaultAsync();

            if (user != null)
            {
                // Hash the new PIN
                user.Pin = BCrypt.Net.BCrypt.HashPassword(newPin);
                await _db.UpdateAsync(user);
            }
        }
        #endregion
    }

}