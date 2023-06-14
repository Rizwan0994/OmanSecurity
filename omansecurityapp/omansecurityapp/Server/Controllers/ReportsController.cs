using omansecurityapp.Shared.Models;
using omansecurityapp.Server.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using omansecurityapp.Client.Pages;

namespace omansecurityapp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : Controller
    {
        readonly IReports _IReports;

        public ReportsController(IReports IReports)
        {
            _IReports = IReports;
        }



        // get all reports from reports database to dissplay to new users
        [HttpGet("[action]")]
        public async Task<List<InvestigationOfficerReports>> ReportsForMainpage()
        {
            var reports = await _IReports.ReportsForMainpage();
            return reports;
        }


        //For reports submitted by users
        [HttpPost("AddNewReport")]
        public async Task<IActionResult> AddNewReport([FromBody] AddNewReports newReport)
        {
            try
            {

                // Save the report to the database including the media file
                var report = new AddNewReports
                {
                    ReportType = newReport.ReportType,
                    Location = newReport.Location,
                    Description = newReport.Description
                };

                if (newReport.Media != null && newReport.MediaType != null)
                {
                    report.Media = newReport.Media;
                    report.MediaType = newReport.MediaType;
                }

                await _IReports.AddNewReport(report);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        // update priority and assigned officer of a report in the database
        [HttpPut("[action]")]
        public async Task<ActionResult> SetPriorityAndAssignOfficer([FromBody] PoliceOfficerReports report)
        {
            try
            {
                await _IReports.SetPriorityAndAssignOfficer(report);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // update status of a report in the database
        [HttpPut("[action]")]
        public async Task<ActionResult> SetStatus([FromBody] InvestigationOfficerReports report)
        {
            try
            {
                await _IReports.SetStatus(report);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // get all submitted reports from the database
        [HttpGet("[action]")]
        public async Task<List<AddNewReports>> GetSubmittedReports()
        {
            var reports = await _IReports.GetSubmittedReports();
            return reports;
        }

        // get all reports assigned to a police officer from the database
        [HttpGet("[action]")]
        public async Task<List<PoliceOfficerReports>> GetReportsFromPoliceOfficer()
        {
            var reports = await _IReports.GetReportsFromPoliceOfficer();
            return reports;
        }

        // get all reports assigned to an investigation officer from the database
        [HttpGet("[action]")]
        public async Task<List<InvestigationOfficerReports>> GetReportsFromInvestigationOfficer()
        {
            var reports = await _IReports.GetReportsFromInvestigationOfficer();
            return reports;
        }
    }
}
