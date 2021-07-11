using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
// Install Newtonsoft.Json with NuGet
using Newtonsoft.Json;
using System.IO;

namespace MIUTranslate
{
    /// <summary>
    /// The C# classes that represents the JSON returned by the Translator Text API.
    /// MIUTRANSLATE.exe /F=EN /T=PT /content=Hello my name is frank
    /// </summary>
    public class TranslationResult
    {
        public DetectedLanguage DetectedLanguage { get; set; }
        public TextResult SourceText { get; set; }
        public Translation[] Translations { get; set; }
    }

    public class DetectedLanguage
    {
        public string Language { get; set; }
        public float Score { get; set; }
    }

    public class TextResult
    {
        public string Text { get; set; }
        public string Script { get; set; }
    }

    public class Translation
    {
        public string Text { get; set; }
        public TextResult Transliteration { get; set; }
        public string To { get; set; }
        public Alignment Alignment { get; set; }
        public SentenceLength SentLen { get; set; }
    }

    public class Alignment
    {
        public string Proj { get; set; }
    }

    public class SentenceLength
    {
        public int[] SrcSentLen { get; set; }
        public int[] TransSentLen { get; set; }
    }

    class Program
    {

        private const string region = "global";
        //private static readonly string region = Environment.GetEnvironmentVariable(region_var);

        //private const string subscriptionKey = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA";
        private static readonly string subscriptionKey = Environment.GetEnvironmentVariable("AZURE_KEY", EnvironmentVariableTarget.User);

        private const string endpoint = "https://api.cognitive.microsofttranslator.com/";
        // private static readonly string endpoint = Environment.GetEnvironmentVariable(endpoint_var);

        static Program()
        {
            if (null == region)
            {
                throw new Exception("Please set/export the environment variable: " + region);
            }
            if (null == subscriptionKey)
            {
                throw new Exception("Please set/export the environment variable: " + subscriptionKey);
            }
            if (null == endpoint)
            {
                throw new Exception("Please set/export the environment variable: " + endpoint);
            }
        }

        // Async call to the Translator Text API
        static public async Task TranslateTextRequest(string subscriptionKey, string endpoint, string route, string inputText, string path)
        {
            object[] body = new object[] { new { Text = inputText } };
            var requestBody = JsonConvert.SerializeObject(body);

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                // Build the request.
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(endpoint + route);
                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                request.Headers.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
                request.Headers.Add("Ocp-Apim-Subscription-Region", region);

                // Send the request and get response.
                HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);
                // Read response as a string.
                string result = await response.Content.ReadAsStringAsync();
                TranslationResult[] deserializedOutput = JsonConvert.DeserializeObject<TranslationResult[]>(result);
                // Iterate over the deserialized results.
                foreach (TranslationResult o in deserializedOutput)
                {
                    // Print the detected input languge and confidence score.
                    //Console.WriteLine("Detected input language: {0}\nConfidence score: {1}\n", o.DetectedLanguage.Language, o.DetectedLanguage.Score);
                    // Iterate over the results and print each translation.
                    foreach (Translation t in o.Translations)
                    {
                        
                        try
                        {
                            // Create the file, or overwrite if the file exists.
                            using (FileStream fs = File.Create(path))
                            {
                                byte[] info = new UTF8Encoding(true).GetBytes(result);
                                // Add some information to the file.
                                fs.Write(info, 0, info.Length);
                            }

                            // Open the stream and read it back.
                            using (StreamReader sr = File.OpenText(path))
                            {
                                string s = "";
                                while ((s = sr.ReadLine()) != null) ;

                            }
                            {
                                Console.WriteLine(t.Text);
                            }

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                    }
                }
            }
        }

        static async Task Main(string[] args)
        {
         //string imputlanguage = "";
         string outputlanguage = "en";
         string textToTranslate = "No text provided";
            string path = @"C:/TEMP/translated.txt";

            foreach (string arg in Environment.GetCommandLineArgs())
            {
                //if(arg.StartsWith("/F" ,true ,System.Globalization.CultureInfo.CurrentCulture))
                //{
                //    string[] from = arg.Split('=');
                //    Console.WriteLine(from[1]);
                //    imputlanguage = from[1];
                //    Console.WriteLine(arg);

                //}
                if (args.Length == 0)
                {
                    Console.WriteLine("Welcome to the amazing translation software");
                    Console.WriteLine("Use /T=ZZ ");
                    Console.WriteLine("Where ZZ is the transdlated to language in ISO code");
                    Console.WriteLine(" ");
                    Console.WriteLine("Use /content=\"any text you need to translate\"");
                    Console.WriteLine(" ");
                    Console.WriteLine("Use /path=\"default to c:/temp/transdlated.txt\"");
                    Console.WriteLine("By PixelbeardQc HEavily guided by CKY \"he basically did most of it, hes amazing\"");
                    return;
                }
                if (arg.StartsWith("/T", true, System.Globalization.CultureInfo.CurrentCulture))
                    {
                        string[] to = arg.Split('=');
                        outputlanguage = to[1];

                    }
                if (arg.StartsWith("/Content", true, System.Globalization.CultureInfo.CurrentCulture))
                {
                    string[] content = arg.Split('=');
                    textToTranslate = content[1];

                }
                if (arg.StartsWith("/path", true, System.Globalization.CultureInfo.CurrentCulture))
                {
                    string[] p = arg.Split('=');
                    path = p[1];

                }
            }
            // This is our main function.
            // Output languages are defined in the route.
            // For a complete list of options, see API reference.
            // https://docs.microsoft.com/azure/cognitive-services/translator/reference/v3-0-translate
            string route = "/translate?api-version=3.0&to="+outputlanguage;
            // Prompts you for text to translate. If you'd prefer, you can
            // provide a string as textToTranslate.
            //Console.Write("Type the phrase you'd like to translate? ");
            //string textToTranslate = Console.ReadLine();
            await TranslateTextRequest(subscriptionKey, endpoint, route, textToTranslate, path);
            //Console.WriteLine("Press any key to continue.");
            //Console.ReadKey();

        }
    }
}