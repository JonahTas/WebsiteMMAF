using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MMAF.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using MMAF.Database;

namespace MMAF.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }



        [Route("Locatie")]
        public IActionResult Locatie()
        {
            return View();
        }

        [Route("Festivals")]
        public IActionResult Festivals()
        {
            return View();
        }

        [Route("Contact")]
        public IActionResult Contact()
        {
            return View();
        }
        [Route("Agenda")]
        public IActionResult Agenda()
        {
            return View();
        }

        [Route("FAQ")]
        public IActionResult FAQ()
        {
            return View();

        }


        public IActionResult Index()
        {
            // lijst met producten ophalen
            var products = GetAllProducts();

            // de lijst met producten in de html stoppen
            return View(products);
        }


        public List<Product> GetAllProducts()
        {
            // alle producten ophalen uit de database
            var rows = DatabaseConnector.GetRows("select * from product");

            // lijst maken om alle producten in te stoppen
            List<Product> products = new List<Product>();

            foreach (var row in rows)
            {
                // Voor elke rij maken we nu een product
                Product p = GetProductFromRow(row);

                // en dat product voegen we toe aan de lijst met producten
                products.Add(p);
            }

            return products;
        }


        public Product GetProduct(int id)
        {
            // product ophalen uit de database op basis van het id
            var rows = DatabaseConnector.GetRows($"select * from product where id = {id}");

            // We krijgen altijd een lijst terug maar er zou altijd één product in moeten
            // zitten dus we pakken voor het gemak gewoon de eerste
            Product product = GetProductFromRow(rows[0]);

            // Als laatste sturen het product uit de lijst terug
            return product;
        }

        private Product GetProductFromRow(Dictionary<string, object> row)
        {
            Product p = new Product();
            p.Naam = row["naam"].ToString();
            p.Prijs = row["prijs"].ToString();
            p.Beschikbaarheid = Convert.ToInt32(row["beschikbaarheid"]);
            p.Id = Convert.ToInt32(row["id"]);

            return p;
        }

        [HttpPost]
        [Route("Contact")]
        public IActionResult Contact(Person person)
        {

            ViewData["firstname"] = person.FirstName;
            ViewData["lastname"] = person.LastName;
            ViewData["email"] = person.Email;
            ViewData["description"] = person.Description;

            // hebben we alles goed ingevuld? Dan sturen we de gebruiker door naar de succes pagina
            if (ModelState.IsValid)
            {

                // alle benodigde gegevens zijn aanwezig, we kunnen opslaan!
                DatabaseConnector.SavePerson(person);

                return Redirect("/succes");
            }

            // niet goed? Dan sturen we de gegevens door naar de view zodat we de fouten kunnen tonen
            return View(person);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("product/{id}")]
        public IActionResult ProductDetails(int id)
        {
            var product = GetProduct(id);

            return View(product);
        }


    }
}


