using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using MovieLibrary.Models;

namespace MovieLibrary.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class MovieController
    {
        static HttpClient client = new HttpClient();
        static string topMovieListUrl = "https://ithstenta2020.s3.eu-north-1.amazonaws.com/topp100.json";
        static string detailedMovieListUrl = "https://ithstenta2020.s3.eu-north-1.amazonaws.com/detailedMovies.json";

        [HttpGet]
        [Route("/toplist")]
        public List<string> GetToplist(bool sortByAscending = true)
        {

            IEnumerable<TopListMovie> topmovieList = GetMovieListFromUrl<TopListMovie>(topMovieListUrl);
            IEnumerable<DetailedMovie> detailedMovieList = GetMovieListFromUrl<DetailedMovie>(detailedMovieListUrl);
            IEnumerable<TopListMovie> mergedmovieList = MergeMovieLists(topmovieList, detailedMovieList);
            IEnumerable<TopListMovie> sortedMovieList = SortMovieListByRate(sortByAscending, mergedmovieList);
            List<string> topList = ConvertListToOnlyTitle(sortedMovieList);

            return topList;
        }


        private List<string> ConvertListToOnlyTitle(IEnumerable<TopListMovie> sortedMovieList)
        {
            List<string> movieToplist = new List<string>();
            foreach (var m in sortedMovieList)
            {
                //movieToplist.Add(m.id + " " + m.title + " " + m.rated);
                movieToplist.Add(m.title);
            }
            return movieToplist;
        }

        private IEnumerable<TopListMovie> SortMovieListByRate(bool sortByAscending, IEnumerable<TopListMovie> movieList)
        {
            if (sortByAscending)
            {
                return movieList.OrderBy(movie => movie.rated);
            }
            return movieList.OrderByDescending(movie => movie.rated);
        }

        private IEnumerable<TopListMovie> MergeMovieLists(IEnumerable<TopListMovie> topmovieList, IEnumerable<DetailedMovie> detailedMovieList)
        {
            List<TopListMovie> additionalMovies = new List<TopListMovie>();
            foreach (var movie in detailedMovieList)
            {
                if (!topmovieList.Any(m => m.title == movie.title))
                {
                    additionalMovies.Add(new TopListMovie() { title = movie.title, id = movie.id, rated = movie.imdbRating.ToString() });
                }
            }
            return topmovieList.Concat(additionalMovies);
        }

        private IEnumerable<T> GetMovieListFromUrl<T>(string url)
        {
            var response = client.GetAsync(url).Result;
            var content = new StreamReader(response.Content.ReadAsStream()).ReadToEnd();
            var movieList = JsonSerializer.Deserialize<IEnumerable<T>>(content);
            return movieList;
        }

        [HttpGet]
        [Route("/movie")]
        public TopListMovie GetMovieById(string id)
        {

            IEnumerable<TopListMovie> topMovieList = GetMovieListFromUrl<TopListMovie>(topMovieListUrl);
            IEnumerable<DetailedMovie> detailedMovieList = GetMovieListFromUrl<DetailedMovie>(detailedMovieListUrl);

            TopListMovie movieFromTopList = GetMovieFromTopList(id, topMovieList);
            DetailedMovie movieFromDetailedList = GetMovieFromDetailedList(id, detailedMovieList);


            if (movieFromTopList == null && movieFromDetailedList == null)
            {
                return null;
            }
            if (movieFromTopList.title == movieFromDetailedList.title)
            {

            }


            return movieFromTopList;
        }

        private TopListMovie GetMovieFromTopList(string id, IEnumerable<TopListMovie> movieList)
        {
            try
            {
                TopListMovie movie = movieList.Single(movie => movie.id == id);
                return movie;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private DetailedMovie GetMovieFromDetailedList(string id, IEnumerable<DetailedMovie> movieList)
        {
            try
            {
                DetailedMovie movie = movieList.Single(movie => movie.id == id);
                return movie;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}