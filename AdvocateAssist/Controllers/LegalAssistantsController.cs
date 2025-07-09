using AdvocateAssist.Data;
using AdvocateAssist.Entities;
using AdvocateAssist.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AdvocateAssist.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LegalAssistantsController : ControllerBase
    {
        private readonly AdvocateAssistContext _context;
        private readonly IWebHostEnvironment _web;

        public LegalAssistantsController(AdvocateAssistContext context, IWebHostEnvironment web)
        {
            _context = context;
            _web = web;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LegalAssistantReadDto>>> GetLegalAssistants()
        {
            var legalAssistants = await _context.LegalAssistants
                .Include(la => la.LegalAssistantCases)
                    .ThenInclude(lac => lac.Case)
                .ToListAsync();

            var legalAssistantReadDtos = legalAssistants.Select(la => new LegalAssistantReadDto
            {
                LegalAssistantId = la.LegalAssistantId,
                LegalAssistantFname = la.LegalAssistantFname,
                LegalAssistantLname = la.LegalAssistantLname,
                JoinDate = la.JoinDate,
                Email = la.Email,
                MobileNo = la.MobileNo,
                IsActive = la.IsActive,
                MonthlyStipend = la.MonthlyStipend,
                NidNumber = la.NidNumber,
                Picture = la.Picture,
                BarLicenseNumber = la.BarLicenseNumber,

                LegalAssistantCases = la.LegalAssistantCases.Select(lac => new LegalAssistantCaseReadDto
                {
                    CaseId = lac.CaseId,
                    CaseNumber = lac.Case?.CaseNumber,
                    CaseTitle = lac.CaseTitle,
                    Firnumber = lac.Firnumber,
                    FilingDate = lac.FilingDate
                }).ToList()

            }).ToList();

            return Ok(legalAssistantReadDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LegalAssistantReadDto>> GetLegalAssistant(int id)
        {
            var legalAssistant = await _context.LegalAssistants
                .Include(la => la.LegalAssistantCases)
                    .ThenInclude(lac => lac.Case)
                .FirstOrDefaultAsync(la => la.LegalAssistantId == id);

            if (legalAssistant == null)
            {
                return NotFound();
            }

            var legalAssistantReadDto = new LegalAssistantReadDto
            {
                LegalAssistantId = legalAssistant.LegalAssistantId,
                LegalAssistantFname = legalAssistant.LegalAssistantFname,
                LegalAssistantLname = legalAssistant.LegalAssistantLname,
                JoinDate = legalAssistant.JoinDate,
                Email = legalAssistant.Email,
                MobileNo = legalAssistant.MobileNo,
                IsActive = legalAssistant.IsActive,
                MonthlyStipend = legalAssistant.MonthlyStipend,
                NidNumber = legalAssistant.NidNumber,
                Picture = legalAssistant.Picture,
                BarLicenseNumber = legalAssistant.BarLicenseNumber,

                LegalAssistantCases = legalAssistant.LegalAssistantCases.Select(lac => new LegalAssistantCaseReadDto
                {
                    CaseId = lac.CaseId,
                    CaseNumber = lac.Case?.CaseNumber,
                    CaseTitle = lac.CaseTitle,
                    Firnumber = lac.Firnumber,
                    FilingDate = lac.FilingDate
                }).ToList()
            };
            return Ok(legalAssistantReadDto);
        }

        [HttpPost]
        public async Task<ActionResult<LegalAssistantReadDto>> CreateLegalAssistant([FromForm] LegalAssistantCreateUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string uniqueFileName = "noimage.png";
            if (dto.PictureFile != null)
            {
                uniqueFileName = await SavePictureFile(dto.PictureFile);
            }

            var legalAssistant = new LegalAssistant
            {
                LegalAssistantFname = dto.LegalAssistantFname,
                LegalAssistantLname = dto.LegalAssistantLname,
                JoinDate = dto.JoinDate,
                Email = dto.Email,
                MobileNo = dto.MobileNo,
                IsActive = dto.IsActive,
                MonthlyStipend = dto.MonthlyStipend,
                NidNumber = dto.NidNumber,
                Picture = uniqueFileName,
                BarLicenseNumber = dto.BarLicenseNumber,
                LegalAssistantCases = new List<LegalAssistantCase>()
            };

            if (!string.IsNullOrWhiteSpace(dto.LegalAssistantCasesJson))
            {
                try
                {
                    
                    if (dto.LegalAssistantCasesJson.Trim().StartsWith("{"))
                    {
                        dto.LegalAssistantCasesJson = "[" + dto.LegalAssistantCasesJson + "]";
                    }

                    var caseDtos = JsonConvert.DeserializeObject<List<LegalAssistantCaseDto>>(dto.LegalAssistantCasesJson);

                    foreach (var c in caseDtos)
                    {
                        legalAssistant.LegalAssistantCases.Add(new LegalAssistantCase
                        {
                            CaseId = c.CaseId,
                            CaseTitle = c.CaseTitle,
                            Firnumber = c.Firnumber,
                            FilingDate = c.FilingDate
                        });
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest($"Invalid JSON in LegalAssistantCasesJson: {ex.Message}");
                }
            }



            // ✅ JSON Deserialization with Swagger-safe fix
            //if (!string.IsNullOrWhiteSpace(dto.LegalAssistantCasesJson))
            //{
            //    //try
            //    //{
            //    //    // Swagger থেকে এলে JSON double escaped হয়ে আসে, তাই Unescape দরকার
            //    //    var json = System.Text.RegularExpressions.Regex.Unescape(dto.LegalAssistantCasesJson);

            //    //    var caseDtos = JsonConvert.DeserializeObject<List<LegalAssistantCaseDto>>(json);
            //    //    foreach (var c in caseDtos)
            //    //    {
            //    //        legalAssistant.LegalAssistantCases.Add(new LegalAssistantCase
            //    //        {
            //    //            CaseId = c.CaseId,
            //    //            CaseTitle = c.CaseTitle,
            //    //            Firnumber = c.Firnumber,
            //    //            FilingDate = c.FilingDate
            //    //        });
            //    //    }
            //    //}
            //    //catch (Exception ex)
            //    //{
            //    //    return BadRequest($"Invalid JSON format in LegalAssistantCasesJson: {ex.Message}");
            //    //}

            //    try
            //    {
            //        // Swagger থেকে পাঠালে Unescape দরকার নেই, সরাসরি Deserialize করো
            //        var caseDtos = JsonConvert.DeserializeObject<List<LegalAssistantCaseDto>>(dto.LegalAssistantCasesJson);

            //        foreach (var c in caseDtos)
            //        {
            //            legalAssistant.LegalAssistantCases.Add(new LegalAssistantCase
            //            {
            //                CaseId = c.CaseId,
            //                CaseTitle = c.CaseTitle,
            //                Firnumber = c.Firnumber,
            //                FilingDate = c.FilingDate
            //            });
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        return BadRequest($"Invalid JSON format in LegalAssistantCasesJson: {ex.Message}");
            //    }
            //}

            _context.LegalAssistants.Add(legalAssistant);
            await _context.SaveChangesAsync();

            var savedLegalAssistant = await _context.LegalAssistants
                .Include(la => la.LegalAssistantCases)
                    .ThenInclude(lac => lac.Case)
                .FirstOrDefaultAsync(la => la.LegalAssistantId == legalAssistant.LegalAssistantId);

            var legalAssistantReadDto = new LegalAssistantReadDto
            {
                LegalAssistantId = savedLegalAssistant.LegalAssistantId,
                LegalAssistantFname = savedLegalAssistant.LegalAssistantFname,
                LegalAssistantLname = savedLegalAssistant.LegalAssistantLname,
                JoinDate = savedLegalAssistant.JoinDate,
                Email = savedLegalAssistant.Email,
                MobileNo = savedLegalAssistant.MobileNo,
                IsActive = savedLegalAssistant.IsActive,
                MonthlyStipend = savedLegalAssistant.MonthlyStipend,
                NidNumber = savedLegalAssistant.NidNumber,
                Picture = savedLegalAssistant.Picture,
                BarLicenseNumber = savedLegalAssistant.BarLicenseNumber,
                LegalAssistantCases = savedLegalAssistant.LegalAssistantCases.Select(lac => new LegalAssistantCaseReadDto
                {
                    CaseId = lac.CaseId,
                    CaseNumber = lac.Case?.CaseNumber,
                    CaseTitle = lac.CaseTitle,
                    Firnumber = lac.Firnumber,
                    FilingDate = lac.FilingDate
                }).ToList()
            };

            return CreatedAtAction(nameof(GetLegalAssistant), new { id = legalAssistantReadDto.LegalAssistantId }, legalAssistantReadDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLegalAssistant(int id, [FromForm] LegalAssistantCreateUpdateDto dto)
        {
            if (id != dto.LegalAssistantId && dto.LegalAssistantId != 0)
            {
                return BadRequest("LegalAssistant ID mismatch or missing.");
            }

            var existingLA = await _context.LegalAssistants
                .Include(la => la.LegalAssistantCases)
                .FirstOrDefaultAsync(la => la.LegalAssistantId == id);

            if (existingLA == null)
            {
                return NotFound();
            }

            // Update Picture if new file uploaded
            if (dto.PictureFile != null)
            {
                if (existingLA.Picture != "noimage.png")
                {
                    DeletePictureFile(existingLA.Picture);
                }
                existingLA.Picture = await SavePictureFile(dto.PictureFile);
            }
            else if (!string.IsNullOrEmpty(dto.Picture))
            {
                existingLA.Picture = dto.Picture;
            }

            // Update basic fields
            existingLA.LegalAssistantFname = dto.LegalAssistantFname;
            existingLA.LegalAssistantLname = dto.LegalAssistantLname;
            existingLA.JoinDate = dto.JoinDate;
            existingLA.Email = dto.Email;
            existingLA.MobileNo = dto.MobileNo;
            existingLA.IsActive = dto.IsActive;
            existingLA.MonthlyStipend = dto.MonthlyStipend;
            existingLA.NidNumber = dto.NidNumber;
            existingLA.BarLicenseNumber = dto.BarLicenseNumber;

            
            var oldCases = _context.LegalAssistantCases
                .Where(lc => lc.LegalAssistantId == existingLA.LegalAssistantId);
            _context.LegalAssistantCases.RemoveRange(oldCases);

            
            if (!string.IsNullOrWhiteSpace(dto.LegalAssistantCasesJson))
            {
                try
                {
                    if (dto.LegalAssistantCasesJson.Trim().StartsWith("{"))
                    {
                        dto.LegalAssistantCasesJson = "[" + dto.LegalAssistantCasesJson + "]";
                    }

                    var caseDtos = JsonConvert.DeserializeObject<List<LegalAssistantCaseDto>>(dto.LegalAssistantCasesJson);

                    foreach (var c in caseDtos)
                    {
                        existingLA.LegalAssistantCases.Add(new LegalAssistantCase
                        {
                            LegalAssistantId = existingLA.LegalAssistantId,
                            CaseId = c.CaseId,
                            CaseTitle = c.CaseTitle,
                            Firnumber = c.Firnumber,
                            FilingDate = c.FilingDate
                        });
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest($"Invalid JSON in LegalAssistantCasesJson: {ex.Message}");
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.LegalAssistants.Any(e => e.LegalAssistantId == id))
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


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLegalAssistant(int id)
        {
            var legalAssistant = await _context.LegalAssistants
                .Include(la => la.LegalAssistantCases)
                .FirstOrDefaultAsync(la => la.LegalAssistantId == id);

            if (legalAssistant == null)
            {
                return NotFound();
            }

            
            if (legalAssistant.Picture != "noimage.png")
            {
                DeletePictureFile(legalAssistant.Picture);
            }

            
            if (legalAssistant.LegalAssistantCases != null && legalAssistant.LegalAssistantCases.Any())
            {
                _context.LegalAssistantCases.RemoveRange(legalAssistant.LegalAssistantCases);
            }

            _context.LegalAssistants.Remove(legalAssistant);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        private async Task<string> SavePictureFile(IFormFile file)
        {
            var uploadsFolder = Path.Combine(_web.WebRootPath, "images");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            return uniqueFileName;
        }

        private void DeletePictureFile(string fileName)
        {
            var filePath = Path.Combine(_web.WebRootPath, "images", fileName);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }

        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateLegalAssistant(int id, [FromForm] LegalAssistantCreateUpdateDto dto)
        //{
        //    if (id != dto.LegalAssistantId && dto.LegalAssistantId != 0)
        //    {
        //        return BadRequest("LegalAssistant ID mismatch or invalid ID in form data.");
        //    }

        //    var existingLA = await _context.LegalAssistants
        //        .Include(la => la.LegalAssistantCases)
        //        .FirstOrDefaultAsync(la => la.LegalAssistantId == id);

        //    if (existingLA == null)
        //    {
        //        return NotFound();
        //    }

        //    // Update picture if new file is provided
        //    if (dto.PictureFile != null)
        //    {
        //        if (existingLA.Picture != "noimage.png")
        //        {
        //            DeletePictureFile(existingLA.Picture);
        //        }

        //        existingLA.Picture = await SavePictureFile(dto.PictureFile);
        //    }
        //    else if (!string.IsNullOrEmpty(dto.Picture))
        //    {
        //        existingLA.Picture = dto.Picture;
        //    }

        //    existingLA.LegalAssistantFname = dto.LegalAssistantFname;
        //    existingLA.LegalAssistantLname = dto.LegalAssistantLname;
        //    existingLA.JoinDate = dto.JoinDate;
        //    existingLA.Email = dto.Email;
        //    existingLA.MobileNo = dto.MobileNo;
        //    existingLA.IsActive = dto.IsActive;
        //    existingLA.MonthlyStipend = dto.MonthlyStipend;
        //    existingLA.NidNumber = dto.NidNumber;
        //    existingLA.BarLicenseNumber = dto.BarLicenseNumber;

        //    existingLA.LegalAssistantCases.Clear();

        //    if (!string.IsNullOrWhiteSpace(dto.LegalAssistantCasesJson))
        //    {
        //        try
        //        {
        //            if (dto.LegalAssistantCasesJson.Trim().StartsWith("{"))
        //            {
        //                dto.LegalAssistantCasesJson = "[" + dto.LegalAssistantCasesJson + "]";
        //            }

        //            var caseDtos = JsonConvert.DeserializeObject<List<LegalAssistantCaseDto>>(dto.LegalAssistantCasesJson);

        //            foreach (var c in caseDtos)
        //            {
        //                existingLA.LegalAssistantCases.Add(new LegalAssistantCase
        //                {
        //                    CaseId = c.CaseId,
        //                    CaseTitle = c.CaseTitle,
        //                    Firnumber = c.Firnumber,
        //                    FilingDate = c.FilingDate
        //                });
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            return BadRequest($"Invalid JSON in LegalAssistantCasesJson: {ex.Message}");
        //        }
        //    }

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!_context.LegalAssistants.Any(e => e.LegalAssistantId == id))
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
        //public async Task<ActionResult<LegalAssistantReadDto>> CreateLegalAssistant([FromForm] LegalAssistantCreateUpdateDto dto)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    string uniqueFileName = "noimage.png";
        //    if (dto.PictureFile != null)
        //    {
        //        uniqueFileName = await SavePictureFile(dto.PictureFile); 
        //    }

        //    var legalAssistant = new LegalAssistant
        //    {
        //        LegalAssistantFname = dto.LegalAssistantFname,
        //        LegalAssistantLname = dto.LegalAssistantLname,
        //        JoinDate = dto.JoinDate,
        //        Email = dto.Email,
        //        MobileNo = dto.MobileNo,
        //        IsActive = dto.IsActive,
        //        MonthlyStipend = dto.MonthlyStipend,
        //        NidNumber = dto.NidNumber,
        //        Picture = uniqueFileName,
        //        BarLicenseNumber = dto.BarLicenseNumber,
        //        LegalAssistantCases = new List<LegalAssistantCase>()
        //    };

        //    if (!string.IsNullOrWhiteSpace(dto.LegalAssistantCasesJson))
        //    {
        //        var caseDtos = JsonConvert.DeserializeObject<List<LegalAssistantCaseDto>>(dto.LegalAssistantCasesJson);
        //        foreach (var c in caseDtos)
        //        {
        //            legalAssistant.LegalAssistantCases.Add(new LegalAssistantCase
        //            {
        //                CaseId = c.CaseId,
        //                CaseTitle = c.CaseTitle,
        //                Firnumber = c.Firnumber,
        //                FilingDate = c.FilingDate
        //            });
        //        }
        //    }

        //    _context.LegalAssistants.Add(legalAssistant);
        //    await _context.SaveChangesAsync();

        //    var savedLegalAssistant = await _context.LegalAssistants
        //        .Include(la => la.LegalAssistantCases)
        //            .ThenInclude(lac => lac.Case)
        //        .FirstOrDefaultAsync(la => la.LegalAssistantId == legalAssistant.LegalAssistantId);

        //    var legalAssistantReadDto = new LegalAssistantReadDto
        //    {
        //        LegalAssistantId = savedLegalAssistant.LegalAssistantId,
        //        LegalAssistantFname = savedLegalAssistant.LegalAssistantFname,
        //        LegalAssistantLname = savedLegalAssistant.LegalAssistantLname,
        //        JoinDate = savedLegalAssistant.JoinDate,
        //        Email = savedLegalAssistant.Email,
        //        MobileNo = savedLegalAssistant.MobileNo,
        //        IsActive = savedLegalAssistant.IsActive,
        //        MonthlyStipend = savedLegalAssistant.MonthlyStipend,
        //        NidNumber = savedLegalAssistant.NidNumber,
        //        Picture = savedLegalAssistant.Picture,
        //        BarLicenseNumber = savedLegalAssistant.BarLicenseNumber,
        //        LegalAssistantCases = savedLegalAssistant.LegalAssistantCases.Select(lac => new LegalAssistantCaseReadDto
        //        {
        //            CaseId = lac.CaseId,
        //            CaseNumber = lac.Case?.CaseNumber,
        //            CaseTitle = lac.CaseTitle,
        //            Firnumber = lac.Firnumber,
        //            FilingDate = lac.FilingDate
        //        }).ToList()
        //    };

        //    return CreatedAtAction(nameof(GetLegalAssistant), new { id = legalAssistantReadDto.LegalAssistantId }, legalAssistantReadDto);
        //}



    }
}
