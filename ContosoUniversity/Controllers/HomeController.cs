﻿using ContosoUniversity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Data;
using ContosoUniversity.Models.SchoolViewModels;

namespace ContosoUniversity.Controllers
{
    public class HomeController : Controller
    {
        private readonly SchoolContext _context;

        public HomeController(SchoolContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> About()
        {
            IQueryable<EnrollmentDateGroup> data =
                from student in _context.Students
                group student by student.EnrollmentDate into dateGroup
                select new EnrollmentDateGroup()
                {
                    EnrollmentDate = dateGroup.Key,
                    StudentCount = dateGroup.Count()
                };
            return View(await data.AsNoTracking().ToListAsync());
        }

        // Tutorial code that relies on the inheritance step that broke.
        //public async Task<ActionResult> About()
        //{
        //    List<EnrollmentDateGroup> groups = new List<EnrollmentDateGroup>();
        //    var conn = _context.Database.GetDbConnection();
        //    try
        //    {
        //        await conn.OpenAsync();
        //        using (var command = conn.CreateCommand())
        //        {
        //            string query = "SELECT EnrollmentDate, COUNT(*) AS StudentCount "
        //                + "FROM Person "
        //                + "WHERE Discriminator = 'Student' "
        //                + "GROUP BY EnrollmentDate";
        //            command.CommandText = query;
        //            DbDataReader reader = await command.ExecuteReaderAsync();

        //            if (reader.HasRows)
        //            {
        //                while (await reader.ReadAsync())
        //                {
        //                    var row = new EnrollmentDateGroup { EnrollmentDate = reader.GetDateTime(0), StudentCount = reader.GetInt32(1) };
        //                    groups.Add(row);
        //                }
        //            }
        //            reader.Dispose();
        //        }
        //    }
        //    finally
        //    {
        //        conn.Close();
        //    }
        //    return View(groups);
        //}

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
