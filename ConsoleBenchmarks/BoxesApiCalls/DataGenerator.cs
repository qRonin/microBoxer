using ConsoleBenchmarks.BoxesApiCalls;
using ConsoleBenchmarks.BoxesApiCalls.Model;
using Microsoft.IdentityModel.Tokens;
using System.Xml.Linq;

namespace ConsoleBenchmarks.BoxesApiCalls
{
    public static class DataGenerator
    {

        public static async Task GenerateBoxes(this BoxesService service
            ,int numberOfBoxes, int numberOfContentsPerBox)
        {
                int counter = 0;
                IDisposable? boxesChangedSubscription;
                string BoxName;

            for (counter = 0; counter < numberOfBoxes; counter++)
            {
                BoxName = $"Box Name number{counter}";

                var result = await SendCreateBoxRequest(service, BoxName);
                
                if(result!=null)
                await GenerateContentsForBox(service, numberOfContentsPerBox, (Guid)result);
            }

             await Task.CompletedTask;
        }

        public static async Task GenerateContentsForBox(this BoxesService service, int numberOfContents, Guid boxId)
        {
            int counter = 0;
            string BoxId = boxId.ToString();
            string BoxContentName;
            string BoxContentDescription;
                
            for (counter = 0; counter < numberOfContents; counter++)
            {
                 BoxContentName = $"Content Name number{counter}";
                 BoxContentDescription = $"Content Desc number{counter}";
                 await SendCreateBoxContentRequest(service, BoxId, BoxContentName, BoxContentDescription);

            }

            await Task.CompletedTask;
        }

        public static async Task<Guid?> SendCreateBoxRequest(this BoxesService service, string BoxName)
        {
            Guid? result;
            try
            {
                var request = new CreateBoxRequest(Guid.NewGuid().ToString(), BoxName, new List<BoxContentRecord>());
                result = await service.CreateBox(request, Guid.NewGuid());
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return null;

        }
        public static async Task<HttpResponseMessage?> SendCreateBoxContentRequest(this BoxesService service, string BoxId,
            string BoxContentName, string BoxContentDescription)
        {
            HttpResponseMessage result;
            try
            {
                var request = new CreateBoxContentRequest(BoxId, BoxContentName, BoxContentDescription);
                result = await service.CreateBoxContent(request, Guid.NewGuid());
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return null;
        }
        public static async Task<List<Box>> GetAllBoxes(this BoxesService service)
        {
            List<BoxRecord> boxes = new List<BoxRecord>();
            List<Box> BoxList = new List<Box>();
            var result = await service.GetBoxes();
            boxes = result.ToList();
            foreach (var box in boxes)
            {
                BoxList.Add(await box.FromRecord());
            }
            return BoxList;
        }
        public static async Task<Box> GetBox(this BoxesService service)
        {
            List<BoxRecord> boxes = new List<BoxRecord>();
            List<Box> BoxList = new List<Box>();
            var result = await service.GetBoxes();
            boxes = result.ToList();
            foreach (var box in boxes)
            {
                BoxList.Add(await box.FromRecord());
            }
            return BoxList.FirstOrDefault();
        }
    }
}
