namespace Customer.Domain.Entites
{
    public class Address
    {

        public string streetAddress {get; internal set;}

        public Address(string streetAddress) 
        {
            this.streetAddress = streetAddress;
        }


    }
}