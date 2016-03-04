using System.Collections.Generic;

namespace Apriori
{
    interface IApriori
    {
        double GetSupport();
        Dictionary<string, double> GenerateCandidates();
        string GenerateCandidate();
        //List<IEnumerable<Item>> 
    }
}