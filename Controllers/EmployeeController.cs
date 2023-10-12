using API.EmployeeManagement.Models;
using API.EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace API.EmployeeManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeContext _employeeContext;

        public EmployeeController(EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
        }

        [HttpGet("GetAllEmployee")]
        public IActionResult GetAllEmployee()
        {
            var employees = _employeeContext.Employees.ToList();

            if (employees == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseVM
                {
                    message = "Tidak Ada Data Employee!"
                });
            }

            return StatusCode(StatusCodes.Status200OK, employees);
        }

        [HttpPost("GetEmployee/{id}")]
        public IActionResult GetEmployee(int id)
        {
            var employees = _employeeContext.Employees.FirstOrDefault(e => e.Id == id);

            if (employees == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseVM
                {
                    message = "Employee Tidak Ditemukan!"
                });
            }

            return StatusCode(StatusCodes.Status200OK, employees);
        }

        [HttpPut("UpdateEmployee/{id}")]
        public IActionResult UpdateEmployee(int id, [FromBody] EmployeeVM employeeVM)
        {
            var employees = _employeeContext.Employees.FirstOrDefault(e => e.Id == id);

            if (employees == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseVM
                {
                    message = "Employee Tidak Ditemukan!"
                });
            }

            employees.Nama = employeeVM.Nama;
            employees.Npp = employeeVM.Npp;
            employees.Divisi = employeeVM.Divisi;
            employees.Kelompok = employeeVM.Kelompok;

           try
            {
                _employeeContext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new ResponseVM
                {
                    message = "Data Employee Berhasil Diperbarui!"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseVM
                {
                    message = "Data Employee Gagal Diperbarui! " + ex.Message
                });
            }
        }

        [HttpPost("AddEmployee")]
        public IActionResult AddEmployee([FromBody] EmployeeVM employeeVM)
        {
            var newEmployee = new EmployeeVM
            {
                Nama = employeeVM.Nama,
                Npp = employeeVM.Npp,
                Kelompok = employeeVM?.Kelompok,
                Divisi = employeeVM?.Divisi,
            };

            var employees = _employeeContext.Employees.FirstOrDefault(e => e.Npp == employeeVM.Npp);

            if (employees != null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseVM
                {
                    message = "Data Dengan NPP Tersebut Sudah Ada!"
                });
            }

            try
            {
                _employeeContext.Employees.Add(newEmployee);
                _employeeContext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new ResponseVM
                {
                    message = "Data Employee Berhasil Ditambahkan!"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseVM
                {
                    message = "Data Employee Gagal Ditambahkan!" + ex.Message
                });
            }
        }

        [HttpDelete("DeleteEmployee/{id}")]
        public IActionResult DeleteEmployee(int id)
        {
            var employees = _employeeContext.Employees.FirstOrDefault(e => e.Id == id);

            if (employees == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseVM
                {
                    message = "Employee Tidak Ditemukan!"
                });
            }

            try
            {
                _employeeContext.Employees.Remove(employees);
                _employeeContext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new ResponseVM
                {
                    message = "Data Employee Berhasil Dihapus!"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseVM
                {
                    message = "Data Employee Gagal Ditambahkan!" + ex.Message
                });
            }
        }
    }
}
