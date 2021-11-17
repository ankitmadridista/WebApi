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

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public DepartmentController(IConfiguration configuration) {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get() {
            string query = @"select DepartmentId, DepartmentName from dbo.Department";

            DataTable dt = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");

            SqlDataReader myReader;
            using (SqlConnection con = new SqlConnection(sqlDataSource)) {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query, con)) {
                    myReader = cmd.ExecuteReader();
                    dt.Load(myReader);
                    con.Close();
                }
            }
            return new JsonResult(dt);
        }

        [HttpPost]
        public JsonResult Post(Department dept) {

            string query = @"insert into dbo.Department values ('"+dept.DepartmentName+@"')";

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
            return new JsonResult(string.Format("{0} Added successfully", dept.DepartmentName));
        }
        
        [HttpPut]
        public JsonResult Put(Department dept) {
            string query = @"
                            update dbo.Department set 
                            DepartmentName = '"+dept.DepartmentName + @"'
                            where DepartmentId = " +dept.DepartmentId + @"
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
            return new JsonResult(string.Format("Department with Department ID: {0} is Updated with new Department Name '{1}' successfully", dept.DepartmentId,dept.DepartmentName));
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"
                            delete from dbo.Department                            
                            where DepartmentId = " + id + @"
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
            return new JsonResult(string.Format("Department with Department ID: {0} is deleted successfully", id));
        } 
    }
}