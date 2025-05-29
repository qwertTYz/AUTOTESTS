global using NUnit;
global using System.Net.Http.Json;
global using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Http.Headers;


namespace API_Testing
{
    public class Tests
    {
        private HttpClient _client;
        private IConfiguration _config;
        private string _createdBookId;
        private string _duplicateIsbn;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var baseUrl = _config["ApiSettings:BaseUrl"];
            _client = new HttpClient { BaseAddress = new Uri(baseUrl) };
        }

        [Test, Order(1)]
        public async Task CreateBook_Valid_ReturnsCreated()
        {
            var endpoint = _config["ApiSettings:BooksEndpoint"];
            var model = new CreateBookModel
            {
                Title = "Test Driven Development",
                Author = "Kent Beck",
                Isbn = Guid.NewGuid().ToString(),
                PublishedDate = DateTime.UtcNow.Date
            };
            _duplicateIsbn = model.Isbn;

            var response = await _client.PostAsJsonAsync(endpoint, model);
            Assert.That(HttpStatusCode.Created == response.StatusCode);
                
            var created = await response.Content.ReadFromJsonAsync<Book>();
            Assert.That(created != null);
            Assert.That(model.Title, Is.EqualTo(created.Title));
            Assert.That(model.Author, Is.EqualTo(created.Author));
            Assert.That(model.Isbn, Is.EqualTo(created.Isbn));
            Assert.That(model.PublishedDate, Is.EqualTo(created.PublishedDate.Date));

            _createdBookId = created.Id;
        }

        [Test, Order(2)]
        public async Task CreateBook_DuplicateIsbn_ReturnsConflictOrBadRequest()
        {
            var endpoint = _config["ApiSettings:BooksEndpoint"];
            var model = new CreateBookModel
            {
                Title = "Another Title",
                Author = "Another Author",
                Isbn = _duplicateIsbn,
                PublishedDate = DateTime.UtcNow.Date
            };

            var response = await _client.PostAsJsonAsync(endpoint, model);
            Assert.That(
                response.StatusCode == HttpStatusCode.Conflict ||
                response.StatusCode == HttpStatusCode.BadRequest);
        }

        [Test, Order(3)]
        public async Task GetAllBooks_ReturnsList()
        {
            var endpoint = _config["ApiSettings:BooksEndpoint"];
            var list = await _client.GetFromJsonAsync<List<Book>>(endpoint);
            Assert.That(list.Count > 0);
        }

        [Test, Order(4)]
        public async Task GetBookById_Valid_ReturnsBook()
        {
            var endpoint = $"{_config["ApiSettings:BooksEndpoint"]}/{_createdBookId}";
            var response = await _client.GetAsync(endpoint);
            Assert.That(HttpStatusCode.OK, Is.EqualTo(response.StatusCode));
            var book = await response.Content.ReadFromJsonAsync<Book>();
            Assert.That(_createdBookId, Is.EqualTo(book.Id));
        }

        [Test, Order(5)]
        public async Task GetBookById_InvalidFormat_ReturnsBadRequest()
        {
            var endpoint = $"{_config["ApiSettings:BooksEndpoint"]}/invalid-guid";
            var response = await _client.GetAsync(endpoint);
            Assert.That(HttpStatusCode.BadRequest, Is.EqualTo(response.StatusCode));
        }

        [Test, Order(6)]
        public async Task GetBookById_Nonexistent_ReturnsNotFound()
        {
            var endpoint = $"{_config["ApiSettings:BooksEndpoint"]}/{Guid.NewGuid()}";
            var response = await _client.GetAsync(endpoint);
            Assert.That(HttpStatusCode.NotFound, Is.EqualTo(response.StatusCode));
        }

        [Test, Order(7)]
        public async Task UpdateBook_Valid_ReturnsNoContent()
        {
            var endpoint = $"{_config["ApiSettings:BooksEndpoint"]}/{_createdBookId}";
            var updated = new CreateBookModel
            {
                Title = "TDD Updated",
                Author = "Kent Beck Updated",
                Isbn = Guid.NewGuid().ToString(),
                PublishedDate = DateTime.UtcNow.Date
            };

            var response = await _client.PutAsJsonAsync(endpoint, updated);
            Assert.That(HttpStatusCode.NoContent, Is.EqualTo(response.StatusCode));

            var get = await _client.GetFromJsonAsync<Book>(endpoint);
            Assert.That(updated.Title, Is.EqualTo(get.Title));
            Assert.That(updated.Author, Is.EqualTo(get.Author));
            Assert.That(updated.Isbn, Is.EqualTo(get.Isbn));
        }

        [Test, Order(8)]
        public async Task UpdateBook_InvalidFormat_ReturnsBadRequest()
        {
            var endpoint = $"{_config["ApiSettings:BooksEndpoint"]}/bad-id";
            var model = new CreateBookModel
            {
                Title = "X",
                Author = "Y",
                Isbn = Guid.NewGuid().ToString(),
                PublishedDate = DateTime.UtcNow.Date
            };
            var response = await _client.PutAsJsonAsync(endpoint, model);
            Assert.That(HttpStatusCode.BadRequest, Is.EqualTo(response.StatusCode));
        }

        [Test, Order(9)]
        public async Task UpdateBook_Nonexistent_ReturnsNotFound()
        {
            var endpoint = $"{_config["ApiSettings:BooksEndpoint"]}/{Guid.NewGuid()}";
            var model = new CreateBookModel
            {
                Title = "X",
                Author = "Y",
                Isbn = Guid.NewGuid().ToString(),
                PublishedDate = DateTime.UtcNow.Date
            };
            var response = await _client.PutAsJsonAsync(endpoint, model);
            Assert.That(HttpStatusCode.NotFound, Is.EqualTo(response.StatusCode));
        }

        [Test, Order(10)]
        public async Task DeleteBook_Valid_ReturnsNoContent()
        {
            var endpoint = $"{_config["ApiSettings:BooksEndpoint"]}/{_createdBookId}";
            var response = await _client.DeleteAsync(endpoint);
            Assert.That(HttpStatusCode.NoContent, Is.EqualTo(response.StatusCode));

            var getResp = await _client.GetAsync(endpoint);
            Assert.That(HttpStatusCode.NotFound, Is.EqualTo(getResp.StatusCode));
        }

        [Test, Order(11)]
        public async Task DeleteBook_InvalidFormat_ReturnsBadRequest()
        {
            var endpoint = $"{_config["ApiSettings:BooksEndpoint"]}/abc";
            var response = await _client.DeleteAsync(endpoint);
            Assert.That(HttpStatusCode.BadRequest, Is.EqualTo(response.StatusCode));
        }

        [Test, Order(12)]
        public async Task DeleteBook_Nonexistent_ReturnsNotFound()
        {
            var endpoint = $"{_config["ApiSettings:BooksEndpoint"]}/{Guid.NewGuid()}";
            var response = await _client.DeleteAsync(endpoint);
            Assert.That(HttpStatusCode.NotFound, Is.EqualTo(response.StatusCode));
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            _client.Dispose();
        }

    }
}
