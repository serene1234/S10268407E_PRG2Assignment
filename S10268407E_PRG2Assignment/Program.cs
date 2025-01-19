//==========================================================
// Student Number	: S10268407E
// Student Name	: Serene Ker Xin Yun
// Partner Name	: Shawntrice Yip Yin Fei
//==========================================================
using S10268407E_PRG2Assignment;

//Feature 1
Dictionary<string, Airline> airlineDict = new Dictionary<string, Airline>();
Dictionary<string, BoardingGate> boardingGateDict = new Dictionary<string, BoardingGate>();

void CreateAirlines(Dictionary<string, Airline> airlineDict)
{
    using (StreamReader sr = new StreamReader("airlines.csv"))
    {
        // Read the header line
        string? s = sr.ReadLine();
        // Read the data lines
        while ((s = sr.ReadLine()) != null)
        {
            string[] data = s.Split(',');
            string name = data[0];
            string code = data[1];
            airlineDict.Add(code, new Airline(name, code));
        }
    }
}
void CreateBoardingGates(Dictionary<string, BoardingGate> bGateDict)
{
    using (StreamReader sr = new StreamReader("boardinggates.csv"))
    {
        // Read the header line
        string? s = sr.ReadLine();
        // Read the data lines
        while ((s = sr.ReadLine()) != null)
        {
            string[] data = s.Split(',');
            string gateName = data[0];
            bool supportsCFFT = Convert.ToBoolean(data[1]);
            bool supportsDDJB = Convert.ToBoolean(data[2]);
            bool supportsLWTT = Convert.ToBoolean(data[3]);
            bGateDict.Add(gateName, new BoardingGate(gateName, supportsCFFT, supportsDDJB, supportsLWTT));
        }
    }
}