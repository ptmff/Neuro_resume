//using System;
//using System.Threading.Tasks;
//using back.API.Controllers;
//using back.Application.DTOs;      // DTO, используемые контроллером
//using back.Application.Interfaces;  // IAuthService
//using Microsoft.AspNetCore.Mvc;
//using Moq;
//using Xunit;

//namespace back.API.Tests
//{
//    // Минимальная реализация Result<T> для тестирования.
//    public class Result<T>
//    {
//        public bool IsSuccess { get; }
//        public T Value { get; }
//        public string Error { get; }

//        protected Result(bool isSuccess, T value, string error)
//        {
//            IsSuccess = isSuccess;
//            Value = value;
//            Error = error;
//        }

//        public static Result<T> Success(T value) => new Result<T>(true, value, null);
//        public static Result<T> Failure(string error) => new Result<T>(false, default, error);

//        public TResult Match<TResult>(Func<T, TResult> success, Func<string, TResult> failure)
//            => IsSuccess ? success(Value) : failure(Error);
//    }

//    // Новая тестовая модель для проверки возвращаемых данных.
//    public class TestUser
//    {
//        public int Id { get; set; }
//        public string Username { get; set; }
//        public string Email { get; set; }
//    }

//    public class AuthControllerTests
//    {
//        private readonly Mock<IAuthService> _authServiceMock;
//        private readonly AuthController _controller;

//        public AuthControllerTests()
//        {
//            _authServiceMock = new Mock<IAuthService>();
//            _controller = new AuthController(_authServiceMock.Object);
//        }

//        // Тест успешной регистрации пользователя.
//        [Theory]
//        [InlineData("john_doe", "john@example.com", "Password123!")]
//        public async Task Register_ReturnsOk_WhenRegistrationIsSuccessful(string username, string email, string password)
//        {
//            // Подготовка входных данных.
//            var request = new UserRegisterDto
//            {
//                Username = username,
//                Email = email,
//                Password = password
//            };

//            // Ожидаемый результат – тестовая модель.
//            var expectedUser = new TestUser { Id = 1, Username = username, Email = email };

//            // Настройка мока: возвращаем успешный результат.
//            _authServiceMock.Setup(x => x.RegisterUserAsync(It.IsAny<UserRegisterDto>()))
//                .ReturnsAsync(Result<TestUser>.Success(expectedUser));

//            // Вызов контроллера.
//            var result = await _controller.Register(request);

//            // Проверка результата.
//            var okResult = Assert.IsType<OkObjectResult>(result);
//            var returnedUser = Assert.IsType<TestUser>(okResult.Value);
//            Assert.Equal(expectedUser.Username, returnedUser.Username);
//            Assert.Equal(expectedUser.Email, returnedUser.Email);
//        }

//        // Тест неуспешной регистрации пользователя.
//        [Theory]
//        [InlineData("invalid_user", "invalid@example.com", "short")]
//        public async Task Register_ReturnsBadRequest_WhenRegistrationFails(string username, string email, string password)
//        {
//            var request = new UserRegisterDto
//            {
//                Username = username,
//                Email = email,
//                Password = password
//            };

//            var errorMessage = "Registration failed";

//            _authServiceMock.Setup(x => x.RegisterUserAsync(It.IsAny<UserRegisterDto>()))
//                .ReturnsAsync(Result<TestUser>.Failure(errorMessage));

//            var result = await _controller.Register(request);
//            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
//            Assert.Equal(errorMessage, badRequestResult.Value);
//        }

//        // Тест успешного логина.
//        [Theory]
//        [InlineData("john_doe", "Password123!")]
//        public async Task Login_ReturnsOk_WhenLoginIsSuccessful(string username, string password)
//        {
//            var request = new UserLoginDto
//            {
//                Username = username,
//                Password = password
//            };

//            var expectedUser = new TestUser { Id = 1, Username = username, Email = "john@example.com" };

//            _authServiceMock.Setup(x => x.LoginUserAsync(It.IsAny<UserLoginDto>()))
//                .ReturnsAsync(Result<TestUser>.Success(expectedUser));

//            var result = await _controller.Login(request);
//            var okResult = Assert.IsType<OkObjectResult>(result);
//            var returnedUser = Assert.IsType<TestUser>(okResult.Value);
//            Assert.Equal(expectedUser.Username, returnedUser.Username);
//            Assert.Equal(expectedUser.Email, returnedUser.Email);
//        }

//        // Тест неуспешного логина.
//        [Theory]
//        [InlineData("john_doe", "WrongPassword")]
//        public async Task Login_ReturnsBadRequest_WhenLoginFails(string username, string password)
//        {
//            var request = new UserLoginDto
//            {
//                Username = username,
//                Password = password
//            };

//            var errorMessage = "Invalid credentials";

//            _authServiceMock.Setup(x => x.LoginUserAsync(It.IsAny<UserLoginDto>()))
//                .ReturnsAsync(Result<TestUser>.Failure(errorMessage));

//            var result = await _controller.Login(request);
//            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
//            Assert.Equal(errorMessage, badRequestResult.Value);
//        }
//    }
//}
