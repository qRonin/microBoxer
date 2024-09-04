using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleBenchmarks.BoxesApiCalls
{

    [MemoryDiagnoser(false)]
    public class CreationBenchmark
    {
        private BoxesService _boxesService;
        private HttpClient _httpClient;
        public CreationBenchmark()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestVersion = new Version(1, 0);

            _boxesService = new BoxesService(_httpClient);

        }


        [Benchmark]
        public async Task CreateBox()
        {
            await _boxesService.GenerateBoxes(1,0);

        }
        [Benchmark]
        public async Task CreateBoxWithContent()
        {
            await _boxesService.GenerateBoxes(1, 1);
        }
        [Benchmark]
        public async Task CreateMassBoxesWithContents()
        {
            await _boxesService.GenerateBoxes(20, 10);
        }

    }
}
