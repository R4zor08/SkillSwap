using SkillSwap.Services.Interfaces;
using SkillSwap.Web.Requests.Interfaces;
using SkillSwap.Web.Requests.Requests;
using SkillSwap.Web.Requests.Responses;

namespace SkillSwap.Web.Requests.Handlers
{
    /// <summary>
    /// Handles all student-related requests
    /// </summary>
    public class StudentRequestHandler : 
        IRequestHandler<CreateStudentRequest, Response<Guid>>,
        IRequestHandler<UpdateStudentRequest, BaseResponse>,
        IRequestHandler<DeleteStudentRequest, BaseResponse>,
        IRequestHandler<LoginRequest, Response<Guid>>,
        IRequestHandler<RegisterRequest, Response<Guid>>
    {
        private readonly IStudentService _studentService;
        private readonly IRequestService _requestService;
        
        public StudentRequestHandler(IStudentService studentService, IRequestService requestService)
        {
            _studentService = studentService;
            _requestService = requestService;
        }
        
        public async Task<Response<Guid>> HandleAsync(CreateStudentRequest request)
        {
            if (!request.IsValid)
                return Response<Guid>.Fail(string.Join(", ", request.Errors.Values));
            
            try
            {
                var student = _studentService.CreateStudent(request.Name, request.Email, request.Password);
                return Response<Guid>.Ok(student.StudentId, "Student created successfully");
            }
            catch (Exception ex)
            {
                return Response<Guid>.Fail($"Failed to create student: {ex.Message}");
            }
        }
        
        public async Task<BaseResponse> HandleAsync(UpdateStudentRequest request)
        {
            if (!request.IsValid)
                return BaseResponse.Fail(string.Join(", ", request.Errors.Values));
            
            try
            {
                _studentService.UpdateStudent(request.StudentId, request.Name, request.Email);
                return BaseResponse.Ok("Student updated successfully");
            }
            catch (Exception ex)
            {
                return BaseResponse.Fail($"Failed to update student: {ex.Message}");
            }
        }
        
        public async Task<BaseResponse> HandleAsync(DeleteStudentRequest request)
        {
            if (!request.IsValid)
                return BaseResponse.Fail(string.Join(", ", request.Errors.Values));
            
            try
            {
                _studentService.DeleteStudent(request.StudentId);
                return BaseResponse.Ok("Student deleted successfully");
            }
            catch (Exception ex)
            {
                return BaseResponse.Fail($"Failed to delete student: {ex.Message}");
            }
        }
        
        public async Task<Response<Guid>> HandleAsync(LoginRequest request)
        {
            if (!request.IsValid)
                return Response<Guid>.Fail(string.Join(", ", request.Errors.Values));
            
            var student = _studentService.GetStudentByEmail(request.Email);
            if (student == null || !BCrypt.Net.BCrypt.Verify(request.Password, student.Password))
                return Response<Guid>.Fail("Invalid email or password");
            
            _requestService.LoginUser(student.StudentId, student.Name, student.Email);
            return Response<Guid>.Ok(student.StudentId, "Login successful");
        }
        
        public async Task<Response<Guid>> HandleAsync(RegisterRequest request)
        {
            if (!request.IsValid)
                return Response<Guid>.Fail(string.Join(", ", request.Errors.Values));
            
            var existing = _studentService.GetStudentByEmail(request.Email);
            if (existing != null)
                return Response<Guid>.Fail("Email already registered");
            
            var student = _studentService.CreateStudent(request.Name, request.Email, request.Password);
            return Response<Guid>.Ok(student.StudentId, "Registration successful");
        }
    }
}
