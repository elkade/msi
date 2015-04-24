using System.Collections.Generic;
using FuzzyFramework;
using FuzzyFramework.Defuzzification;
using FuzzyFramework.Dimensions;
using FuzzyFramework.Sets;

namespace MSI
{
    class FuzzyReasoning
    {
        private readonly FuzzyParameter _rainParam;
        private readonly FuzzyParameter _temperatureParam;
        private readonly FuzzyParameter _visibilityParam;
        private readonly FuzzyParameter _hourParam;

        public FuzzyReasoning(FuzzyParameter rainParam, FuzzyParameter temperatureParam, FuzzyParameter visibilityParam, FuzzyParameter hourParam)
        {
            _rainParam = rainParam;
            _temperatureParam = temperatureParam;
            _visibilityParam = visibilityParam;
            _hourParam = hourParam;
        }

        // public decimal Work(decimal rain, decimal visibility, decimal temperature, decimal hour)
        public decimal Work(decimal rainValue, decimal temperatureValue)
        {
            //Definition of dimensions on which we will measure the input values
            //ContinuousDimension height = new ContinuousDimension("Height", "Personal height", "cm", 100, 250);
            //ContinuousDimension weight = new ContinuousDimension("Weight", "Personal weight", "kg", 30, 200);

            //Definition of dimension for output value
            ContinuousDimension consequent = new ContinuousDimension("Suitability for basket ball", "0 = not good, 5 = very good", "grade", 0, 5);

            //Definition of basic fuzzy sets with which we will work
            //  input sets:
            //FuzzySet tall = new LeftQuadraticSet(height, "Tall person", 150, 180, 200);
            //FuzzySet weighty = new LeftLinearSet(weight, "Weighty person", 80, 100);
            //_tall = new LeftQuadraticSet(height, "Tall person", 150, 180, 200);
            //_weighty = new LeftLinearSet(weight, "Weighty person", 80, 100);

            //  output set:
            FuzzySet goodConditions = new LeftLinearSet(consequent, "Good in basket ball", 0, 5);



            //Implication
            //FuzzyRelation term =/* ((tall & !weighty) & goodForBasket); |*/ (!(tall & !weighty) & !goodForBasket);
            //FuzzyRelation term = ((_rainParam.PositiveSet & !_temperatureParam.PositiveSet) & goodForBasket) | ((!_rainParam.PositiveSet & _temperatureParam.PositiveSet) & !goodForBasket);
            FuzzyRelation term = ((_rainParam.NegativeSet & _temperatureParam.PositiveSet) & goodConditions);

            Defuzzification result = new MeanOfMaximum(
                term,
                new Dictionary<IDimension, decimal>{
                    { _rainParam.PositiveSet.Dimensions[0], rainValue },
                    { _rainParam.NegativeSet.Dimensions[0], rainValue },
                    { _temperatureParam.PositiveSet.Dimensions[0], temperatureValue },
                    { _temperatureParam.NegativeSet.Dimensions[0], temperatureValue },
                    //{ _visibilityParam.PositiveSet.Dimensions[0], visibilityValue },
                    //{ _visibilityParam.NegativeSet.Dimensions[0], visibilityValue },
                    //{ _hourParam.PositiveSet.Dimensions[0], hourValue },
                    //{ _hourParam.NegativeSet.Dimensions[0], hourValue },
                }
            );
            return result.CrispValue;
        }
    }
}
