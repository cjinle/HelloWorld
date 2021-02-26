using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;

namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Test t = new Test();
            t.Echo();
            Test.String();
            Test.List();
            Test.Replace();

            string s = "cjinle";
            Console.WriteLine($"Hello {s}, xxxx.");

            MyJson.test();

            Network.GetAsync("http://192.168.56.101:8080/api");
            Network.Get("http://192.168.56.101:8080/api");
            Network.Post("http://192.168.56.101:8080/api");

            Network.Stream();

            Console.ReadKey();
            
        }
    }

    class Movie
    {
        public string Name { get; set; }
        public DateTime ReleaseDate { get; set; }

        public string[] Genres { get; set; }
    }

    class MyJson
    {
        public static void test()
        {
            string json = @"{
                'Name': 'Bad Boys',
                'ReleaseDate': '1995-4-7T00:00:00',
                'Genres': [
                    'Action',
                    'Comedy'
                ]
            }";

            Movie m = JsonConvert.DeserializeObject<Movie>(json);

            Console.WriteLine(m.Name);
            Console.WriteLine(m.ReleaseDate);
            Console.WriteLine(m.Genres[0]);
            Console.WriteLine(m.Genres[1]);

            string jsonStr = JsonConvert.SerializeObject(m);
            Console.WriteLine(jsonStr);
        }
    }

    class Test
    {
        enum Day { Zero, One, Two };
        struct Books
        {
            public string title;
            public int book_id;
        };
        public int Attr1 { get; set; }

        public void Echo()
        {
            Console.WriteLine("class test func echo call!");
            Books book1;
            book1.book_id = 1;
            book1.title = "hello";

            Books[] bb = new Books[] { book1 };
            Console.WriteLine(bb.Length);

            Attr1 = 0;
            Attr1 += 100;
        }

        public static void String()
        {
            StringBuilder sb = new StringBuilder("ABC", 50);
            sb.Append(" ");
            sb.Append("hello");
            Console.WriteLine("string builder lenght: {0}, content: {1}", sb.Length, sb.ToString());
        }

        public static void List()
        {
            List<string> sl = new List<string>();
            sl.Add("one");
            sl.Add("two");
            sl.Add("three");
            Console.WriteLine(sl[0]);
            Console.WriteLine(sl.Count);
        }

        public static void Replace()
        {
            string source = "aaa bbb ccc ddd";
            var replacement = source.Replace("ccc", "xxx");
            Console.WriteLine($"source is <{source}>");
            Console.WriteLine($"updated is <{replacement}>");

            object xx = source.Clone();
            Console.WriteLine("clone string {0}", xx.ToString());

            char[] cArr = source.ToCharArray();
            cArr[0] = '*';
            cArr[1] = '*';
            cArr[2] = '*';
            string s2 = new string(cArr);
            Console.WriteLine(s2);
        }

    }

    class Network
    {
        public async static void GetAsync(string url)
        {
            using var client = new HttpClient();

            var result = await client.SendAsync(new HttpRequestMessage(HttpMethod.Head, url));
            Console.WriteLine("----------- async -------------");
            Console.WriteLine(result);
        }

        public static void Get(string url) 
        {
            using var client = new HttpClient();

            var result = client.Send(new HttpRequestMessage(HttpMethod.Get, url));
            Console.WriteLine("---------- sync -------------");
            Console.WriteLine(result.Content.ReadAsStringAsync().Result);
        }

        public async static void Post(string url)
        {
            using var client = new HttpClient();
            
            var data = new StringContent("500");
            var response = await client.PostAsync(url, data);
            string result = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine("---------- async post -------------");
            Console.WriteLine(result);
        }

        public static void Stream()
        {
            var client = new TcpClient("192.168.56.101", 1234);
            try 
            {
                var ns = client.GetStream();
                var bs = Encoding.ASCII.GetBytes("500");
        
                int num = 0;
                while (num < 10)
                {
                    num++;
                    ns.Write(bs, 0, bs.Length);
                    var recv = new byte[1024];
                    int len = ns.Read(recv, 0, recv.Length);
                    Console.WriteLine(Encoding.ASCII.GetString(recv, 0, len));
                }
                client.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
