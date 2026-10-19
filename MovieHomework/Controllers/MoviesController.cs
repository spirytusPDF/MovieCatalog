using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieHomework.Models;

namespace MovieHomework.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        public static List<Movie> movies = new List<Movie>
            {
               new Movie { Id = 1, Title = "...And justice for all", Year = 1979, Genre = "Thriller/crime", Rating = 7.4 },
               new Movie { Id = 2, Title = "Scent of a woman", Year = 1992, Genre = "Drama", Rating = 8.0 },
               new Movie { Id = 3, Title = "The Godfather", Year = 1972, Genre = "Crime/Drama", Rating = 9.2 }
            };
        [HttpGet]
        public IActionResult GetMovies()
        {
            if (movies.Count == 0 || movies == null)
            {
                return NotFound("No movies found.");
            }
            return Ok(movies);
        }

        [HttpGet("{id:int:min(1)}")]
        public IActionResult GetAMovie([FromRoute] int id)
        {
            var movie = movies.FirstOrDefault(m => m.Id == id);
            if (movie == null)
            {
                return NotFound("Movie not found.");
            }
            return Ok(movie);
        }

        [HttpGet("/title/{title}")]
        public IActionResult GetAMovieByTitle([FromRoute] string title)
        {
            var movie = movies.FirstOrDefault(m => m.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
            if (movie == null)
            {
                return NotFound("Movie not found.");
            }
            return Ok(movie);
        }

        [HttpGet("/search/")]
        public IActionResult GetAMovieByTitle([FromQuery] string genre, [FromQuery] double rating = 0)
        {
            var moviesFind = movies.Where(m => m.Genre.Contains(genre, StringComparison.OrdinalIgnoreCase) && m.Rating>=rating);
            if (moviesFind == null || moviesFind.Count() == 0)
            {
                return NotFound("Movies are not found.");
            }
            return Ok(moviesFind);
        }

        [HttpPost]
        public IActionResult AddMovie([FromBody] Movie movie)
        {
            //it's wrong to let the user input ID and then change it. I've thought about the solution:
            //Probably it's best to create a MovieWrapper that wil be used and then assign it to original Movie obj with added ID
            if (movie == null) return BadRequest();
            movie.Id = movies.MaxBy(m => m.Id).Id + 1;
            movies.Add(movie);
            return Ok(movies);
        }

        [HttpDelete]
        public IActionResult DeleteMovie([FromBody] int id)
        {
            if (id <=0 ) return BadRequest();
            var movie = movies.FirstOrDefault(m => m.Id == id);
            if(movie == null) return NotFound();
            movies.Remove(movie);
            return Ok(movies);
        }
    }
}
