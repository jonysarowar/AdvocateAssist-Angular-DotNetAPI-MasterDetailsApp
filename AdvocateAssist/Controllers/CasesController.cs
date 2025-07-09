using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AdvocateAssist.Data;
using AdvocateAssist.Entities;
using AdvocateAssist.Models.DTOs;

namespace AdvocateAssist.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CasesController : ControllerBase
    {
        private readonly AdvocateAssistContext _context;

        public CasesController(AdvocateAssistContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CaseReadDto>>> GetCases()
        {
            var cases = await _context.Cases.ToListAsync();
            var caseDtos = cases.Select(c => new CaseReadDto
            {
                CaseId = c.CaseId,
                CaseNumber = c.CaseNumber
            }).ToList();
            return Ok(caseDtos);
        }



        //[HttpGet("{id}")]
        //public async Task<ActionResult<Case>> GetCase(int id)
        //{
        //    var @case = await _context.Cases.FindAsync(id);

        //    if (@case == null)
        //    {
        //        return NotFound();
        //    }

        //    return @case;
        //}

        
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutCase(int id, Case @case)
        //{
        //    if (id != @case.CaseId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(@case).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!CaseExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        
        //[HttpPost]
        //public async Task<ActionResult<Case>> PostCase(Case @case)
        //{
        //    _context.Cases.Add(@case);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetCase", new { id = @case.CaseId }, @case);
        //}

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteCase(int id)
        //{
        //    var @case = await _context.Cases.FindAsync(id);
        //    if (@case == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Cases.Remove(@case);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool CaseExists(int id)
        //{
        //    return _context.Cases.Any(e => e.CaseId == id);
        //}
    }
}
