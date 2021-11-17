using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebApi.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _env;

        public EmployeeController(IConfiguration configuration, IHostingEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                            select EmployeeId, EmployeeName, Department, 
                            convert(varchar(10),DateOfJoining,120) as DateOfJoining, PhotoFileName 
                            from dbo.Employee
                            ";

            DataTable dt = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");

            SqlDataReader myReader;
            using (SqlConnection con = new SqlConnection(sqlDataSource))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    myReader = cmd.ExecuteReader();
                    dt.Load(myReader);
                    con.Close();
                }
            }
            return new JsonResult(dt);
        }

        [HttpPost]
        public JsonResult Post(Employee emp)
        {

            string query = @"
                            insert into dbo.Employee 
                            (EmployeeName, Department, DateOfJoining, PhotoFileName)
                            values (
                            '" + emp.EmployeeName+ @"', 
                            '" + emp.Department + @"',
                            '" + emp.DateOfJoining + @"',
                            '" + emp.PhotoFileName + @"'
                            )
                            ";

            DataTable dt = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");

            SqlDataReader myReader;
            using (SqlConnection con = new SqlConnection(sqlDataSource))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    myReader = cmd.ExecuteReader();
                    dt.Load(myReader);
                    con.Close();
                }
            }
            return new JsonResult(string.Format("Employee {0} Added successfully", emp.EmployeeName));
        }

        [HttpPut]
        public JsonResult Put(Employee emp)
        {
            string query = @"
                            update dbo.Employee set 
                            EmployeeName = '" + emp.EmployeeName + @"',
                            Department = '" + emp.Department + @"',
                            DateOfJoining = '" + emp.DateOfJoining + @"',
                            PhotoFileName = '" + emp.PhotoFileName + @"'
                            where EmployeeId = " + emp.EmployeeId + @"
                            ";

            DataTable dt = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");

            SqlDataReader myReader;
            using (SqlConnection con = new SqlConnection(sqlDataSource))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    myReader = cmd.ExecuteReader();
                    dt.Load(myReader);
                    con.Close();
                }
            }
            return new JsonResult(string.Format("Employee with Employee ID: {0} is Updated successfully", emp.EmployeeId));
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"
                            delete from dbo.Employee                            
                            where EmployeeId = " + id + @"
                            ";

            DataTable dt = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");

            SqlDataReader myReader;
            using (SqlConnection con = new SqlConnection(sqlDataSource))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    myReader = cmd.ExecuteReader();
                    dt.Load(myReader);
                    con.Close();
                }
            }
            return new JsonResult(string.Format("Employee with Employee ID: {0} is deleted successfully", id));
        }

        [Route("SaveFile")]
        [HttpPost]

        public JsonResult SaveFile() {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string fileName = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "\\Photos\\" + fileName;

                using (var stream = new FileStream(physicalPath, FileMode.Create)) {

                    postedFile.CopyTo(stream);
                }

                return new JsonResult(fileName);
            }
            catch (Exception)
            {
                return new JsonResult("anonymous.jpg");
            }
        }

        [Route("GetAllDepartmentNames")]
        [HttpGet]
        public JsonResult GetAllDepartmentNames() {
            string query = @"
                            select DepartmentName 
                            from dbo.Department
                            ";

            DataTable dt = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");

            SqlDataReader myReader;
            using (SqlConnection con = new SqlConnection(sqlDataSource))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    myReader = cmd.ExecuteReader();
                    dt.Load(myReader);
                    con.Close();
                }
            }
            return new JsonResult(dt);
        }
        
    }
}