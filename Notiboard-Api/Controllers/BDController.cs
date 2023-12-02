using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Notiboard_Api.Data;
using Notiboard_Api.Model;

namespace Notiboard_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BDController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BDController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/BD
        [HttpGet]
        public IActionResult GetBoards()
        {
            var content = _context.Boards.Include(b => b.Group).ToList();

            if (content == null)
            {
                return BadRequest();
            }
            return Ok(content);
        }

        // GET: api/GetGroup
        [HttpGet("GetGroup")]
        public async Task<ActionResult<IEnumerable<Group>>> GetGroup()
        {
            if (_context.Groups == null)
            {
                return NotFound();
            }
            return await _context.Groups.ToListAsync();
        }

        // GET: api/GetGroup/Id}
        [HttpGet("GetGroup/{id}")]
        public async Task<ActionResult<Group>> GetGroup(int id)
        {
            if (_context.Groups == null)
            {
                return NotFound();
            }
            var Data = await _context.Groups.FindAsync(id);

            if (Data == null)
            {
                return NotFound();
            }

            return Data;
        }


        // GET: api/BD/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Board>> GetBoard(int id)
        {
          if (_context.Boards == null)
          {
              return NotFound();
          }
            var board = await _context.Boards.FindAsync(id);

            if (board == null)
            {
                return NotFound();
            }

            return board;
        }

        // PUT: api/BD/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBoard(int id, Board board)
        {
            if (id != board.ID)
            {
                return BadRequest();
            }

            _context.Entry(board).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BoardExists(id))
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

        [HttpPut("PutGroup/{id}")]
        public async Task<IActionResult> PutGroup(int id, Group Data)
        {
            if (id != Data.ID)
            {
                return BadRequest();
            }

            _context.Entry(Data).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BoardExists(id))
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

        // POST: api/BD
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Board>> PostBoard(Board board)
        {
          if (_context.Boards == null)
          {
              return Problem("Entity set 'AppDbContext.Boards'  is null.");
          }
            _context.Boards.Add(board);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBoard", new { id = board.ID }, board);
        }
        
        [HttpPost("PostGroup")]
        public async Task<ActionResult<Board>> PostGroup(Group  Group)
        {
            if (_context.Groups == null)
            {
                return Problem("Entity set 'AppDbContext.Boards'  is null.");
            }
            _context.Groups.Add(Group);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBoard", new { id = Group.ID }, Group);
        }

        // DELETE: api/BD/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBoard(int id)
        {
            if (_context.Boards == null)
            {
                return NotFound();
            }
            var board = await _context.Boards.FindAsync(id);
            if (board == null)
            {
                return NotFound();
            }

            _context.Boards.Remove(board);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/DeleteGroup/{Id}
        [HttpDelete("DeleteGroup/{id}")]
        public async Task<IActionResult> DeleteGroup(int id)
        {
            if (_context.Groups == null)
            {
                return NotFound();
            }
            var board = await _context.Groups.FindAsync(id);
            if (board == null)
            {
                return NotFound();
            }

            _context.Groups.Remove(board);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BoardExists(int id)
        {
            return (_context.Boards?.Any(e => e.ID == id)).GetValueOrDefault();
        }

        // GET: api/BD/GetBoardsByGroup/{groupId}
        [HttpGet("GetByGroup/{groupId}")]
        public IActionResult GetBoardsByGroup(int groupId)
        {
            var boardsInGroup = _context.Boards.Include(b => b.Group)
                                              .Where(b => b.GroupID == groupId)
                                              .ToList();

            if (boardsInGroup == null || !boardsInGroup.Any())
            {
                return NotFound($"No boards found for Group ID {groupId}");
            }

            return Ok(boardsInGroup);
        }
    }
}
 