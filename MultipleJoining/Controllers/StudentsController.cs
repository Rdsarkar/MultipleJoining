using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultipleJoining.Models;
using MultipleJoining.DTOs;

namespace MultipleJoining
{
    public class SelfInput 
    {
        public decimal? Sid { get; set; }
    }

    public class SelfOutput
    {
        public decimal? Sid { get; set; }
        public string Sname { get; set; }
        public string Crname { get; set; }
    }
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly ModelContext _context;

        public StudentsController(ModelContext context)
        {
            _context = context;
        }

        // GET: api/Students
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            return await _context.Students.ToListAsync();
        }

        [HttpPost("JoiningTable")]
        public async Task<ActionResult<ResponseDto>> JoinData([FromBody] SelfInput input)
        {

            if (input.Sid == 0) 
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseDto 
                {
                    Message = "Please Input The Sid Field",
                    Success = false,
                    Payload = null
                });
            }

            List<SelfOutput> selfOutputs =
                await (from s in _context.Students
                                            .Where(i => i.Sid == input.Sid)
                       from d in _context.Departments
                                            .Where(i => s.Did == i.Did)
                       from e in _context.Exams
                                            .Where(i => d.Exid == i.Exid)
                       from m in _context.Months
                                            .Where(i => e.Mid == i.Mid)
                       from c in _context.Courses 
                                            .Where(i => m.Crid == i.Crid)
                       select new SelfOutput
                       {
                           Sid = s.Sid,
                           Sname = s.Sname,
                           Crname = c.Crname
                       }).OrderBy(i => i.Sid).ToListAsync();
            if (selfOutputs == null ) 
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseDto 
                {
                    Message = "Data is not found",
                    Success = false,
                    Payload = null
                });
            }

            return StatusCode(StatusCodes.Status200OK, new ResponseDto
            {
                Message = "Joining Done",
                Success = true,
                Payload = selfOutputs
            });
        }

        // GET: api/Students/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(decimal? id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            return student;
        }

        // PUT: api/Students/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(decimal? id, Student student)
        {
            if (id != student.Sid)
            {
                return BadRequest();
            }

            _context.Entry(student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Students
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
            _context.Students.Add(student);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (StudentExists(student.Sid))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetStudent", new { id = student.Sid }, student);
        }

        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(decimal? id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StudentExists(decimal? id)
        {
            return _context.Students.Any(e => e.Sid == id);
        }
    }
}
