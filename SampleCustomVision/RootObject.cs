using System;
using System.Collections.Generic;
using System.Text;

namespace SampleCustomVision
{
    public class RootObject
    {
        public string Id { get; set; }
        public string Project { get; set; }
        public string Iteration { get; set; }
        public string Created { get; set; }
        public List<Prediction> Predictions { get; set; }
    }

}
