using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Test_Framework;
using Test_Framework.Models;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Text;

namespace Test_Framework.Controllers
{
    public class TestController : Controller
    {
        private readonly TestContext _context;

        public TestController(TestContext context)
        {
            _context = context;
        }

        // GET: Test
        public async Task<IActionResult> Index()
        {
            return View(await _context.Tests.ToListAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<string> Run([Bind("ID,Name,Variables,Urls")] TestModel testModel)
        {
            JArray urls = JArray.Parse(testModel.Urls);
            List<Dictionary<string, string>> paramList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(testModel.Variables);
            var httpClient = new HttpClient();
            StringBuilder summary = new StringBuilder();
            summary.Append($"Number of urls: {urls.Count}");
            StringBuilder failedUrls = new StringBuilder();
            StringBuilder errorDetails = new StringBuilder();
            StringBuilder traceDetails = new StringBuilder();

            int failedUrlsCount = 0;
            foreach (var url in urls)
            {
                bool failed = false;
                var response = await httpClient.GetAsync(url.Value<string>());
                traceDetails.Append($"Testing url: {url}");
                traceDetails.Append(Environment.NewLine);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    failed = true;
                    ++failedUrlsCount;
                    failedUrls.Append(url);
                    failedUrls.Append(Environment.NewLine);
                    errorDetails.Append($"Failed!! Response code was {response.StatusCode}");
                    errorDetails.Append(Environment.NewLine);
                    continue;
                }
                var body = await response.Content.ReadAsStringAsync();
               
                //var jsonObject = JObject.Parse(body);
                foreach (var variable in paramList)
                {
                    var pair = variable.FirstOrDefault();
                    string testValue = $"\"{pair.Key}\":\"{pair.Value}\"";
                    traceDetails.Append($"Testing parameter: {pair.Key} with value {pair.Value}");
                    traceDetails.Append(Environment.NewLine);
                    bool test = body.Contains(testValue, StringComparison.InvariantCultureIgnoreCase);
                    if (test == false)
                    {
                        if (!failed)
                        {
                            ++failedUrlsCount;
                            failedUrls.Append(url);
                            failedUrls.Append(Environment.NewLine);
                            failed = true;
                        }
                        errorDetails.Append($"Failed!! Couldn't find {testValue}");
                        errorDetails.Append(Environment.NewLine);
                    }
                    //var results = (JArray)jsonObject["result"];
                    //bool exists = results.Any(p => p.Value<string>(pair.Key) == pair.Value);
                    
                }
            }
            StringBuilder result = new StringBuilder();
            if(failedUrlsCount == 0)
            {
                result.Append("All tests PASSED!!!");
                result.Append(Environment.NewLine);
            }
            else
            {
                result.Append($"Number of failed Urls: {failedUrlsCount}");
                result.Append(Environment.NewLine);
                result.Append($"Failed Urls..");
                result.Append(Environment.NewLine);
                result.Append($"********************************************************************************");
                result.Append(Environment.NewLine);
                result.Append(failedUrls.ToString());
                result.Append(Environment.NewLine);
                result.Append($"********************************************************************************");
                result.Append(Environment.NewLine);
                result.Append($"Erros..");
                result.Append(Environment.NewLine);
                result.Append(errorDetails.ToString());
            }
            result.Append(Environment.NewLine);
            result.Append($"********************************************************************************");
            result.Append(Environment.NewLine);
            result.Append($"Tracing ..");
            result.Append(Environment.NewLine);
            result.Append(traceDetails.ToString());
            return result.ToString();
        }

        // GET: Test/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testModel = await _context.Tests
                .FirstOrDefaultAsync(m => m.ID == id);
            if (testModel == null)
            {
                return NotFound();
            }

            return View(testModel);
        }

        // GET: Test/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Test/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Variables,Urls")] TestModel testModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(testModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(testModel);
        }

        // GET: Test/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testModel = await _context.Tests.FindAsync(id);
            if (testModel == null)
            {
                return NotFound();
            }
            return View(testModel);
        }

        // POST: Test/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Variables,Urls")] TestModel testModel)
        {
            if (id != testModel.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(testModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestModelExists(testModel.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(testModel);
        }

        // GET: Test/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testModel = await _context.Tests
                .FirstOrDefaultAsync(m => m.ID == id);
            if (testModel == null)
            {
                return NotFound();
            }

            return View(testModel);
        }

        // POST: Test/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var testModel = await _context.Tests.FindAsync(id);
            _context.Tests.Remove(testModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TestModelExists(int id)
        {
            return _context.Tests.Any(e => e.ID == id);
        }
    }
}
