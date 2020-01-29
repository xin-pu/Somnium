using System.Collections.Generic;
using System.Linq;

namespace Somnium.Data
{
    public class LabelMap
    {
        private Dictionary<string, int> Dict { set; get; }

        public int Count => Dict.Count;

        public LabelMap(IList<string> labels)
        {
            CreateMap(labels);
        }

        public LabelMap(IEnumerable<string> labels)
        {
            CreateMap(labels.ToArray());
        }

        public string GetLabel(int value)
        {
            return Dict.First(a => a.Value == value).Key;
        }

        public int GetValue(string label)
        {
            return Dict.First(a => a.Key == label).Value;
        }

        public double[] GetCorrectResult(string label)
        {
            var value = GetValue(label);
            var array = new double[Dict.Count];
            array[value] = 1;
            return array;
        }


        private void CreateMap(IList<string> labels)
        {
            Dict = new Dictionary<string, int>();
            var i = 0;
            labels.Distinct().ToList().ForEach(label =>
            {
                Dict[label] = i;
                i++;
            });
        }

    }
}
