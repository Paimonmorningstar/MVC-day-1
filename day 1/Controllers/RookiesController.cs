using System.IO;
using Microsoft.AspNetCore.Mvc;
using CsvHelper;
using day_1.Models;

namespace day_1.Controllers
{
    [Route("[controller]")]
    public class RookiesController : Controller
    {
        private static List<PersonModlel> _list = new List<PersonModlel>
        {
   new PersonModlel
   {
       FirstName = "Loc",
       LastName = "Nguyen Thanh",
       Gender = "Male",
       DateOfBirth = new DateTime(2000, 01, 21),
       PhoneNumber = "3333231313",
       BirthPlace = "hanoi",
       IsGraduated = false
   },
   new PersonModlel
   {
       FirstName = "Linh",
       LastName = "abc xyz",
       Gender = "Female",
       DateOfBirth = new DateTime(2001, 03, 20),
       PhoneNumber = "23131333621",
       BirthPlace = "bac ninh",
       IsGraduated = false
   },
   new PersonModlel
   {
       FirstName = "Hung",
       LastName = "abcd xyzt",
       Gender = "Male",
       DateOfBirth = new DateTime(1999, 03, 20),
       PhoneNumber = "231313336821",
       BirthPlace = "lao cai",
       IsGraduated = false
   }
        };
        private readonly ILogger<RookiesController> _logger;

        public RookiesController(ILogger<RookiesController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return Json(_list);
        }

        public IActionResult GetMaleMember()
        {
            var data = _list.Where(m => m.Gender == "Male");
            return Json(data);
        }
        public IActionResult GetOldestMember()
        {
            var maxAge = _list.Max(m => m.Age);
            var oldest = _list.FirstOrDefault(o => o.Age == maxAge);
            return Json(oldest);
        }
        public IActionResult FullName()
        {
            var fullNames = _list.Select(n => n.FullName);
            return Json(fullNames);
        }

        private IActionResult GetMemberByBirthYear(int year, string compareType)
        {
            switch (compareType)
            {
                case "equal":
                    return Json(_list.Where(y => y.DateOfBirth.Year == year));
                case "greaterThan":
                    return Json(_list.Where(y => y.DateOfBirth.Year > year));
                case "lessthan":
                    return Json(_list.Where(y => y.DateOfBirth.Year < year));
                default:
                    return Json(null);
            }
        }
        public IActionResult GetMemberWhoBornIn2000()
        {
            return RedirectToAction("GetMembersByBirthYear", new { year = 2000, comparetype = "equal" });
        }
        public IActionResult GetMembersWhoBornAfter2000()
        {
            return RedirectToAction("GetNembersByBirthYear", new { year = 2000, compareType = "greaterthan" });
        }

        public IActionResult GetMembersWhoBornBefore2000()
        {
            return RedirectToAction("GetNembersByBirthYear", new { year = 2000, compareType = "lessthan" });
        }

        public byte[] WriteCsvToMemory(IEnumerable<PersonModlel> people)
        {
            using (var streamMemory = new MemoryStream())
            using (var streamWriter = new StreamWriter(streamMemory))
            using (var csvWriter = new CsvWriter(streamWriter, System.Globalization.CultureInfo.CurrentCulture))
            {
                csvWriter.WriteRecords(people);
                streamWriter.Flush();
                return streamMemory.ToArray();
            }
        }

        public FileStreamResult Export()
        {
            var result = WriteCsvToMemory(_list);
            var memoryStream = new MemoryStream(result);
            return new FileStreamResult(memoryStream, "text/csv") { FileDownloadName = "report.csv" };
        }

    }
}