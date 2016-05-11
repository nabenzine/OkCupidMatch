using System.Collections.Generic;


namespace OkCupid.JsonUtils
{
    class JsonCustomWriter
    {
        public List<Result> results { get; set; }

        public JsonCustomWriter()
        {
            results = new List<Result>();
        }

        public void AddResult(Result result)
        {
            results.Add(result);
        }
    }
}
