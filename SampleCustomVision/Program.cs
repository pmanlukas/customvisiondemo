using System;
using System.Data;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace SampleCustomVision
{
    class Program
    {
        //authorization data for our prediction model
        public static string predictionKey = "42441a607d184fceb34a7c33d74e5d34";

        public static string predictionURL = "https://southcentralus.api.cognitive.microsoft.com/customvision/v1.0/Prediction/d620aff4-8b4e-49ac-b15c-871e28b0cc5a/image?iterationId=496ca037-c5cb-40a8-a112-086b34d6e291";

        public static string predictionContent = "";

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the fcb predictor!");
            Console.Write("Enter the file path: ");
            String imageFilePath = Console.ReadLine();

            MakePredictionRequest(imageFilePath).Wait();

            //TODO: Implement JSON deserilization and display just result of prediction


            var root = JsonConvert.DeserializeObject<RootObject>(predictionContent);
            var prediction = root.Predictions;
            foreach (var predict in prediction)
            {
                Console.WriteLine("Tag: {0}, Probability: {1}", predict.Tag, predict.Probability);
            }

            Console.WriteLine("\n\n\nHit Enter to exit...");
            Console.ReadLine();

            //use: C:\Users\lupollma\Desktop\class test\trikotOne.jpg

        }

        static byte[] GetImageAsByteArray(String imageFilePath)
        {
            FileStream fileStream = new IsolatedStorageFileStream(imageFilePath, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            return binaryReader.ReadBytes((int) fileStream.Length);
        }

        static async Task MakePredictionRequest(string imageFilePath)
        {
            var client = new HttpClient();

            //Request Header with subscription key
            client.DefaultRequestHeaders.Add("Prediction-Key", predictionKey);

            //http response object 
            HttpResponseMessage response;

            //Request body
            byte[] byteData = GetImageAsByteArray(imageFilePath);

            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/FCBclassify");
                response = await client.PostAsync(predictionURL, content);

                predictionContent = await response.Content.ReadAsStringAsync();
            }
        }
    }
}
