using System.Runtime.Serialization;
using MediatR;

namespace ProductWebAPI.Commands
{
    [DataContract]
    public class CreateProductCommand : IRequest<bool>
    {
        [DataMember]
        public string Name { get; private set; }
        [DataMember]
        public decimal Price { get; private set; }

        public CreateProductCommand() {}

        public CreateProductCommand(string name, decimal price)
        {
            Name = name;
            Price = price;
        }
    }
}