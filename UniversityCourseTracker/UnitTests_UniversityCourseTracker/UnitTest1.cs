using System.Threading.Tasks;
using Xunit;

namespace UnitTesting_UniversityCourseTracker
{
    //PART D
    // Mock class that simulates the database service
    public static class MockDatabaseService
    {
        private static string _storedHashedPin = "";
        private static bool _isFirstLogin = true;
        private static bool _isInitialized = false;

        private static Task Init()
        {
            _isInitialized = true;
            return Task.CompletedTask;
        }



        public static async Task CreateUser(string pin)
        {
            await Init();
            // Simulate BCrypt hashing
            _storedHashedPin = "$2a$" + pin + "$"; // Not actual BCrypt, just a simulation
            _isFirstLogin = false;
        }

        public static async Task<bool> ValidatePin(string pin)
        {
            await Init();
            if (string.IsNullOrEmpty(_storedHashedPin))
                return false;

            // Simulate BCrypt verification
            return _storedHashedPin == "$2a$" + pin + "$";
        }

    }

    public class UserAuthenticationTests
    {
        [Fact]
        public async Task ValidatePin_EmptyPassword_ReturnsFalse()
        {
            // Arrange: Define an empty PIN.
            string emptyPin = "";
            // Act: Attempt to validate the empty PIN.
            bool result = await MockDatabaseService.ValidatePin(emptyPin);
            // Assert: The result should be false, as an empty PIN is invalid.
            Assert.False(result, "Validating an empty PIN should return false.");
        }

        [Fact]
        public async Task ValidatePin_CorrectPassword_ReturnsTrue()
        {
            // Arrange: Define a test PIN.
            string testPin = "1234";
            // Ensure that a user is created with the test PIN.
            await MockDatabaseService.CreateUser(testPin);
            // Act: Validate the correct PIN.
            bool result = await MockDatabaseService.ValidatePin(testPin);
            // Assert: The result should be true, as the correct PIN is provided.
            Assert.True(result, "Validating the correct PIN should return true.");
        }


    }
}