using FuzzyFramework.Dimensions;
using FuzzyFramework.Sets;

namespace MSI
{
    public class FuzzyParameter
    {
        public FuzzySet NegativeSet { get; set; }
        public FuzzySet PositiveSet { get; set; }
        public Dimension Dimension { get; set; }
    }
    public class FuzzyParameterEx
    {
        public FuzzySet NegativeSet { get; set; }
        public FuzzySet MiddleSet { get; set; }
        public FuzzySet PositiveSet { get; set; }
    }
}
