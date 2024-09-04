using MicroBoxer.Web.Services;
using System.Xml.Linq;

namespace MicroBoxer.Web.Extensions
{
    public static class DataGenerator
    {

        public static async Task GenerateBoxes(this BoxesService service, int numberOfBoxes, int numberOfContentsPerBox)
        {
            int counter = 0;

            Guid BoxId = Guid.NewGuid();
            string BoxName;

            for (counter = 0; counter < numberOfBoxes; counter++)
            {
                BoxName = $"Box Name number{counter}";

                await SendCreateBoxRequest(service, BoxName);

                //if(result)
                //await GenerateContentsForBox(service, numberOfContentsPerBox, BoxId);

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

        public static async Task SendCreateBoxRequest(this BoxesService service, string BoxName)
        {
            try
            {
                var request = new CreateBoxRequest(Guid.NewGuid().ToString(), BoxName, new List<BoxContentRecord>());
                var result = await service.CreateBox(request, Guid.NewGuid());
                Console.WriteLine(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        public static async Task SendCreateBoxContentRequest(this BoxesService service, string BoxId,
            string BoxContentName, string BoxContentDescription)
        {
            try
            {
                var request = new CreateBoxContentRequest(BoxId, BoxContentName, BoxContentDescription);
                var result = await service.CreateBoxContent(request, Guid.NewGuid());
                Console.WriteLine(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

    }
}
