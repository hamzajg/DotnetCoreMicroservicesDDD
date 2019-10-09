namespace Customer.App.Domain
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