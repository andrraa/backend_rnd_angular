using API.EmployeeManagement.Models;
using API.EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace API.EmployeeManagement.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly EmployeeContext _employeeContext;

        public UserController(EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
        }

        [HttpPost("UserLogin")]
        public IActionResult UserLogin([FromBody] UserLoginVM userLoginVM)
        {
            var user = _employeeContext.Useraccounts.FirstOrDefault(u => u.Username == userLoginVM.UserName && u.Password == userLoginVM.Password);

            if (user == null)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new ResponseVM
                {
                    message = "Username atau Password Salah!"
                });
            }

            var userNpp = user.FkEmployee;

            var employee = _employeeContext.Employees.FirstOrDefault(e => e.Npp == userNpp);

            if (employee != null)
            {
                return StatusCode(StatusCodes.Status200OK, new
                {
                    Nama = employee.Nama,
                    Username = user.Username,
                    Npp = employee.Npp,
                    Divisi = employee.Divisi,
                    Kelompok = employee.Kelompok
                });
            }

            return StatusCode(StatusCodes.Status404NotFound, new ResponseVM
            {
                message = "Data Employee Tidak Ditemukan"
            });
        }

        [HttpPost("UserRegister")]
        public IActionResult UserRegister([FromBody] UserRegisterVM userRegisterVM)
        {

            var newUser = new UserAccountVM
            {
                FkEmployee = userRegisterVM.Npp,
                Username = userRegisterVM.Username,
                Password = userRegisterVM.Password
            };

            var newEmployee = new EmployeeVM
            {
                Nama = userRegisterVM.Nama,
                Npp = userRegisterVM.Npp,
                Divisi = userRegisterVM.Divisi,
                Kelompok = userRegisterVM.Kelompok
            };

            var check = _employeeContext.Useraccounts.FirstOrDefault(u => u.FkEmployee == userRegisterVM.Npp);

            if (check != null)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new ResponseVM
                {
                    message = "User Dengan NPP " + userRegisterVM.Npp + " Sudah Ada!"
                });
            }

            try
            {
                _employeeContext.Employees.Add(newEmployee);
                _employeeContext.Useraccounts.Add(newUser);
                _employeeContext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new ResponseVM
                {
                    message = "Berhasil Melakukan Registrasi!"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseVM
                {
                    message = "Gagal Melakukan Proses Registrasi, Karena: " + ex.Message
                });
            }
        }
    }
}
