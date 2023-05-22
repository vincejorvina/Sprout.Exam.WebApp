using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.Common.Enums;
using Sprout.Exam.WebApp.Data;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Sprout.Exam.WebApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EmployeesController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Refactor this method to go through proper layers and fetch from the DB.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await Task.FromResult(_context.Employee.Where(m => !m.IsDeleted));
            return Ok(result);
        }

        /// <summary>
        /// Refactor this method to go through proper layers and fetch from the DB.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await Task.FromResult(_context.Employee.FirstOrDefault(m => m.Id == id));
            return Ok(result);
        }

        /// <summary>
        /// Refactor this method to go through proper layers and update changes to the DB.
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(EditEmployeeDto input)
        {
            var item = await Task.FromResult(_context.Employee.FirstOrDefault(m => m.Id == input.Id));
            if (item == null) return NotFound();
            item.FullName = input.FullName;
            item.Tin = input.Tin;
            item.Birthdate = input.Birthdate;
            item.TypeId = input.TypeId;
            _context.Update(item);
            await _context.SaveChangesAsync();
            return Ok(item);
        }

        /// <summary>
        /// Refactor this method to go through proper layers and insert employees to the DB.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post(CreateEmployeeDto input)
        {
            var id = await Task.FromResult(_context.Employee.Max(m => m.Id) + 1);
            _context.Add<EmployeeDto>(new EmployeeDto
            {
                Birthdate = input.Birthdate,
                FullName = input.FullName,
                Tin = input.Tin,
                TypeId = input.TypeId
            });
            await _context.SaveChangesAsync();

            return Created($"/api/employees/{id}", id);
        }

        /// <summary>
        /// Refactor this method to go through proper layers and perform soft deletion of an employee to the DB.
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await Task.FromResult(_context.Employee.FirstOrDefault(m => m.Id == id));
            if (item == null) return NotFound();
            item.IsDeleted = true;
            _context.Update(item);
            await _context.SaveChangesAsync();
            return Ok(item);
        }
        /* HARD DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await Task.FromResult(_context.Employee.FirstOrDefault(m => m.Id == id));
            if (result == null) return NotFound();
            _context.Remove<EmployeeDto>(result);
            await _context.SaveChangesAsync();
            return Ok(id);
        }
        */

        /// <summary>
        /// Refactor this method to go through proper layers and use Factory pattern
        /// </summary>
        /// <param name="id"></param>
        /// <param name="absentDays"></param>
        /// <param name="workedDays"></param>
        /// <returns></returns>
        [HttpPost("{id}/calculate")]
        public async Task<IActionResult> Calculate(int id, [FromBody] Salary r)
        {
            var result = await Task.FromResult(_context.Employee.FirstOrDefault(m => m.Id == id));
            if (result == null) return NotFound();
            var type = (EmployeeType) result.TypeId;
            return type switch
            {
                EmployeeType.Regular => 
                    Ok(new Salary.Regular().Compute(r.AbsentDays)),
                EmployeeType.Contractual =>
                    Ok(new Salary.Contractual().Compute(r.WorkedDays)),
                _ => NotFound("Employee Type not found")
            };
        }
    }
}
