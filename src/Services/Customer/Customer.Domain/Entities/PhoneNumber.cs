namespace Customer.Domain.Entites
{
    public class PhoneNumber
    {
        public string PhoneNum {get; internal set;}

        public PhoneNumber(string phoneNumber)
        {
            this.PhoneNum = phoneNumber;
        }
    }
}