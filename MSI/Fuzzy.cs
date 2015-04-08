using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FuzzyFramework;
using FuzzyFramework.Defuzzification;
using FuzzyFramework.Dimensions;
using FuzzyFramework.Graphics;
using FuzzyFramework.Sets;

namespace MSI
{
    class FuzzyReasoning
    {
        // public decimal Work(decimal rain, decimal visibility, decimal temperature, decimal hour)
        public decimal Work(decimal inputHeight, decimal inputWeight)
        {
            //Definition of dimensions on which we will measure the input values
            ContinuousDimension height = new ContinuousDimension("Height", "Personal height", "cm", 100, 250);
            ContinuousDimension weight = new ContinuousDimension("Weight", "Personal weight", "kg", 30, 200);

            //Definition of dimension for output value
            ContinuousDimension consequent = new ContinuousDimension("Suitability for basket ball", "0 = not good, 5 = very good", "grade", 0, 5);

            //Definition of basic fuzzy sets with which we will work
            //  input sets:
            FuzzySet tall = new LeftQuadraticSet(height, "Tall person", 150, 180, 200);
            FuzzySet weighty = new LeftLinearSet(weight, "Weighty person", 80, 100);
            //  output set:
            FuzzySet goodForBasket = new LeftLinearSet(consequent, "Good in basket ball", 0, 5);



            //Implication
            //FuzzyRelation term =/* ((tall & !weighty) & goodForBasket); |*/ (!(tall & !weighty) & !goodForBasket);
            FuzzyRelation term = ((tall & !weighty) & goodForBasket) | ((!tall & weighty) & !goodForBasket);

            Defuzzification result = new MeanOfMaximum(
                term,
                new Dictionary<IDimension, decimal>{
                    { height, inputHeight },
                    { weight, inputWeight }
                }
            );
            return result.CrispValue;
        }
    }
}
