using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Inspari_API.Models;
using Inspari_API.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dapper_ORM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly IDapper _dapper;
        private static readonly string _selectPageSQL = @"
        SELECT * FROM [Todo]
        ORDER BY [Todo].Deadline DESC
        OFFSET @PageSize * (@PageNumber - 1) ROWS
        FETCH NEXT @PageSize ROWS ONLY";

        public TodoController(IDapper dapper)
        {
            _dapper = dapper;
        }

        [HttpGet]
        public async Task<IEnumerable<TodoItem>> Get(int page = 1, int size = 100)
        {
            if (page < 1) page = 1;
            return await Task.FromResult(_dapper.GetAll<TodoItem>(_selectPageSQL, new { PageSize = size, PageNumber = page }));
        }

        [HttpGet("{Id}")]
        public async Task<TodoItem> GetById(int Id)
        {
            return await Task.FromResult(_dapper.Get<TodoItem>($"Select * from [Todo] where Id = @Id", new { Id }));
        }

        [HttpGet(nameof(Count))]
        public Task<int> Count()
        {
            return Task.FromResult(_dapper.Get<int>($"SELECT COUNT(*) FROM [Todo]", null));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TodoItem data)
        {
            if (string.IsNullOrEmpty(data.Name)) return BadRequest("Name is required");

            TodoItem result = await Task.FromResult(_dapper.Insert<TodoItem>("INSERT INTO [Todo] (Name, Description, IsComplete, Deadline) VALUES (@Name, @Description, @IsComplete, @Deadline)", data));
            return Ok(result);
        }

        [HttpPatch("{Id}")]
        public async Task<IActionResult> Update(int Id, [FromBody] TodoItem data)
        {
            if ((long?)Id is null) return BadRequest("ID Required");
            if (Id != data.Id) return BadRequest("ID Mismatch");

            var currentTodo = await Task.FromResult(_dapper.Get<TodoItem>($"Select * from [Todo] where Id = @Id", new { Id = data.Id }));

            data.Name ??= currentTodo.Name;
            data.Description ??= currentTodo.Description;
            data.IsComplete ??= currentTodo.IsComplete;
            data.Deadline ??= currentTodo.Deadline;
           
            return Ok(await Task.FromResult(_dapper.Update<int>("UPDATE [Todo] SET Name=@Name, Description=@Description, IsComplete=@IsComplete, Deadline=@Deadline WHERE Id = @Id", data)));
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            var rowsAffected = await Task.FromResult(_dapper.Update<TodoItem>($"DELETE FROM [Todo] WHERE Id = @Id", new { Id }));

            return Ok(rowsAffected);
        }
    }
}