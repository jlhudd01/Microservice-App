using System.Runtime.Serialization;
using MediatR;

namespace ProductWebAPI.Commands
{
    [DataContract]
    public class DeleteProductCommand : IRequest<bool>
    {
        [DataMember]
        public int Id {get;set;}
        [DataMember]
        public string Name {get;set;}
        [DataMember]
        public decimal Price {get;set;}

        public DeleteProductCommand() { }

        public DeleteProductCommand(int id, string name, decimal price)
        {
            Id = id;
            Name = name;
            Price = price;
        }
    }
}