using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace testProject
{
    class Program
    {
        static async Task Main(string[] args)
        {



            //    https://involved-htf-challenge.azurewebsites.net/api/path/1/easy/Puzzle

            // De httpclient die we gebruiken om http calls te maken
            var client = new HttpClient();
            // De base url die voor alle calls hetzelfde is
            client.BaseAddress = new Uri("http://involved-htf-challenge.azurewebsites.net");

            // De token die je gebruikt om je team te authenticeren, deze kan je via de swagger ophalen met je teamname + password
            var token = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiMzYiLCJuYmYiOjE2Mzg0Mzc3NDYsImV4cCI6MTYzODUyNDE0NiwiaWF0IjoxNjM4NDM3NzQ2fQ._N-SIBKPD4w2-EjomXmNFoHppHHaTEUkaVEimPkczw1Wquq-Exq2XPv6QZjoMGgX_chTeVzx9WgJn74PHFM2dw";
            // We stellen de token in zodat die wordt meegestuurd bij alle calls, anders krijgen we een 400 Unauthorized response op onze calls
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // SolvePuzzleA1(client); Works

            // await SolvePuzzleA2(client); Works

            await SolvePuzzleA3(client);


            // checkResult(client);

        }

        async static void checkResult(HttpClient client)
        {
            var progress = await client.GetAsync("/api/team/Progress");
            var result = await progress.Content.ReadAsStringAsync();
            Console.WriteLine(result);
        }




        async static Task SolvePuzzleA1(HttpClient client)
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

        async static Task SolvePuzzleA2(HttpClient client)
        {
            var url1 = "api/path/1/medium/Puzzle";
            var url2 = "api/path/1/medium/Sample";
            var startUrl = "api/path/1/medium/Start";

            var startResponse = await client.GetAsync(url1);
            Console.WriteLine(startResponse);
            var responseBody = await startResponse.Content.ReadFromJsonAsync<ResponseA2>();

            // {"start":0,"destination":17}
            Console.WriteLine(responseBody);
            var response = SolveA2Algo(responseBody);
            response.ToList().ForEach(Console.WriteLine);
            // var result = await client.PostAsJsonAsync(url1, response);
        }

        async static Task SolvePuzzleA3(HttpClient client)
        {
            var url1 = "api/path/1/hard/Puzzle";
            var url2 = "api/path/1/hard/Sample";
            var startUrl = "api/path/1/hard/Start";

            var startResponse = await client.GetAsync(url2);
            var responseBody = await startResponse.Content.ReadAsStringAsync();

            var solved = SolveA3Algo(responseBody);

            //             {
            //    "directions":[
            //       "0 = Up",
            //       "1 = UpRight",
            //       "2 = Right",
            //       "3 = DownRight",
            //       "4 = Down",
            //       "5 = DownLeft",
            //       "6 = Left",
            //       "7 = UpLeft",
            //       "8 = End "
            //    ],
            //    "tiles":[
            //       {
            //          "id":1,
            //          "direction":4,
            //          "x":1,
            //          "y":1
            //       },
            //       {
            //          "id":2,
            //          "direction":2,
            //          "x":2,
            //          "y":1
            //       },
            //       {
            //          "id":3,
            //          "direction":5,
            //          "x":3,
            //          "y":1
            //       },
            //       {
            //          "id":4,
            //          "direction":3,
            //          "x":1,
            //          "y":2
            //       },
            //       {
            //          "id":5,
            //          "direction":3,
            //          "x":2,
            //          "y":2
            //       },
            //       {
            //          "id":6,
            //          "direction":7,
            //          "x":3,
            //          "y":2
            //       },
            //       {
            //          "id":7,
            //          "direction":0,
            //          "x":1,
            //          "y":3
            //       },
            //       {
            //          "id":8,
            //          "direction":1,
            //          "x":2,
            //          "y":3
            //       },
            //       {
            //          "id":9,
            //          "direction":8,
            //          "x":3,
            //          "y":3
            //       }
            //    ]
            // }



        }

        static IList<int> SolveA3Algo(String bruh){
            var walks = new List<int>() { 0 };














            return walks;

        }



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

        // static async void Test()
        // {
        //      // Swagger
        //     // https://involved-htf-challenge.azurewebsites.net/swagger/index.html
        //      
        //     // De httpclient die we gebruiken om http calls te maken
        //     var client = new HttpClient();
        //     // De base url die voor alle calls hetzelfde is
        //     client.BaseAddress = new Uri("http://involved-htf-challenge.azurewebsites.net");
        //      
        //     // De token die je gebruikt om je team te authenticeren, deze kan je via de swagger ophalen met je teamname + password
        //     var token = "yzonlicht";
        //     // We stellen de token in zodat die wordt meegestuurd bij alle calls, anders krijgen we een 401 Unauthorized response op onze calls
        //     client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        //      
        //     // De url om de challenge te starten
        //     var startUrl = "api/path/1/easy/Start";
        //     // We voeren de call uit en wachten op de response
        //     // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/await
        //     // De start endpoint moet je 1 keer oproepen voordat je aan een challenge kan beginnen
        //     // Krijg je een 403 Forbidden response op je Sample of Puzzle calls? Dat betekent dat de challenge niet gestart is
        //     var startResponse = await client.GetAsync(startUrl);
        //      
        //     // De url om de sample challenge data op te halen
        //     var sampleUrl = "api/path/1/easy/Sample";
        //     // We doen de GET request en wachten op de het antwoord
        //     // De response die we verwachten is een lijst van getallen dus gebruiken we List<int>
        //     var sampleGetResponse = await client.GetFromJsonAsync<List<int>>(sampleUrl);
        //      
        //     // Je zoekt het antwoord
        //   //  var sampleAnswer = GetAnswer(sampleGetResponse);
        //      
        //     // We sturen het antwoord met een POST request
        //     // Het antwoord dat we moeten versturen is een getal dus gebruiken we int
        //     // De response die we krijgen zal ons zeggen of ons antwoord juist was
        //    // var samplePostResponse = await client.PostAsJsonAsync<int>(sampleUrl, sampleAnswer);
        //     // Om te zien of ons antwoord juist was moeten we de response uitlezen
        //     // Een 200 status code betekent dus niet dat je antwoord juist was!
        //     var samplePostResponseValue = await samplePostResponse.Content.ReadAsStringAsync();
        //      
        //     // De url om de puzzle challenge data op te halen
        //     var puzzleUrl = "api/path/1/easy/Puzzle";
        //     // We doen de GET request en wachten op de het antwoord
        //     // De response die we verwachten is een lijst van getallen dus gebruiken we List<int>
        //     var puzzleGetResponse = await client.GetFromJsonAsync<List<int>>(puzzleUrl);
        //      
        //     // Je zoekt het antwoord
        //   //  var puzzleAnswer = GetAnswer(puzzleGetResponse);
        //      
        //     // We sturen het antwoord met een POST request
        //     // Het antwoord dat we moeten versturen is een getal dus gebruiken we int
        //     // De response die we krijgen zal ons zeggen of ons antwoord juist was
        //     var puzzlePostResponse = await client.PostAsJsonAsync<int>(sampleUrl, puzzleAnswer);
        //     // Om te zien of ons antwoord juist was moeten we de response uitlezen
        //     // Een 200 status code betekent dus niet dat je antwoord juist was!
        //     var puzzlePostResponseValue = await samplePostResponse.Content.ReadAsStringAsync();
        // }
    }
}