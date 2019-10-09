using Services.Common.Commands;
namespace Customer.App.UseCases
{
    public class RegisterCustomerCommandMessage : ICommand
    {

        public string FirstName { get; internal set; }
        public string LasrName { get; internal set; }
        public string Address { get; internal set; }
        public string PhoneNumber { get; internal set; }
        public string Email { get; internal set; }


        protected RegisterCustomerCommandMessage ()
        {

        }
        public RegisterCustomerCommandMessage(string firstName, string lastName, string email, 
                                                    string phoneNumber, string address)
        {
            FirstName = firstName;
            LasrName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            Address = address;
        }
    }
}