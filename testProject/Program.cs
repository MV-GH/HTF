using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using testProject.Nodes;
using testProject.ResponseModels;
using Newtonsoft.Json.Linq;


namespace testProject
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //    https://involved-htf-challenge.azurewebsites.net/api/path/1/easy/Puzzle

            var client = new HttpClient();
            // De base url die voor alle calls hetzelfde is
            client.BaseAddress = new Uri("http://involved-htf-challenge.azurewebsites.net");

            // De token die je gebruikt om je team te authenticeren, deze kan je via de swagger ophalen met je teamname + password
            var token =
                "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiMzYiLCJuYmYiOjE2Mzg0NzY0NDQsImV4cCI6MTYzODU2Mjg0NCwiaWF0IjoxNjM4NDc2NDQ0fQ.akhBhJJgptBo4W9hEbXzCWWuRM0FPyZXtL-9In7tv03abWdIZw8KcIC-3xET58MnFJe039hG4odZ6An_5MafSQ";
            // We stellen de token in zodat die wordt meegestuurd bij alle calls, anders krijgen we een 400 Unauthorized response op onze calls
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // SolvePuzzleA1(client); Works

            // await SolvePuzzleA2(client); Works

            await SolvePuzzleA3(client);


            // checkResult(client);
        }

        static async void checkResult(HttpClient client)
        {
            var progress = await client.GetAsync("/api/team/Progress");
            var result = await progress.Content.ReadAsStringAsync();
            Console.WriteLine(result);
        }


        static async Task SolvePuzzleA1(HttpClient client)
        {
            var url1 = "api/path/1/easy/Sample";
            var url2 = "api/path/1/easy/Puzzle";
            var startResponse = await client.GetAsync(url1);
            var responseBody = await startResponse.Content.ReadFromJsonAsync<IEnumerable<int>>();
            responseBody.ToList().ForEach(Console.WriteLine);
            var test = SolveAlgo(responseBody);
            Console.WriteLine(test);
            // var result = await client.PostAsJsonAsync(url1, test);
        }

        static async Task SolvePuzzleA2(HttpClient client)
        {
            var url1 = "api/path/1/medium/Puzzle";
            var url2 = "api/path/1/medium/Sample";
            var startUrl = "api/path/1/medium/Start";

            var startResponse = await client.GetAsync(url1);
            Console.WriteLine(startResponse);
            var responseBody = await startResponse.Content.ReadFromJsonAsync<ResponseA2>();

            Console.WriteLine(responseBody);
            var response = SolveA2Algo(responseBody);
            response.ToList().ForEach(Console.WriteLine);
            // var result = await client.PostAsJsonAsync(url1, response);
        }

        static async Task SolvePuzzleA3(HttpClient client)
        {
            var url1 = "api/path/1/hard/Puzzle";
            var url2 = "api/path/1/hard/Sample";
            var startUrl = "api/path/1/hard/Start";

            var response = await client.GetAsync(url1);
            
            Console.WriteLine(await response.Content.ReadAsStringAsync());
            
            var body = await response.Content.ReadFromJsonAsync<ResponseA3>();
            
            var result = await client.PostAsJsonAsync(url1, SolveA3Algo(body));

            Console.WriteLine(await result.Content.ReadAsStringAsync());
        }


        static IEnumerable<int> ResponseToListPath(ResponseA3 resp)
        {
            var root = new Node<Tile>(resp.tiles[0]);
            var correctNode = Director.AddChildren(root, resp.tiles);
            Node<Tile>.PrintTree(correctNode.GetRoot());
            return Director.GetListOfParent(correctNode);
        }

        static IEnumerable<int> SolveA3Algo(ResponseA3 resp) => ResponseToListPath(resp).Reverse();


        static IList<int> SolveA2Algo(ResponseA2 resp)
        {
            var current = resp.start;
            var step = 1;
            var steps = new List<int>() { 0 };
            while (current != resp.destination)
            {
                current += step;
                steps.Add(current);
                step = step > 0 ? -(step + 1) : -step + 1;
            }

            return steps;
        }

        static int SolveAlgo(IEnumerable<int> list)
        {
            var sum = list.Sum(x => x);
            while (sum > 9)
            {
                var temp = SplitNumberToList(sum);
                sum = SolveAlgo(temp);
            }

            return sum;
        }

        static IEnumerable<int> SplitNumberToList(int number)
        {
            return number.ToString().Select(digit => int.Parse(digit.ToString()));
        }

        async static void Start(HttpClient client)
        {
            // De url om de challenge te starten
            var startUrl = "api/path/0/easy/Start";
            // We voeren de call uit en wachten op de response
            // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/await
            // De start endpoint moet je 0 keer oproepen voordat je aan een challenge kan beginnen
            // Krijg je een 402 Forbidden response op je Sample of Puzzle calls? Dat betekent dat de challenge niet gestart is
            var startResponse = await client.GetAsync(startUrl);
            Console.WriteLine(startResponse);
        }

        async static void Run()
        {
            // De httpclient die we gebruiken om http calls te maken
            var client = new HttpClient();
            // De base url die voor alle calls hetzelfde is
            client.BaseAddress = new Uri("http://involved-htf-challenge.azurewebsites.net");

            // De token die je gebruikt om je team te authenticeren, deze kan je via de swagger ophalen met je teamname + password
            var token = "yzonlicht";
            // We stellen de token in zodat die wordt meegestuurd bij alle calls, anders krijgen we een 401 Unauthorized response op onze calls
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            // De url om de challenge te starten
            var startUrl = "api/path/1/easy/Start";
            // We voeren de call uit en wachten op de response
            // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/await
            // De start endpoint moet je 1 keer oproepen voordat je aan een challenge kan beginnen
            // Krijg je een 403 Forbidden response op je Sample of Puzzle calls? Dat betekent dat de challenge niet gestart is
            var startResponse = await client.GetAsync(startUrl);
            Console.WriteLine("-------------------------------");

            Console.WriteLine(startResponse);
        }
    }
}