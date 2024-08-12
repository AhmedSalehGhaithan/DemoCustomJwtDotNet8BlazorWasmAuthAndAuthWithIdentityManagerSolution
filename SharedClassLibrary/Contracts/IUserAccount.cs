using SharedClassLibrary.DTOs;
using static SharedClassLibrary.DTOs.ServiceResponses;
namespace SharedClassLibrary.Contracts
{
    public interface IUserAccount
    {
        Task<GeneralResponse> CreateAccount(UserDTO userDTO);
        Task<LoginResponse> LoginAccount(LoginDTO loginDTO);
    }
}
