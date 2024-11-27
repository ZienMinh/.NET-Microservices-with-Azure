using AutoMapper;
using eCommerce.Core.DTO;
using eCommerce.Core.Entities;
using eCommerce.Core.RepositoryContracts;
using eCommerce.Core.ServiceContracts;

namespace eCommerce.Core.Services;

internal class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<AuthenticationResponse> Login(LoginRequest loginRequest)
    {
        ApplicationUser? user = await _userRepository.GetUserByEmailAndPassword(loginRequest.Email, loginRequest.Password);
        if (user == null)
            return null;

        //return new AuthenticationResponse(
        //    user.UserID,
        //    user.Email,
        //    user.PersonName,
        //    user.Gender,
        //    "token", Success: true);

        return _mapper.Map<AuthenticationResponse>(user) with
        { Success = true, Token = "token" };
    }

    public async Task<AuthenticationResponse> Register(RegisterRequest registerRequest)
    {
        // Create a new ApplicationUser object from RegisterRequest
        //ApplicationUser user = new ApplicationUser()
        //{
        //    PersonName = registerRequest.PersonName,
        //    Email = registerRequest.Email,
        //    Password = registerRequest.Password,
        //    Gender = registerRequest.Gender.ToString()
        //};

        // Map registerRequest
        ApplicationUser user = _mapper.Map<ApplicationUser>(registerRequest);
        ApplicationUser? registeredUser = await _userRepository.AddUser(user);
        if (registeredUser == null)
            return null;

        //return new AuthenticationResponse(
        //    registeredUser.UserID,
        //    registeredUser.Email,
        //    registeredUser.PersonName,
        //    registeredUser.Gender,
        //    "token", Success: true);

        return _mapper.Map<AuthenticationResponse>(user) with
        { Success = true, Token = "token" };
    }
}
