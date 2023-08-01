using mongo1;

namespace mongo2
{
    public class Garage
    {
        public mongo1.City City { get; set; }

        public String address { get; set; }

        public City getCityOfGarage()
        {
            return City;
        }

        public void setCityOfGarage(City city)
        {
            City = city;
        }
        
    }
}
